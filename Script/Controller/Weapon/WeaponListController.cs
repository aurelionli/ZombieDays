
using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.InputSystem;

namespace FPS
{
    public class WeaponListController :MonoBehaviour,IController
    {
        //有三把武器， 1.必须手枪  2.随意(可空)   3.随意*(可空)
        //有切换功能，可以捡到武器，可以目前手上的武器进行切换，如果不够三把就直接添加
        //可以切换武器
        public IController IC;


        public PlayerControls.WeaponActions weaponActions;

        public BaseWeapon[] weaponList = new BaseWeapon[3];

        public BaseWeapon currentWeapon;

        public int currentIndex;

        private PlayerReuseableData reuseableData;

        private Dictionary<int,string> weaponPath = new Dictionary<int,string>();
        private Dictionary<int,GameObject> weaponDir = new Dictionary<int,GameObject>();

       


        private void Awake()
        {
            IC = this;
            weaponActions = IC.GetWeaponControls();

          //  InitializeWeapons();
            InitWeaponPath();
            InitEvent();
            InitObjectPool();
           
        }
        /// <summary>
        /// 初始化对象池 //子弹，雷，弹壳都在这里注册
        /// </summary>
        private void InitObjectPool()
        {
            //注册子弹
            IC.RegisterObjectPool("Bullet", "Object/BulletCasing/Bullet_Prefab");
            IC.RegisterObjectPool("GrenadeLauncher", "Object/BulletCasing/Grenade_Launcher_01_Projectile");
            //注册三个弹壳
            IC.RegisterObjectPool("BigCasing", "Object/BulletCasing/Big_Casing_Prefab");
            IC.RegisterObjectPool("SmallCasing", "Object/BulletCasing/Small_Casing_Prefab");
            IC.RegisterObjectPool("ShotCasing", "Object/BulletCasing/Shotgun_Shell_Prefab");
            //注册手雷
            IC.RegisterObjectPool("HandGrenade", "Object/BulletCasing/Hand_Grenade_Prefab");
            //注册特效,血，混凝土，泥土，钢铁，炸
            IC.RegisterObjectPool("Blood", "Object/Effect/Blood Impact Prefab");
            IC.RegisterObjectPool("Concrete", "Object/Effect/Concrete Impact Prefab");
            IC.RegisterObjectPool("Dirt", "Object/Effect/Dirt Impact Prefab");
            IC.RegisterObjectPool("Metal", "Object/Effect/Metal Impact Prefab");
            IC.RegisterObjectPool("Explosion", "Object/Effect/Explosion Prefab");
        }

        /// <summary>
        /// 初始武器路径，在Awake
        /// </summary>
        private void InitWeaponPath()
        {
            weaponPath.Add(1001, "Object/Weapon/arms_handgun_01");
            weaponPath.Add(1002, "Object/Weapon/arms_handgun_02");
            weaponPath.Add(1003, "Object/Weapon/arms_handgun_03");
            weaponPath.Add(1004, "Object/Weapon/arms_handgun_04");

            weaponPath.Add(2001, "Object/Weapon/arms_assault_rifle_01");
            weaponPath.Add(2002, "Object/Weapon/arms_assault_rifle_02");
            weaponPath.Add(2003, "Object/Weapon/arms_assault_rifle_03");

            weaponPath.Add(4001, "Object/Weapon/arms_shotgun_01");

            weaponPath.Add(3001, "Object/Weapon/arms_smg_01");
            weaponPath.Add(3002, "Object/Weapon/arms_smg_02");
            weaponPath.Add(3003, "Object/Weapon/arms_smg_03");
            weaponPath.Add(3004, "Object/Weapon/arms_smg_04");
            weaponPath.Add(3005, "Object/Weapon/arms_smg_05");

            weaponPath.Add(5001, "Object/Weapon/arms_sniper_01");
            weaponPath.Add(5002, "Object/Weapon/arms_sniper_02");
            weaponPath.Add(5003, "Object/Weapon/arms_sniper_03");

            weaponPath.Add(6001, "Object/Weapon/arms_grenade_launcher_01");

        }
       

       /* private void InitializeWeapons()
        {
            //todo:找到枪，然后存入，
            //0是主相机
           for(int i = 1; i < transform.childCount; i++)
            {
                weaponList[i-1]= transform.GetChild(i).GetComponent<BaseWeapon>();
                weaponList[i - 1].gameObject.SetActive(false);
            }
        }*/

