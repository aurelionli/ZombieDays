
using DG.Tweening;
using FPS_Manager;
using TMPro;
using UnityEngine;

using UnityEngine.UI;

namespace FPS
{
    public class GamePanel :BaseUIPanel
    {
        #region 关于武器配件的参数
        private Transform weaponChangeGroup;

        private Toggle weaponScope01;
        private Toggle weaponScope02;
        private Toggle weaponScope03;
        private Toggle weaponScope04;
        private Toggle weaponSilencer;
        #endregion
        #region 关于玩家状态的参数
        private TMP_Text healthPoint; //当前生命值
        private Image ammoImage;   //子弹类型图
        private TMP_Text ammoText; //子弹数量
        private TMP_Text throwText;//手雷数量
        private Image gunImage; //根据当前武器换图
        private Image energyPoint;//当前体力值
        #endregion
        #region 准心
        private RectTransform crosshair;
        private Image hitImage;
        private Image crosshairImage;
        
        private float defaultSize = 50f;        // 默认准心大小
        private float shootSpreadSize = 100f;   // 射击时最大扩散
        private float spreadDuration = 0.1f;    // 扩散速度（秒）
        private float recoverDuration = 0.3f;   // 恢复速度（秒）
        private Color hitColor = Color.red;     // 命中反馈颜色
        private Color normalColor = Color.white;// 默认颜色
        private float hitFlashDuration = 0.2f;  // 颜色反馈时长

        private Vector2 originalSize;
        private Tween sizeTween;
        private Tween colorTween;
        private Tween fadeTween;

        private bool IsMoving;
        #endregion
        #region 关于手雷，枪械贴图
        private UI_SO uiData;
        #endregion
        private CurrentWeaponModel currentWeaponModel;


        
        //在Update中添加移动扩散
       /* public void MoveSpread()
        {
           
      /*      if (IsMoving)
            {
                float moveSpread = Mathf.Lerp(
                    crosshair.sizeDelta.x,
                    defaultSize * movementSpreadFactor,
                    Time.deltaTime * 10f
                );
                crosshair.sizeDelta = new Vector2(moveSpread, moveSpread);
            }
        }

       
        private void OnHit(RaycastHit hit)
        {
            // 命中反馈
            if (hit.transform.CompareTag("Blood"))
            {
                colorTween?.Kill();
                colorTween = crosshairImage.DOColor(hitColor, hitFlashDuration / 2)
                    .OnComplete(() =>
                    {
                        crosshairImage.DOColor(normalColor, hitFlashDuration / 2);
                    });
            }
        }
        */
        private void OnAimStateChanged(bool isAiming)
        {
            // 瞄准时隐藏准心
            crosshair.gameObject.SetActive(!isAiming);
        }

        private bool isOpenChangeWeaponImage;
        protected override void Awake()
        {
            Level = PanelLevel.One;
            Type = PanelType.NormalPanel;
            base.Awake();
            uiData = IC.GetData<UI_SO>();
            currentWeaponModel = IC.GetModel<CurrentWeaponModel>();
            InitFindGameObject();
            RegisterEvent();
           
        }
        private void Start()
        {
            ListenEvent();

            InitGameObjectListenEvent();
            InitGameObjectState();
        }
        private void Update()
        {
            if (IsMoving)
            {
                float moveSpread = Mathf.Lerp( crosshair.sizeDelta.x,defaultSize * 1.5f, Time.deltaTime * 10f);
                crosshair.sizeDelta = new Vector2(moveSpread, moveSpread);  
            }
        }
        /// <summary>
        /// 寻找自身物体，放在Awake
        /// </summary>
        private void InitFindGameObject()
        {
            weaponChangeGroup = GameTools.GetSingleComponentInChild<Transform>(gameObject, "WeaponChangeGroup");

            weaponScope01 = GameTools.GetSingleComponentInChild<Toggle>(gameObject, "Scope01BG");
            weaponScope02 = GameTools.GetSingleComponentInChild<Toggle>(gameObject, "Scope02BG");
            weaponScope03 = GameTools.GetSingleComponentInChild<Toggle>(gameObject, "Scope03BG");
            weaponScope04 = GameTools.GetSingleComponentInChild<Toggle>(gameObject, "Scope04BG");
            weaponSilencer = GameTools.GetSingleComponentInChild<Toggle>(gameObject, "SilencerBG");

            healthPoint = GameTools.GetSingleComponentInChild<TMP_Text>(gameObject, "HealthPoint");
            ammoImage = GameTools.GetSingleComponentInChild<Image>(gameObject, "AmmoImage");
            ammoText = GameTools.GetSingleComponentInChild<TMP_Text>(gameObject, "AmmoText");
            throwText = GameTools.GetSingleComponentInChild<TMP_Text>(gameObject, "ThrowText");
            gunImage = GameTools.GetSingleComponentInChild<Image>(gameObject, "GunImage");

            crosshair = GameTools.GetSingleComponentInChild<RectTransform>(gameObject, "Crosshair");
           
            crosshairImage = GameTools.GetSingleComponentInChild<Image>(gameObject, "Crosshair");
            hitImage = GameTools.GetSingleComponentInChild<Image>(gameObject, "Hit");

            energyPoint = GameTools.GetSingleComponentInChild<Image>(gameObject, "EnergyPoint");
        }
      
