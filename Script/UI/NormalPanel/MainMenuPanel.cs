

using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


namespace FPS
{
    public class MainMenuPanel :BaseUIPanel,IController
    {
     
        public Button startButton;
        public Button rangeButton;
        public Button exitButton;



        protected override void Awake()
        {
           
            Level = PanelLevel.One;
            Type = PanelType.NormalPanel;
            base.Awake();
            InitFindGameObject();
        }

        private void InitFindGameObject()
        {
            startButton = GameTools.GetSingleComponentInChild<Button>(gameObject, "StartButton");
            rangeButton = GameTools.GetSingleComponentInChild<Button>(gameObject, "RangeButton");
            exitButton = GameTools.GetSingleComponentInChild<Button>(gameObject, "ExitButton");

            startButton.AddComponent<ButtonSoundController>();
            rangeButton.AddComponent<ButtonSoundController>();
            exitButton.AddComponent<ButtonSoundController>();
        }
        
        private void ListenEvent()
        {
            //startButton.OnPointerEnter.AddListener(OnPointerEnter);
           
            startButton.onClick.AddListener(OnClicked_StartGame);
            rangeButton.onClick.AddListener(OnClicked_GoRange);
            exitButton.onClick.AddListener(OnClicked_GoExit);
            
        }
        // 鼠标进入按钮区域时调用
        void OnPointerEntere(PointerEventData eventData)
        {
            // 显示文字
           
        }

        private void Start()
        {
            ListenEvent();
        }

        private void OnClicked_StartGame()
        {
           
            IC.LoadScene(1);
        }
        private void OnClicked_GoRange()
        {
          
            IC.LoadScene(2);
        }
        private void OnClicked_GoExit()
        {
            
            Application.Quit();
        }

     
    }
}
