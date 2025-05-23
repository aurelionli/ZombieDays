using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.UI;
namespace FPS {
    public class MapManager : MonoBehaviour, IController
    {
        private IController IC;
        private OcTreeController octree;

        private List<GameObject> activeObjects = new List<GameObject>(); // 当前显示的物体

        public int showSpeed;

        public int hideSpeed;

        public int checkSpeed;

        private Scene_SO data;

        public float waitTime;

        public Vector3 fanwei;

        public bool enableDynamicLoading;

        private void Awake()
        {
            IC =this;
            data = IC.GetData<Scene_SO>();
      
            showSpeed = data.ShowSpeed;
            hideSpeed = data.HideSpeed;
            checkSpeed = data.CheckSpeed;
            fanwei = data.Fanwei;
            waitTime = data.waitTime;

        }
        private void Start()
        {
            // 初始化八叉树
            octree = new OcTreeController(new Bounds(new Vector3(0, 0, 0), new Vector3(2000, 2000, 1000)), 4);

            // 插入场景中的物体
            foreach (var obj in GameObject.FindGameObjectsWithTag("scene"))
            {
                // Debug.Log("插入");

                obj.gameObject.SetActive(false);

                octree.Insert(obj);
            }
            // 启动协程
            StartCoroutine(UpdateSceneObjects());
            // ToggleDynamicLoading(true);
            // 根据初始状态启动或关闭动态加载
            /*   if (enableDynamicLoading)
               {
                   // 开启动态加载
                   StartCoroutine(UpdateSceneObjects());
                   Debug.Log("动态加开启");
               }
               else
               {
                   // 关闭动态加载并显示所有物体
                   ShowAllObjects();
                   Debug.Log("动态已关闭");
               }*/
        }
        // 在Inspector中动态切换开关

        private IEnumerator UpdateSceneObjects()
        {
            while (true)
            {
                // 查询玩家周围的物体
                Vector3 playerPos = transform.position;
                Bounds queryRange = new Bounds(playerPos, fanwei);
                // var nearbyObjects = octree.Query(queryRange);
                List<GameObject> nearbyObjects = new List<GameObject>();
                //Debug.Log("查询");
                yield return StartCoroutine(octree.QueryOverFrames(queryRange, nearbyObjects, checkSpeed));
                //  yield return new WaitForSeconds(waitTime);

                // Debug.Log("物体数量：" + nearbyObjects.Count);
                //  Debug.Log("显示");
                yield return StartCoroutine(ShowObjectsOverFrames(nearbyObjects));
                // 隐藏之前显示但不再在查询范围内的物体
                // Debug.Log("隐藏");
                yield return StartCoroutine(HideInactiveObjectsOverFrames(nearbyObjects));
                //    yield return new WaitForSeconds(waitTime);
                // 分帧显示附近的物体



                /* Debug.Log($"范围内物体有{nearbyObjects.Count}个");
                 // 隐藏之前显示但不再在查询范围内的物体
                 yield return StartCoroutine(HideInactiveObjectsOverFrames(nearbyObjects));
                // yield return new WaitForSeconds(0.1f);

                 // 分帧显示附近的物体
                 yield return StartCoroutine(ShowObjectsOverFrames(nearbyObjects));*/

                // 每隔2秒执行一次
              //  yield return new WaitForSeconds(waitTime);
            }
        }

        private IEnumerator ShowObjectsOverFrames(List<GameObject> nearbyObjects)
        {
            // 每帧显示的物体数量
            int objectsPerFrame = showSpeed;

            for (int i = 0; i < nearbyObjects.Count; i += objectsPerFrame)
            {
                // 显示当前帧的物体
                for (int j = i; j < i + objectsPerFrame && j < nearbyObjects.Count; j++)
                {
                    GameObject obj = nearbyObjects[j];
                    //    Debug.Log("Nearby object: " + obj.name);
                    obj.gameObject.SetActive(true);

                    activeObjects.Add(obj);
                }

                // 等待下一帧
                yield return null;
            }
        }
        // 显示所有物体的方法
        public void ShowAllObjects()
        {
            /*foreach (var obj in GameObject.FindGameObjectsWithTag("scene"))
            {
                obj.SetActive(true);
            }*/
            // 找到所有物体（包括禁用的）
            GameObject[] allObjects = Resources.FindObjectsOfTypeAll<GameObject>();

            // 过滤出带有"scene"标签的物体
            foreach (var obj in allObjects)
            {
                if (obj.CompareTag("scene"))
                {
                    Debug.Log("找到物体: " + obj.name);
                    obj.SetActive(true); // 禁用物体
                }
            }
        }
        public void ToggleDynamicLoading(bool enable)
        {
            if (enable != enableDynamicLoading)
            {
                enableDynamicLoading = enable;

                if (enable)
                {
                    // 启动协程
                    StartCoroutine(UpdateSceneObjects());
                    Debug.Log("动态加载已开启");
                }
                else
                {
                    // 停止协程并显示所有物体
                    StopAllCoroutines();
                    ShowAllObjects();
                    Debug.Log("动态加载已关闭");
                }
            }
        }



        private void Update()
        {
        }
        // 隐藏不再在查询范围内的物体
        private IEnumerator HideInactiveObjectsOverFrames(List<GameObject> nearbyObjects)
        {
            // 使用HashSet来提高查询效率
            HashSet<GameObject> nearbySet = new HashSet<GameObject>(nearbyObjects);

            // 每帧处理的物体数量
            int objectsPerFrame = hideSpeed;

            // 从后往前遍历activeObjects，找出需要隐藏的物体并分帧处理
            for (int i = activeObjects.Count - 1; i >= 0; i -= objectsPerFrame)
            {

                // 处理当前帧的物体
                for (int j = i; j > i - objectsPerFrame && j >= 0; j--)
                {
                    GameObject obj = activeObjects[j];

                    // 如果物体不在查询范围内，隐藏它
                    if (!nearbySet.Contains(obj))
                    {
                        obj.gameObject.SetActive(false);
                        activeObjects.RemoveAt(j); // 从activeObjects中移除

                    }
                }


                yield return null;

            }




        }
    }
}