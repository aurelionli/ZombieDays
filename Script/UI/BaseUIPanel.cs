using DG.Tweening;
using UnityEngine;

namespace FPS
{
    public enum PanelLevel { One, Two, Three }  //面板等级枚举
    public enum PanelType { NormalPanel, InteractPopupPanel, AutoClosePopupPanel }

    public class BaseUIPanel : MonoBehaviour,IController
    {
        protected IController IC;
        public string message;

        public CanvasGroup canvasGroup;
        public PanelLevel Level { get; set; }
        public PanelType Type { get; set; }
        //初始化东西

        private RectTransform t;
        private Vector3 pos;
        protected virtual void Awake()
        {
            IC = this;
            t = GetComponent<RectTransform>();
            pos = t.anchoredPosition3D;
            canvasGroup = GetComponent<CanvasGroup>();

        }
        public virtual void Open()
        {
            gameObject.SetActive(true);
            canvasGroup.alpha = 1f;
            transform.DOMoveY(500, 0.25f).From(false);
        }
        //释放东西，摧毁自己
        public virtual void Close()
        {
            transform.DOMoveY(500, 0.25f).OnComplete(
                () =>
                {
                    canvasGroup.alpha = 0f;
                    t.anchoredPosition3D = pos;
                    gameObject.SetActive(false);
                    Destroy(gameObject);
                });
        }
        //显示面板，并重新设置一些东西
        public virtual void Show()
        {
            gameObject.SetActive(true);
            canvasGroup.alpha = 1f;
            canvasGroup.interactable = true;
            transform.DOMoveY(500, 0.25f).From(false);
        }
        //隐藏面板
        public virtual void Hide()
        {
            transform.DOMoveY(500, 0.25f).OnComplete(
                () =>
                {
                    canvasGroup.alpha = 0f;
                    t.anchoredPosition3D = pos;
                    gameObject.SetActive(false);
                });

        }



    }

}