        private void Start()
        {
            StartListenEvent();
            reuseableData = FindObjectOfType<PlayerController>().reuseableData;
            Debug.Log("3.----生成武器");
            PickUpWeapon(1002);
            Debug.Log("4.----执行切换武器");
            SwitchWeapon(0);
            currentIndex = 0;
            
            weaponList[currentIndex].gameObject.SetActive(true);
            currentWeapon = weaponList[currentIndex];
           /* if (weaponList[0]==null)
            {
                Debug.LogError("缺少手枪");
                return;
            }
            currentWeapon = weaponList[0];
            Transform temp = GameTools.GetSingleComponentInChild<Transform>(currentWeapon.gameObject, "Gun Camera");
            IC.TriggerEvent("SwitchGunCamera", temp);
            currentWeapon.gameObject.SetActive(true);
            currentIndex = 0;*/




        }
        private void InitEvent() //awake
        {
            IC.RegisterEvent("PickUpWeapon");
            IC.RegisterEvent("SwitchWeapon");
        }
        private void StartListenEvent()
        {
            IC.StartListening("PickUpWeapon", TriggerEvent_PickUpWeapon);
        }
        private void OnDestroy()
        {
            IC.StopListening("PickUpWeapon", TriggerEvent_PickUpWeapon);
    
            IC.UnRegisterEvent("PickUpWeapon");
            IC.UnRegisterEvent("SwitchWeapon");
        }
        private float time;
        private void Update()
        {
            

            CheckWeaponSwitch();
           /* if(Time.time>time+1f)
            {
                Debug.Log("currentIndex" + currentIndex+"交互"+ reuseableData.canSwitchWeapon+"当前武器"+currentWeapon.gameObject.name);
                time = Time.time;
            }*/
        }

