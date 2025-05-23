
using UnityEngine;

namespace FPS
{
    public class ArmsAssaultRifle02Controller : ArmsWeapon
    {

       /* private Transform Scope01Mesh;
        private Transform Scope02Mesh;
        private Transform Scope03Mesh;
        private Transform Scope04Mesh;*/
        private Transform assault_rifle_02_iron_sights;


      /*  private SkinnedMeshRenderer scope01Skin;
        private SkinnedMeshRenderer scope02Skin;
        private SkinnedMeshRenderer scope03Skin;
        private SkinnedMeshRenderer scope04Skin;

        private SkinnedMeshRenderer silencer;//消音器

        private List<SkinnedMeshRenderer> scopeSkinGroup;
        private List<Transform> scopeMeshGroup;*/
        protected override void Awake()
        {
            base.Awake();
           /* weaponType = WeaponType.Arms;
            auto = true;
            silencerMode = false;*/
           // bwData = FindObjectOfType<PlayerController>().player_SO.AAR02Data;
            bwData = IC.GetData<Player_SO>().AAR02Data;

        }
        protected override void OnEnable()
        {
            base.OnEnable();
           /* InitListenEvent();
            resuableData.weaponType = WeaponType.Arms;
            resuableData.silencer = silencerMode;
            resuableData.aimMode = aimMode;
            resuableData.weaponId = weaponId;*/

            SwitchWeaponCommandData temp = new SwitchWeaponCommandData();
            temp.weaponId = weaponId;
            temp.Type = weaponType;
            IC.SendCommand<SwitchWeaponCommand>(temp);
            /*Debug.Log("步枪");
            IC.PlaySFX("TakeOutWeapon", player);*/
        }
      /*  protected override void Start()
        {
           
            //3.开始添加事件
           // InitListenEvent();
            //4.初始化：隐藏物体
            InitGameObjectState();
            //5.设置枪械的数据，并且发送给Model层
            base.Start();


        
        }*/
        protected override void InitFindGameObject()
        {
            base.InitFindGameObject();
            assault_rifle_02_iron_sights = GameTools.GetSingleComponentInChild<Transform>(gameObject, "assault_rifle_02_iron_sights");

            /*  scopeSkinGroup = new List<SkinnedMeshRenderer>();
              scopeMeshGroup = new List<Transform>();
              Scope01Mesh = GameTools.GetSingleComponentInChild<Transform>(gameObject, "Scope 1 Render Mesh");
              scopeMeshGroup.Add(Scope01Mesh);
              Scope02Mesh = GameTools.GetSingleComponentInChild<Transform>(gameObject, "Scope 2 Render Mesh");
              scopeMeshGroup.Add(Scope02Mesh);
              Scope03Mesh = GameTools.GetSingleComponentInChild<Transform>(gameObject, "Scope 3 Render Mesh");
              scopeMeshGroup.Add(Scope03Mesh);
              Scope04Mesh = GameTools.GetSingleComponentInChild<Transform>(gameObject, "Scope 4 Render Mesh");
              scopeMeshGroup.Add(Scope04Mesh);

              scope01Skin = GameTools.GetSingleComponentInChild<SkinnedMeshRenderer>(gameObject, "scope_01");
              scopeSkinGroup.Add(scope01Skin);
              scope02Skin = GameTools.GetSingleComponentInChild<SkinnedMeshRenderer>(gameObject, "scope_02");
              scopeSkinGroup.Add(scope02Skin);
              scope03Skin = GameTools.GetSingleComponentInChild<SkinnedMeshRenderer>(gameObject, "scope_03");
              scopeSkinGroup.Add(scope03Skin);
              scope04Skin = GameTools.GetSingleComponentInChild<SkinnedMeshRenderer>(gameObject, "scope_04");
              scopeSkinGroup.Add(scope04Skin);

              assault_rifle_02_iron_sights = GameTools.GetSingleComponentInChild<Transform>(gameObject, "assault_rifle_02_iron_sights");

              silencer = GameTools.GetSingleComponentInChild<SkinnedMeshRenderer>(gameObject, "silencer");

              bulletShootPoint = GameTools.GetSingleComponentInChild<Transform>(gameObject, "Bullet Spawn Point");
              casingPrefab = Resources.Load<GameObject>("Object/BulletCasing/Big_Casing_Prefab");
            */
        }
        /*   protected void InitGameObjectState()
           {
               for (int i = 0; i < scopeMeshGroup.Count; i++)
               {
                   scopeMeshGroup[i].gameObject.SetActive(false);
               }
           }
           protected void InitListenEvent()
           {
               IC.StartListening("WeaponChange", TriggerEvent_ChangeWeaponScope);
               IC.StartListening("WeaponSilencer", TriggerEvent_WeaponSilencer);

           }

           #region TriggerEvent
           private void TriggerEvent_WeaponSilencer(object data)
           {
               bool value = (bool)data;
               silencer.enabled = value;
               resuableData.silencer = value;
               silencerMode = value;
           }*/
        #region TriggerEvent
        protected override void TriggerEvent_ChangeWeaponScope(object data)
        {
            int i = (int)data;

            switch (i)
            {
                case -1:
                    for (int j = 0; j < scopeMeshGroup.Count; j++)
                    {
                        scopeMeshGroup[j].gameObject.SetActive(false);
                        scopeSkinGroup[j].enabled = false;
                    }
                    resuableData.aimMode = 0;aimMode = 0;
                    assault_rifle_02_iron_sights.gameObject.SetActive(true);
                    break;
                case 0:
                    SetMeshTrue(0);
                    break;
                case 1:
                    SetMeshTrue(1);
                    break;
                case 2:
                    SetMeshTrue(2);
                    break;
                case 3:
                    SetMeshTrue(3);
                    break;
            }
        }
        protected override void SetMeshTrue(int i)
        {
            scopeMeshGroup[i].gameObject.SetActive(true);
            scopeSkinGroup[i].enabled = true;
            aimMode = i+1;
            resuableData.aimMode = i + 1;
            assault_rifle_02_iron_sights.gameObject.SetActive(false);
        }

        #endregion
        /*private void OnDisable()
        {
            IC.StopListening("WeaponChange", TriggerEvent_ChangeWeaponScope);
            IC.StopListening("WeaponSilencer", TriggerEvent_WeaponSilencer);
        }
        private void OnDestroy()
        {
            IC.StopListening("WeaponChange", TriggerEvent_ChangeWeaponScope);
            IC.StopListening("WeaponSilencer", TriggerEvent_WeaponSilencer);
        }*/
    }
}