        private void InitGameObjectState()
        {
            originalSize = crosshair.sizeDelta;

            throwText.text = "0";
            weaponChangeGroup.gameObject.SetActive(false);
            isOpenChangeWeaponImage = false;
        }
        
        /// <summary>
        /// 注册事件，放在Awake
        /// </summary>
        private void RegisterEvent()
        {
            //注册事件：开启改装界面，改瞄准镜事件，改消音事件。
            IC.RegisterEvent("OpenChangWeaponPanel");
            IC.RegisterEvent("WeaponChange");
            IC.RegisterEvent("WeaponSilencer");
            //射击扩散事件
            IC.RegisterEvent("WeaponShoot");
            //换弹或者子弹减少
            IC.RegisterEvent("WeaponAmmo");
            //瞄准准心隐藏事件
            IC.RegisterEvent("WeaponCrossHairHideShow");
            //扔雷事件
            IC.RegisterEvent("ThrowGe");
            //击中敌人事件
            IC.RegisterEvent("HitEnemy");
            //IC.RegisterEvent("IsMoving");
            IC.RegisterEvent("PlayerHealthChange");
            //体力改变
            IC.RegisterEvent("PlayerEnergyChange");
        }
        private void ListenEvent()
        {
            //监听事件：打开改装界面
            IC.StartListening("OpenChangWeaponPanel", TriggerEvent_WeaponChange);
            IC.StartListening("WeaponShoot", TriggerEvent_WeaponShootEvent);
            IC.StartListening("WeaponAmmo", TriggerEvent_WeaponAmmo);
            IC.StartListening("WeaponCrossHairHideShow", TriggerEvent_SetWeaponSpread);
            Debug.Log("2.----开始监听换武器事件");
            IC.StartListening("SwitchWeapon", TriggetEvent_SwitchWeapon);
            IC.StartListening("HitEnemy", TriggerEvent_HitEnemy);
            IC.StartListening("ThrowGe", TriggerEvent_Throw);
   
            IC.StartListening("PlayerHealthChange", TriggerEvent_HealthPointChange);
            IC.StartListening("PlayerEnergyChange", TriggerEvent_EnergyPointChage);
        }
        /// <summary>
        /// 绑定事件，放在Start
        /// </summary>
        private void InitGameObjectListenEvent()
        {
            weaponScope01.onValueChanged.AddListener((value) => { if (value) { IC.TriggerEvent("WeaponChange", 0); return; } IC.TriggerEvent("WeaponChange", -1); });
            weaponScope02.onValueChanged.AddListener((value) => { if (value) { IC.TriggerEvent("WeaponChange", 1); return; } IC.TriggerEvent("WeaponChange", -1); });
            weaponScope03.onValueChanged.AddListener((value) => { if (value) { IC.TriggerEvent("WeaponChange", 2); return; } IC.TriggerEvent("WeaponChange", -1); });
            weaponScope04.onValueChanged.AddListener((value) => { if (value) { IC.TriggerEvent("WeaponChange", 3); return; } IC.TriggerEvent("WeaponChange", -1); });

            weaponSilencer.onValueChanged.AddListener((value) => { if (value) { IC.TriggerEvent("WeaponSilencer", true); return; } IC.TriggerEvent("WeaponSilencer", false); });



            
            
         //   IC.StartListening("IsMoving", TriggerEvent_IsMoveing);
        }
        /// <summary>
        /// 初始化物体的状态，放在Start
        /// </summary>
        private void OnDestroy()
        {
            IC.StopListening("OpenChangWeaponPanel", TriggerEvent_WeaponChange);
            IC.StopListening("WeaponShoot", TriggerEvent_WeaponShootEvent);
            IC.StopListening("WeaponAmmo", TriggerEvent_WeaponAmmo);
            IC.StopListening("WeaponCrossHairHideShow", TriggerEvent_SetWeaponSpread);
            IC.StopListening("SwitchWeapon", TriggetEvent_SwitchWeapon);
            //IC.StopListening("IsMoving", TriggerEvent_IsMoveing);
            IC.StopListening("HitEnemy", TriggerEvent_HitEnemy);
            IC.StopListening("ThrowGe", TriggerEvent_Throw);
            IC.StopListening("PlayerHealthChange", TriggerEvent_HealthPointChange);
            IC.StopListening("PlayerEnergyChange", TriggerEvent_EnergyPointChage);

            IC.UnRegisterEvent("OpenChangWeaponPanel");
            IC.UnRegisterEvent("WeaponChange");
            IC.UnRegisterEvent("WeaponSilencer");

            IC.UnRegisterEvent("WeaponShoot");
            IC.UnRegisterEvent("WeaponAmmo");
            IC.UnRegisterEvent("WeaponCrossHairHideShow");
            IC.UnRegisterEvent("ThrowGe");

            IC.UnRegisterEvent("HitEnemy");
            IC.UnRegisterEvent("PlayerHealthChange");
            IC.UnRegisterEvent("PlayerEnergyChange");
            // IC.UnRegisterEvent("IsMoving");
        }
        #region ToggleOnClicked
        //以此为例子，但是我使用的lambada表达式
        private void OnClicked_Scope_01Toggle(bool value)
        {
            if (value) { IC.TriggerEvent("WeaponChange", 0); }
            else
            {
                IC.TriggerEvent("WeaponChange", -1);
            }
        }

