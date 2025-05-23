using DG.Tweening;
using FPS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.EventSystems.EventTrigger;

namespace FPS
{
    public class DialoguePanel: BaseUIPanel
    {
        private TMP_Text DialogueText;
        private Button continuteButton;

        private Transform buttonGroup;
        private GameObject buttonPrefab;

        private DialogueNode optionsCallbackNode;
        protected override void Awake()
        {
            Cursor.lockState = CursorLockMode.None;//鼠标解锁并显示
            Level = PanelLevel.Two;
            Type = PanelType.NormalPanel;
            base.Awake();
            InitFindGameObject();

            IC.RegisterEvent("GeneraOptions");
            IC.StartListening("GeneraOptions", TriggerEvent_GenerateOptions);


            IC.RegisterEvent("ShowDialogueText");
            IC.StartListening("ShowDialogueText", TriggerEvent_ShowDialogueText);

            buttonPrefab = Resources.Load<GameObject>("ChooseButton");

        }
      
        /// <summary>
        /// emm，为了方便辨认，提供个函数。
        /// </summary>
        /// <param name="obj"></param>
        private void TriggerEvent_ShowDialogueText(object obj)
        {
            ShowText(obj as DialogueNode);

        }

        private void InitFindGameObject()
        {
            continuteButton = GameTools.GetSingleComponentInChild<Button>(gameObject, "DialogueBackGround");
            DialogueText = GameTools.GetSingleComponentInChild<TMP_Text>(gameObject, "DialogueText");
            buttonGroup = GameTools.GetSingleComponentInChild<Transform>(gameObject, "ButtonGroup");
            continuteButton.interactable = false;
            DialogueText.text = "";
            continuteButton.onClick.AddListener(() => { IC.TriggerEvent("ContinueDialogue"); });
        }



        /// <summary>
        /// 这个是提供生成按钮的功能，给类型Options的语句使用。
        /// </summary>
        /// <param name="data"></param>
        public void TriggerEvent_GenerateOptions(object data)
        {
            continuteButton.interactable = false;
            Debug.Log("这里是对话面板的生成按钮");
            DialogueNode node = data as DialogueNode;
            Button option = Instantiate(buttonPrefab, buttonGroup).GetComponent<Button>();
            option.transform.GetChild(0).GetComponent<TMP_Text>().text = node.Content;

            switch(node.defaultNextID)
            {//options的对话中，defaultNextID是来判断这个选项，会打开其他面板，还是只是单纯的改变分支
                case 0://IC.TriggerEvent("OptionButtonOnClicked", node.defaultNextID); 这个东西可以放在Exit里面，
                    option.onClick.AddListener(() => { node.TriggerEnter();  CleanAllButton(); });
                    break;
                case 1:
                    option.onClick.AddListener(() => { node.TriggerEnter(); optionsCallbackNode = node;  CleanAllButton(); });
                    break;
            }



          
        }
        public void CleanAllButton()
        {
            //这个在选择之后清除所有按钮。可以改成隐藏，启用显示，但之后再说。
            for (int i = buttonGroup.childCount-1; i>=0 ; i--)
            {
                Destroy(buttonGroup.GetChild(i).gameObject);
            }

        }
        public void ShowText(DialogueNode nodes)
        {
            DOTween.To(() => string.Empty, value => DialogueText.text = value, nodes.Content, 1f).SetEase(Ease.Linear)
                    .OnStart(() => { Debug.Log("这里是进入回调"); nodes.TriggerEnter(); continuteButton.interactable = false; })
                    .OnComplete(() => {Debug.Log("这里是显示结束回调"); nodes.TriggerExit(); continuteButton.interactable = true;   IC.TriggerEvent("UpdateUICallback"); });//逐字显示
        }

        private void OnDestroy()
        {
            IC.TriggerEvent("ControlInput", true);

           
            IC.StopListening("GeneraOptions", TriggerEvent_GenerateOptions);

            Cursor.lockState = CursorLockMode.Locked;

            IC.StopListening("ShowDialogueText", TriggerEvent_ShowDialogueText);
            IC.UnRegisterEvent("ShowDialogueText");
            IC.UnRegisterEvent("GeneraOptions");
        }
        public override void Show()
        {
            base.Show();
            HandleOptionsCallback();
            Cursor.lockState = CursorLockMode.None;
            IC.TriggerEvent("ControlInput", false);

        }

        public void HandleOptionsCallback()
        {
            if(optionsCallbackNode!=null)
            { optionsCallbackNode.TriggerExit(); optionsCallbackNode=null; }
        }
        public override void Hide()
        {
            base.Hide();
            Cursor.lockState = CursorLockMode.Locked;
            IC.TriggerEvent("ControlInput", true);
        }
        public override void Open()
        {
            base.Open();
         
            Cursor.lockState = CursorLockMode.None;
            IC.TriggerEvent("ControlInput", false);
        }
        public override void Close()
        {
            base.Close();
            Cursor.lockState = CursorLockMode.Locked;
            IC.TriggerEvent("ControlInput", true);
        }
    }
}