        private void TriggerEvent_PickUpWeapon(object data)
        {
            int Id = (int)data;
            PickUpWeapon(Id);
            //生成武器

        }
        private void CheckWeaponSwitch()
        {
            if (!reuseableData.canSwitchWeapon) { return; }
           
            if (weaponActions.UseOne.triggered&& currentIndex != 0 && weaponList[0]!=null)
            {
                currentIndex = 0;
               // StartCoroutine(SwitchWeaponIE(0));
                //如果当前武器不为空
                SwitchWeapon(0);
                
            }
            if (weaponActions.UseTwo.triggered && currentIndex != 1 && weaponList[1] != null)
            {
                currentIndex = 1;
               // StartCoroutine(SwitchWeaponIE(1));
                SwitchWeapon(1);
                
            }
            if (weaponActions.UseThree.triggered && currentIndex != 2 && weaponList[2] != null)
            {
                currentIndex = 2;
               // StartCoroutine(SwitchWeaponIE(2));
                SwitchWeapon(2);
               
            }
        }
        private IEnumerator SwitchWeaponIE(int index)
        {
            if (index >= 0 && index < weaponList.Length && weaponList[index] != null)
            {
                if (currentWeapon != null)
                {
                    currentWeapon.gameObject.SetActive(false);
                }
                currentWeapon = weaponList[index];
                yield return SetActive();
                Transform temp = GameTools.GetSingleComponentInChild<Transform>(currentWeapon.gameObject, "Gun Camera");
                IC.TriggerEvent("SwitchGunCamera", temp);
                Debug.Log("换枪调用");
                SwitchWeaponCommandData tempdata = new SwitchWeaponCommandData();
                tempdata.weaponId = currentWeapon.weaponId;
                tempdata.Type = currentWeapon.weaponType;
                IC.SendCommand<SwitchWeaponCommand>(tempdata);

            }
        }
        public IEnumerator SetActive()
        {
            currentWeapon.gameObject.SetActive(true);
            yield return null;
        }
        private void SwitchWeapon(int index)
        {
            if (index >= 0 && index < weaponList.Length && weaponList[index] != null)
            {
                if (currentWeapon != null)
                {
                    currentWeapon.gameObject.SetActive(false);
                }
                currentWeapon = weaponList[index];
                currentWeapon.gameObject.SetActive(true);
                Transform temp = GameTools.GetSingleComponentInChild<Transform>(currentWeapon.gameObject, "Gun Camera");
                IC.TriggerEvent("SwitchGunCamera", temp);
                Debug.Log("换枪调用");
                SwitchWeaponCommandData tempdata = new SwitchWeaponCommandData();
                tempdata.weaponId = currentWeapon.weaponId;
                tempdata.Type = currentWeapon.weaponType;
                IC.SendCommand<SwitchWeaponCommand>(tempdata);
             
            }

           /* if (weaponList[i] != null)
            {
                currentWeapon.gameObject.SetActive(false);
                currentWeapon = weaponList[i];
                currentWeapon.gameObject.SetActive(true);
                IC.TriggerEvent("SwitchGunCamera", GameTools.GetSingleComponentInChild<Transform>(currentWeapon.gameObject, "Gun Camera"));
            }*/

        }

       
        private void PickUpWeapon(int id)
        {
            //检查空位前,查看是否是重复拿，重复拿就补充子弹
            for (int i = 0; i < weaponList.Length; i++)
            {
                if (weaponList[i]!=null&&weaponList[i].weaponId == id)
                {
                    Debug.Log("重复拿相同武器，只会重置子弹");
                    IC.SendCommand<GetTheSameWeaponCommand>();
                    return;
                }
            }

            bool hasEmptySlot = false;
            int emptySlotIndex = -1;
            //这里检查有没有空位。
            for(int i = 0; i < weaponList.Length; i++)
            {
                if (weaponList[i] == null)
                {
                    hasEmptySlot = true;
                    emptySlotIndex = i;
                    break;
                }
            }
           

            // 如果有空位，直接添加新武器
            if (hasEmptySlot)
            {
                // 如果有空位，直接添加新武器
                weaponList[emptySlotIndex] = GetWeapon(id);
                weaponList[emptySlotIndex].gameObject.SetActive(false);
               
                //这里去注册。
                RegisterWeaponCommandData temp = new RegisterWeaponCommandData();
                temp.weaponType = weaponList[emptySlotIndex].weaponType;
                temp.bulletMag = weaponList[emptySlotIndex].bwData.bulletMag;
                temp.weaponId = weaponList[emptySlotIndex].weaponId;
                Debug.Log($"有空位，注册武器{weaponList[emptySlotIndex].name},发送Command，参数为{temp.weaponType}和{temp.bulletMag}");
                IC.SendCommand<RegisterWeaponCommand>(temp);
            }
            else
            {
                // 如果没有空位，提示无法拾取更多武器或替换已有武器
                Debug.LogWarning("Cannot pick up more weapons. Inventory is full.");
                // 或者可以选择替换已有武器
                ReplaceExistingWeapon(id);
            }
           

        }
        private void ReplaceExistingWeapon(int id)
        {
            // 这里可以实现替换已有武器的逻辑
            // 例如，询问玩家是否要替换某一把武器
            // 这里仅作示例，实际实现可能需要UI交互
            Debug.LogWarning("Replacing an existing weapon. Implement UI to select which one.");
            
            //逻辑：不做限制
            //手枪id1004 到 1001
           /* if(currentIndex==0&&id<=1001&&id>=1004)
            {

                 Debug.LogWarning("当前武器是手枪，当前替换武器不是手枪!");
                return;
            }
            if(currentIndex!=0&& id >= 1001 && id <= 1004)
            {
                Debug.LogWarning("当前武器是其他，当前替换武器是手枪!");
                return;
            }*/
            //    if(currentIndex == 0) { Debug.LogWarning("当前武器是手枪，不能替换!"); return; }
            //1.首先生成武器，覆盖原有的
            Debug.Log("手枪换手枪，其他武器换其他武器");
            weaponList[currentIndex] = GetWeapon(id);

            //发送指令
            RegisterWeaponCommandData temp = new RegisterWeaponCommandData();
            temp.weaponType = weaponList[currentIndex].weaponType;
            temp.bulletMag = weaponList[currentIndex].bwData.bulletMag;
            temp.weaponId = weaponList[currentIndex].weaponId;
            IC.SendCommand<RegisterWeaponCommand>(temp);



            //2.然后摧毁currentWeapon
            Destroy(currentWeapon.gameObject);
            currentWeapon = null;
            //3.然后执行
            SwitchWeapon(currentIndex);

        }
        private BaseWeapon GetWeapon(int id)
        {
            //1.检测这个武器有没有注册
            if (!weaponPath.ContainsKey(id))
            {
                Debug.LogError($"未注册的武器Id{id}");
                return null;
            }
            GameObject temp;
            //2.实例化想要打开的武器
            //2.1分已经加载，和没有加载
            if (!weaponDir.ContainsKey(id))
            {
                GameObject prefab = Resources.Load<GameObject>(weaponPath[id]);
                if (prefab == null)
                {
                    Debug.LogError($"无法加载预制体:{weaponPath[id]}");
                }
                weaponDir.Add(id, prefab);
            }
            Debug.Log("此处生成武器");
            temp = Instantiate(weaponDir[id], transform);

            //此处出现了问题，如果生成的物体不是禁用状态的话，OnEnable的切换武器指令会比注册指令先发从而导致BUG.

            //给武器加上组件，由于是Res，此处不用:如果需要id得改成武器脚本名字，通过反射
            BaseWeapon tempBase = temp.GetComponent<BaseWeapon>();

            if (tempBase == null)
            {
                Debug.LogError($"实例化对象{temp.name}不是BaseWeapon的子类"); return null;
            }
            return tempBase;
            //然后执行
           // PickUpWeapon(tempBase);
        }
    }
}
