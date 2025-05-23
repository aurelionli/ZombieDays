
using DG.Tweening;

using System.Linq;
using UnityEngine;
using TMPro;
using System.Collections;

using UnityEngine.UI;
using Unity.VisualScripting;


namespace FPS
{
    public class DeadPanel :BaseUIPanel
    {
        private CanvasGroup BackGround;
        public Button startButton;

        public Button returnButton;

        private RectTransform BlackOccluderDown;

        private RectTransform BlackOccluderUp;

        protected override void Awake()
        {

            Level = PanelLevel.One;
            Type = PanelType.NormalPanel;
            base.Awake();
            InitFindGameObject();

            
        }

        public override void Open()
        {
            gameObject.SetActive(true);
            DG.Tweening.Sequence loopTween = DOTween.Sequence();
            //1.第一步，两块黑屏合并
            loopTween.Append(BlackOccluderDown.DOLocalMoveY(-270f, 1f));
            loopTween.Join(BlackOccluderUp.DOLocalMoveY(270f, 1f));
            //2.淡出黑屏
            loopTween.Append(BackGround.DOFade(1f, 0.5f));
            //3.打开
            loopTween.Append(BlackOccluderDown.DOLocalMoveY(-810f, 1f));
            loopTween.Join(BlackOccluderUp.DOLocalMoveY(810f, 1f));
            loopTween.Play().OnComplete(() => { loopTween.Kill(); loopTween = null; });
            IC.TriggerEvent("ControlInput", false);
        }
        public override void Close() { base.Close(); IC.TriggerEvent("ControlInput", true); }

        private void InitFindGameObject()
        {
            startButton = GameTools.GetSingleComponentInChild<Button>(gameObject, "RestartGameButton");
            returnButton = GameTools.GetSingleComponentInChild<Button>(gameObject, "ReturnMainMenuButton");
            returnButton.AddComponent<ButtonSoundController>();
            startButton.AddComponent<ButtonSoundController>();
            BlackOccluderDown = GameTools.GetSingleComponentInChild<RectTransform>(gameObject, "BlackOccluderDown");
            BlackOccluderUp = GameTools.GetSingleComponentInChild<RectTransform>(gameObject, "BlackOccluderUp");
            BackGround = GameTools.GetSingleComponentInChild<CanvasGroup>(gameObject, "BackGround");
        }
        private void ListenEvent()
        {
            startButton.onClick.AddListener(OnClicked_StartGame);
            returnButton.onClick.AddListener(OnClicked_GoRange);

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

            IC.LoadScene(0);
        }


    }
}
