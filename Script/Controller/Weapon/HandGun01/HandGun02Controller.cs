using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using FPS_Manager;
namespace FPS
{
    public class HandGun02Controller : HandWeapon
    {
      /*  private Transform Scope02Mesh;
        private Transform Scope03Mesh;

        private SkinnedMeshRenderer scope02Skin;
        private SkinnedMeshRenderer scope03Skin;

        private SkinnedMeshRenderer silencer;//消音器

        private List<SkinnedMeshRenderer> scopeSkinGroup;
        private List<Transform> scopeMeshGroup;*/

        protected override void Awake()
        {
            base.Awake();
           /* silencerMode = false;
            weaponType = WeaponType.Hand;
            auto = false;*/
           // bwData = FindObjectOfType<PlayerController>().player_SO.HG02Data;
            bwData = IC.GetData<Player_SO>().HG02Data;


        }
        /*protected override void Start()
        {
            
           
            InitGameObjectState();
            base.Start();
           


        }*/
        protected override void OnEnable()
        {
            base.OnEnable();
            // InitListenEvent();
            /* resuableData.weaponType = WeaponType.Hand;
             resuableData.silencer = silencerMode;
             resuableData.aimMode = aimMode;
             resuableData.weaponId = weaponId;*/

            SwitchWeaponCommandData temp = new SwitchWeaponCommandData();
            temp.weaponId = weaponId;
            temp.Type = weaponType;
            IC.SendCommand<SwitchWeaponCommand>(temp);
          //  IC.PlaySFX("TakeOutWeapon", player);
        }
       /* protected override void InitFindGameObject()
        {
            base.InitFindGameObject();
            scopeSkinGroup = new List<SkinnedMeshRenderer>();
            scopeMeshGroup = new List<Transform>();
           
            Scope02Mesh = GameTools.GetSingleComponentInChild<Transform>(gameObject, "Scope 2 Render Mesh");
            scopeMeshGroup.Add(Scope02Mesh);
            Scope03Mesh = GameTools.GetSingleComponentInChild<Transform>(gameObject, "Scope 3 Render Mesh");
            scopeMeshGroup.Add(Scope03Mesh);
         
      
            scope02Skin = GameTools.GetSingleComponentInChild<SkinnedMeshRenderer>(gameObject, "scope_02");
            scopeSkinGroup.Add(scope02Skin);
            scope03Skin = GameTools.GetSingleComponentInChild<SkinnedMeshRenderer>(gameObject, "scope_03");
            scopeSkinGroup.Add(scope03Skin);
   
            silencer = GameTools.GetSingleComponentInChild<SkinnedMeshRenderer>(gameObject, "silencer");

            bulletShootPoint = GameTools.GetSingleComponentInChild<Transform>(gameObject, "Bullet Spawn Point");
            casingPrefab = Resources.Load<GameObject>("Object/BulletCasing/Small_Casing_Prefab");
        }
        protected void InitGameObjectState()
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
        }
        private void TriggerEvent_ChangeWeaponScope(object data)
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
                    resuableData.aimMode = 0;
                 
                    break;
                case 0:
                    //SetMeshTrue(0);
                    break;
                case 1:
                    SetMeshTrue(0);
                    break;
                case 2:
                    SetMeshTrue(1);
                    break;
                case 3:
                   // SetMeshTrue(3);
                    break;
            }
        }
        private void SetMeshTrue(int i)
        {
            scopeMeshGroup[i].gameObject.SetActive(true);
            scopeSkinGroup[i].enabled = true;
            aimMode = i + 2;
            resuableData.aimMode = i + 2;
        }
        #endregion

        private void OnDisable()
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