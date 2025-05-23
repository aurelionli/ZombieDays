using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using FPS_Manager;

namespace FPS
{
    public class HandWeapon :BaseWeapon
    {
      
        protected Transform Scope02Mesh;
        protected Transform Scope03Mesh;
 

       
        protected SkinnedMeshRenderer scope02Skin;
        protected SkinnedMeshRenderer scope03Skin;
   

        protected SkinnedMeshRenderer silencer;//消音器

        protected List<SkinnedMeshRenderer> scopeSkinGroup;
        protected List<Transform> scopeMeshGroup;

        protected override void Awake()
        {
            base.Awake();
            weaponType = WeaponType.Hand;
            auto = false;
            silencerMode = false;
        }



        protected virtual void OnEnable()
        {
            InitListenEvent();
            resuableData.weaponType = WeaponType.Hand;
            resuableData.silencer = silencerMode;
            resuableData.aimMode = aimMode;
            resuableData.weaponId = weaponId;
            IC.PlaySFX("TakeOutWeapon", player);
        }

        protected override void Start()
        {
            InitGameObjectState();
            base.Start();
        }

        protected override void InitFindGameObject()
        {
            base.InitFindGameObject();
            scopeSkinGroup = new List<SkinnedMeshRenderer>();
            scopeMeshGroup = new List<Transform>();
          //  Scope01Mesh = GameTools.GetSingleComponentInChild<Transform>(gameObject, "Scope 1 Render Mesh");
           // scopeMeshGroup.Add(Scope01Mesh);
            Scope02Mesh = GameTools.GetSingleComponentInChild<Transform>(gameObject, "Scope 2 Render Mesh");
            scopeMeshGroup.Add(Scope02Mesh);
            Scope03Mesh = GameTools.GetSingleComponentInChild<Transform>(gameObject, "Scope 3 Render Mesh");
            scopeMeshGroup.Add(Scope03Mesh);
          //  Scope04Mesh = GameTools.GetSingleComponentInChild<Transform>(gameObject, "Scope 4 Render Mesh");
          //  scopeMeshGroup.Add(Scope04Mesh);

          //  scope01Skin = GameTools.GetSingleComponentInChild<SkinnedMeshRenderer>(gameObject, "scope_01");
           // scopeSkinGroup.Add(scope01Skin);
            scope02Skin = GameTools.GetSingleComponentInChild<SkinnedMeshRenderer>(gameObject, "scope_02");
            scopeSkinGroup.Add(scope02Skin);
            scope03Skin = GameTools.GetSingleComponentInChild<SkinnedMeshRenderer>(gameObject, "scope_03");
            scopeSkinGroup.Add(scope03Skin);
          //  scope04Skin = GameTools.GetSingleComponentInChild<SkinnedMeshRenderer>(gameObject, "scope_04");
          //  scopeSkinGroup.Add(scope04Skin);

            silencer = GameTools.GetSingleComponentInChild<SkinnedMeshRenderer>(gameObject, "silencer");

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
            IC.StartListening("WeaponSilencer", TriggerEvent_WeaponSilencer);

        }
        #region TriggerEvent
        protected virtual void TriggerEvent_WeaponSilencer(object data)
        {
            bool value = (bool)data;
            silencer.enabled = value;
            resuableData.silencer = value;
            silencerMode = value;
        }
        protected virtual void TriggerEvent_ChangeWeaponScope(object data)
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
                    aimMode = 0;

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
        protected virtual void SetMeshTrue(int i)
        {
            scopeMeshGroup[i].gameObject.SetActive(true);
            scopeSkinGroup[i].enabled = true;
            aimMode = i + 2;
            resuableData.aimMode = i + 2;

        }
        #endregion
        protected void OnDisable()
        {
            IC.StopListening("WeaponChange", TriggerEvent_ChangeWeaponScope);
            IC.StopListening("WeaponSilencer", TriggerEvent_WeaponSilencer);
        }
        protected void OnDestroy()
        {
            IC.StopListening("WeaponChange", TriggerEvent_ChangeWeaponScope);
            IC.StopListening("WeaponSilencer", TriggerEvent_WeaponSilencer);
        }

    }
}
