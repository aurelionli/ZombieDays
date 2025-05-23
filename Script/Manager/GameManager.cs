using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using FPS;
using Unity.VisualScripting;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
namespace FPS_Manager
{
    public class GameManager : MonoBehaviour
    {
        //缺点，系统没有是一次性加载全部，应该按需加载。
        private static GameManager _instance;
        public static GameManager Instance { get { return _instance; } }
        private bool isInitialized = false;

        public UiManager UIManager;
        public CommandManager CommandManager;
        public ModelManager ModelManager;

        public BuffSystem BuffController;
        public AudioSystem AudioSystem;
        public EventsSystem EventSystem;

        //public PlayerControls playerControls;
        public PlayerInputManager inputManager;
        public ObjectPoolSystem objectPoolSystem;

        public SceneSystem sceneSystem;

        public DialogueSystem dialogueSystem;
        
        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(this.gameObject);
                return;
            }
            else
            {
                _instance = this;
                DontDestroyOnLoad(gameObject);
            }
           
              if (!isInitialized)
              {
                 // Debug.Log("事件系统初始化");
                  EventSystem = new EventsSystem();

                // playerControls = new PlayerControls();
                //playerControls.Enable();
                inputManager = new PlayerInputManager();

                CommandManager = GetComponent<CommandManager>(); 
                 // Debug.Log("数据管理初始化");
                  ModelManager = new ModelManager();
                objectPoolSystem = new ObjectPoolSystem();
                  BuffController = GetComponent<BuffSystem>();
                  sceneSystem = GetComponent<SceneSystem>();
                  AudioSystem = GetComponent<AudioSystem>();
                  UIManager = GetComponent<UiManager>();
                dialogueSystem = new DialogueSystem();
                  isInitialized = true;
              }
            

        }

        private void Start()
        {
           if( SceneManager.GetActiveScene().buildIndex == 0)
            {
                UIManager.OpenPanel<MainMenuPanel>();

            }
           else
            {
                UIManager.OpenPanel<GamePanel>();
            }
            // UIManager.OpenPanel("GamePanel");
            // UIManager.OpenPanel<MainMenuPanel>();
            //UIManager.OpenPanel<GamePanel>();

        }
    }
}