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
        //ȱ�㣬ϵͳû����һ���Լ���ȫ����Ӧ�ð�����ء�
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
                 // Debug.Log("�¼�ϵͳ��ʼ��");
                  EventSystem = new EventsSystem();

                // playerControls = new PlayerControls();
                //playerControls.Enable();
                inputManager = new PlayerInputManager();

                CommandManager = GetComponent<CommandManager>(); 
                 // Debug.Log("���ݹ����ʼ��");
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