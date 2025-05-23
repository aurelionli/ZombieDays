using DG.Tweening;
using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace FPS
{
    public class TaskPanel :BaseUIPanel
    {
        
        protected override void Awake()
        {
            Type = PanelType.InteractPopupPanel;
            Level = PanelLevel.One;
            base.Awake();
        }
        private void Start()
        {
            
            GameTools.GetSingleComponentInChild<Button>(gameObject, "CloseButton").onClick.AddListener(()=> { IC.TriggerEvent("GuideTarget", FindObjectOfType<LapTopController>().transform); IC.ClosePanel(); });
        }
        
        public override void Close()
        {
            base.Close();
            IC.TriggerEvent("ControlInput", true);
            Cursor.lockState = CursorLockMode.Locked;//鼠标解锁并显示
        }
        public override void Open()
        {
            IC.TriggerEvent("ControlInput", false);
            Cursor.lockState = CursorLockMode.None;//鼠标解锁并显示
            base.Open();
        }

    }
}
