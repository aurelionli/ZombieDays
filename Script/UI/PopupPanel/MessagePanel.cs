using DG.Tweening;
using System;
using System.Collections;
using TMPro;
using UnityEngine;

namespace FPS
{
    public class MessagePanel :BaseUIPanel
    {
        private RectTransform RectTransform;
        private TMP_Text messageText;

        protected override void Awake()
        {
            
            Level = PanelLevel.One;
            Type = PanelType.AutoClosePopupPanel;
            base.Awake();
            InitFindGameObject();

            StartCoroutine(asdasdas());
        }

        private void InitFindGameObject()
        {
            RectTransform = GameTools.GetSingleComponentInChild<RectTransform>(gameObject, "MessagePanelBG");
            messageText = GameTools.GetSingleComponentInChild<TMP_Text>(gameObject, "MessageText");
        }

        IEnumerator asdasdas()
        {
            Open();
            yield return new WaitForSeconds(5f);
            Close();
        }
        public override void Close()
        {
            RectTransform.DOSizeDelta(new Vector2(2000f, 0f), 0.2f).OnComplete(() => Destroy(gameObject));
        }
        public override void Open()
        {
            messageText.text = message;
            RectTransform.DOSizeDelta(new Vector2(2000f, 125f), 0.2f);
        }

       
    }
}
