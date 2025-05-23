using DG.Tweening;
using UnityEngine;

namespace FPS
{
    public enum PanelLevel { One, Two, Three }  //���ȼ�ö��
    public enum PanelType { NormalPanel, InteractPopupPanel, AutoClosePopupPanel }

    public class BaseUIPanel : MonoBehaviour,IController
    {
        protected IController IC;
        public string message;

        public CanvasGroup canvasGroup;
        public PanelLevel Level { get; set; }
        public PanelType Type { get; set; }
        //��ʼ������

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
        //�ͷŶ������ݻ��Լ�
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
        //��ʾ��壬����������һЩ����
        public virtual void Show()
        {
            gameObject.SetActive(true);
            canvasGroup.alpha = 1f;
            canvasGroup.interactable = true;
            transform.DOMoveY(500, 0.25f).From(false);
        }
        //�������
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