        #endregion
        #region TriggerEvent

        /// <summary>
        /// 是否打开改装界面的事件
        /// </summary>
        /// <param name="data"></param>
        private void TriggerEvent_WeaponChange(object data)
        {
            isOpenChangeWeaponImage = !isOpenChangeWeaponImage;
            

            Cursor.lockState = CursorLockMode.None;//鼠标解锁并显示
            if(isOpenChangeWeaponImage)
            {
                Cursor.lockState = CursorLockMode.None;//鼠标解锁并显示
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;//鼠标锁定并隐藏
            }
            //todo:这里根据枪械类型，从而增减按钮
            //1.手枪只有23消音
            //2.步枪全有，rpg全有
            //3.狙击全无
            //4.喷子无消音
            //5.冲锋枪全由


            weaponChangeGroup.gameObject.SetActive(isOpenChangeWeaponImage);



        }
        /// <summary>
        /// 更新子弹事件
        /// </summary>
        /// <param name="data"></param>
        private void TriggerEvent_WeaponAmmo(object data)
        {

            //这里根据传过来的Data，对Ui进行一定的修改。
            //这里需要的是，弹匣子弹/备弹
            ammoText.text = currentWeaponModel.weaponBulletMagDic[currentWeaponModel.currentWeaponId].ToString()
               + "/" + currentWeaponModel.weaponBulletNumDic[currentWeaponModel.currentWeapon].ToString();
        }
        /// <summary>
        /// 更换武器事件
        /// </summary>
        /// <param name="data"></param>
        private void TriggetEvent_SwitchWeapon(object data)
        {
            //1.更改枪械图片
            foreach (var item in uiData.w)
            {
                if(item.weaponID== currentWeaponModel.currentWeaponId)
                {
                    gunImage.sprite = item.weaponImage;
                    ammoImage.sprite = item.casingBulletImage;
                }
            }

            //2.更改当前子弹
            Debug.Log("SwitchWeapon触发！，当前武器类型为" + currentWeaponModel.currentWeapon);
            //3.更改当前子弹数量
            ammoText.text = currentWeaponModel.weaponBulletMagDic[currentWeaponModel.currentWeaponId].ToString()
             + "/" + currentWeaponModel.weaponBulletNumDic[currentWeaponModel.currentWeapon].ToString();
            //4.更改改装界面
            ChangeWeaponChangeSelect(currentWeaponModel.currentWeapon);


        }
        private void ChangeWeaponChangeSelect(WeaponType type)
        {
            if (type == WeaponType.Arms|| type == WeaponType.Smg)
            {
                //全启动
                weaponScope01.gameObject.SetActive(true);
                weaponScope02.gameObject.SetActive(true);
                weaponScope03.gameObject.SetActive(true);
                weaponScope04.gameObject.SetActive(true);
                weaponSilencer.gameObject.SetActive(true);
            }
            if(type == WeaponType.Hand)
            {
                //关 1  4
                weaponScope01.gameObject.SetActive(false);
                weaponScope02.gameObject.SetActive(true);
                weaponScope03.gameObject.SetActive(true);
                weaponScope04.gameObject.SetActive(false);
                weaponSilencer.gameObject.SetActive(true);
            }
            if(type == WeaponType.Sniper)
            {
                //全关
                weaponScope01.gameObject.SetActive(false);
                weaponScope02.gameObject.SetActive(false);
                weaponScope03.gameObject.SetActive(false);
                weaponScope04.gameObject.SetActive(false);
                weaponSilencer.gameObject.SetActive(false);
            }
            if(type==WeaponType.Shot||type==WeaponType.Rpg)
            {
                //关消音
                weaponScope01.gameObject.SetActive(true);
                weaponScope02.gameObject.SetActive(true);
                weaponScope03.gameObject.SetActive(true);
                weaponScope04.gameObject.SetActive(true);
                weaponSilencer.gameObject.SetActive(false);
            }
        }
         
