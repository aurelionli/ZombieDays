
using System;
using System.Collections.Generic;
using UnityEngine;
using FPS_Manager;

namespace FPS
{
    public class PRGController : BaseWeapon
    {
        private Transform Scope01Mesh;
        private Transform Scope02Mesh;
        private Transform Scope03Mesh;
        private Transform Scope04Mesh;


        private SkinnedMeshRenderer scope01Skin;
        private SkinnedMeshRenderer scope02Skin;
        private SkinnedMeshRenderer scope03Skin;
        private SkinnedMeshRenderer scope04Skin;

  

        private List<SkinnedMeshRenderer> scopeSkinGroup;
        private List<Transform> scopeMeshGroup;
        protected override void Awake()
        {
            base.Awake();
            weaponType = WeaponType.Rpg;
            auto = false;
            bwData = FindObjectOfType<PlayerController>().player_SO.Geata;
           

        }
        protected override void Start()
        {
          
          
          //  InitListenEvent();
            InitGameObjectState();

           
         

            base.Start();

         
        }
        private void OnEnable()
        {
            InitListenEvent();
            resuableData.weaponType = WeaponType.Rpg;
            resuableData.aimMode = aimMode;
            resuableData.weaponId = weaponId;

            SwitchWeaponCommandData temp = new SwitchWeaponCommandData();
            temp.weaponId = weaponId;
            temp.Type = weaponType;
            IC.SendCommand<SwitchWeaponCommand>(temp);
            IC.PlaySFX("TakeOutWeapon", player);
        }
        protected override void InitFindGameObject()
        {
            base.InitFindGameObject();
            scopeSkinGroup = new List<SkinnedMeshRenderer>();
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

           
            bulletShootPoint = GameTools.GetSingleComponentInChild<Transform>(gameObject, "Bullet Spawn Point");


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
       

        }

        #region TriggerEvent

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
        private void SetMeshTrue(int i)
        {
            scopeMeshGroup[i].gameObject.SetActive(true);
            scopeSkinGroup[i].enabled = true;
            aimMode = i + 1;
            resuableData.aimMode = i + 1;
        }
        #endregion
        private void OnDisable()
        {
            IC.StopListening("WeaponChange", TriggerEvent_ChangeWeaponScope);
        
        }
        private void OnDestroy()
        {
            IC.StopListening("WeaponChange", TriggerEvent_ChangeWeaponScope);
           
        }
    }
}
