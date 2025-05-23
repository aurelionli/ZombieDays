using BehaviorDesigner.Runtime.Tasks.Unity.UnityPlayerPrefs;
using FPS;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//public enum Scene { MainMenu=0,GameMap=1,Range=2} //主界面，游戏，靶场
namespace FPS_Manager
{
    public class SceneSystem : MonoBehaviour, ISystem
    {
        public ISystem IC;
        public GameObject playerPrefab;
        public bool isSceneLoading;
        public Scene_SO data;
        private void Awake()
        {
            IC = this;
            playerPrefab = Resources.Load<GameObject>("Player/Player");
            data = IC.GetData<Scene_SO>();
 
        }
        private void Start()
        {
            

        }
        public void LoadScene(int sceneName)
        {
            if (isSceneLoading)
            {
                return;
            }
            isSceneLoading = true;
            IC.RemoveObjectPoolGameObject();
            StartCoroutine(LoadSceneAsync(sceneName));
        }

        //TODO:加载不同的场景
        private IEnumerator LoadSceneAsync(int sceneName)
        {


            //3.关闭所有面板
            IC.CloseAllPanel();
            //4.打开加载场景面板
            IC.OpenPanel<LoadPanel>();
            yield return new WaitForSeconds(3f);
            IC.StopBGMusic();
            //1.建立一个异步加载场景
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
            //2.禁止一加载完场景就显示
            asyncLoad.allowSceneActivation = false;
            //5.循环加载
            while (!asyncLoad.isDone)
            {
                float progress = Mathf.Clamp01(asyncLoad.progress / 0.9f);
                if (progress >= 1.0f)
                {
                    asyncLoad.allowSceneActivation = true;
                }
                yield return null;
            }
          
            if(SceneManager.GetActiveScene().buildIndex == 0)
            {
                IC.PlayerBGMusic("Dance", true, AudioClipType.Normal);
            }
            yield return new WaitForSeconds(2f);
            //6.关闭加载场景面板
            IC.CloseAllPanel();



            if (SceneManager.GetActiveScene().buildIndex == 1)
            {
                //8.打开game面板
                IC.OpenPanel<GamePanel>();
                Cursor.lockState = CursorLockMode.Locked;//鼠标锁定并隐藏

     
                GameObject player = Instantiate(playerPrefab, data.spawnPoint, Quaternion.identity);
                IC.PlayerBGMusic("BackGround", true, AudioClipType.Normal);
                GameObject emptyGameObject = new GameObject("MapController");
                emptyGameObject.AddComponent<MapManager>();
                emptyGameObject.transform.SetParent(player.transform);
                emptyGameObject.transform.localPosition = new Vector3(0f, 0f, 25f);

            }
            else if(SceneManager.GetActiveScene().buildIndex == 2)
            {
                IC.OpenPanel<GamePanel>();
                Cursor.lockState = CursorLockMode.Locked;//鼠标锁定并隐藏

             
                GameObject player = Instantiate(playerPrefab, data.rangePoint, Quaternion.identity);
                IC.PlayerBGMusic("BackGround", true, AudioClipType.Normal);
               
            }
            else
            {
                

                Cursor.lockState = CursorLockMode.None;//鼠标解锁并显示
                IC.OpenPanel<MainMenuPanel>();
            }
            //9.加载完毕
            isSceneLoading = false;

        
        }


     }
 }