        /// <summary>
        /// 击中敌人事件
        /// </summary>
        /// <param name="data"></param>
        private void TriggerEvent_HitEnemy(object data)
        {
            if (crosshair.gameObject.activeSelf) 
            {
                colorTween?.Kill();
                colorTween = crosshair.GetComponent<Image>().DOColor(hitColor, hitFlashDuration).
            OnComplete(() =>
            {
                // 恢复动画
                crosshair.GetComponent<Image>().DOColor(normalColor, hitFlashDuration);
            });
            }
            //取消之前的动画

            fadeTween?.Kill();

            fadeTween = hitImage.DOFade(1f, hitFlashDuration).OnComplete(() => hitImage.DOFade(0f, hitFlashDuration));

          
        }
        /// <summary>
        /// 扔手雷事件
        /// </summary>
        /// <param name="data"></param>
        private void TriggerEvent_Throw(object data)
        {
            //更改手雷的数量
            throwText.text = currentWeaponModel.throwNum.ToString();
        }
        /// <summary>
        /// 血量变更事件
        /// </summary>
        /// <param name="data"></param>
        private void TriggerEvent_HealthPointChange(object data)
        {
            healthPoint.text = "HP:"+IC.GetModel<PlayerCurrentStateModel>().currentHealth.ToString();
        }
        /// <summary>
        /// 武器射击，准心扩散
        /// </summary>
        /// <param name="data"></param>
        private void TriggerEvent_WeaponShootEvent(object data)
        {
            if (!crosshair.gameObject.activeSelf) { return; }
            //取消之前的动画

            sizeTween?.Kill();

            //射击扩散动画
            sizeTween = crosshair.DOSizeDelta(
            new Vector2(shootSpreadSize, shootSpreadSize),
            spreadDuration
        ).OnComplete(() =>
        {
            // 恢复动画
            crosshair.DOSizeDelta(originalSize, recoverDuration);
        });
        }
        /// <summary>
        /// 体力出现变化
        /// </summary>
        public void TriggerEvent_EnergyPointChage(object data)
        {
           // Debug.Log(IC.GetModel<PlayerCurrentStateModel>().currentEnergy+"  "+ IC.GetModel<PlayerCurrentStateModel>().playerEnergy);
            energyPoint.fillAmount = IC.GetModel<PlayerCurrentStateModel>().currentEnergy / IC.GetModel<PlayerCurrentStateModel>().playerEnergy;
        }
        /// <summary>
        ///  瞄准时隐藏  
        /// </summary>
        /// <param name="spreadMultiplier"></param>
        public void TriggerEvent_SetWeaponSpread(object data)
        {
            // shootSpreadSize = defaultSize * spreadMultiplier;
            bool value = (bool)data;
            crosshair.gameObject.SetActive(value);
        }

      /*  private void TriggerEvent_IsMoveing(object data)
        {
            float e= (float)data;
            if (e == 0) { IsMoving =false; }
            else
            { IsMoving = true; }
        }*/
        #endregion



    }
}
