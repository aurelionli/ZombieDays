using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace FPS 
{ 
    public class GuidePanel : BaseUIPanel
    {
        public Camera mainCamera; // 主摄像机
        public Transform target; // 目标物体
        public RectTransform icon; // UI图标
        public float radius = 150f; // 圆圈半径

        protected override void Awake()
        {
            Cursor.lockState = CursorLockMode.None;//鼠标解锁并显示
            Type = PanelType.AutoClosePopupPanel;

            Level = PanelLevel.One;
            base.Awake();

           
            IC.RegisterEvent("GuideTarget");
            IC.StartListening("GuideTarget", TriggerEvent_SetGuideTarget);
            IC.RegisterEvent("CloseGuidePanel");
            IC.StartListening("CloseGuidePanel", (obj)=>Close());
        }
        private void OnDestroy()
        {
            IC.StopListening("GuideTarget", TriggerEvent_SetGuideTarget);
            IC.StopListening("CloseGuidePanel", (obj) => Close());
            IC.UnRegisterEvent("CloseGuidePanel");
            IC.UnRegisterEvent("GuideTarget");
        }
       
        private void TriggerEvent_SetGuideTarget(object obj)
        {
            target = obj as Transform;
            icon.gameObject.SetActive(true);
        }

        private void Start()
        {
            icon = GameTools.GetSingleComponentInChild<RectTransform>(gameObject, "icon");
            icon.gameObject.SetActive(false);
            mainCamera = Camera.main;
        }
        private void Update()
        {
            if (target == null || mainCamera == null || icon == null) return;

            // 计算物体相对于相机的方向
            Vector3 directionToTarget = target.position - mainCamera.transform.position;

            directionToTarget.y = 0f;
            Vector3 cameraForward = mainCamera.transform.forward;
            cameraForward.y = 0f;

            // 计算物体方向与相机前方的夹角
            float angle = Vector3.SignedAngle(cameraForward, directionToTarget, Vector3.up);

            // 将夹角映射到UI圆圈上// 注意: 在Unity中，屏幕坐标系的Y轴是向下的，因此需要对cos值取反
            // Vector2 screenCenter = new Vector2(Screen.width / 2, Screen.height / 2);
            //极坐标（基于角度和半径的距离）转换为笛卡尔坐标（x, y位置）
            float radians = angle * Mathf.Deg2Rad; // 角度转弧度
            //x = r*cos(弧度)  y=r*sin(弧度)
            Vector2 iconPosition =   new Vector2(Mathf.Sin(radians), Mathf.Cos(radians)) * radius;

            // 更新UI图标的位置
            icon.anchoredPosition = iconPosition;
        }
    }
}

