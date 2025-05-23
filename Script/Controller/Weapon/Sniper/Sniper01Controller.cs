
using System;
using System.Collections.Generic;
using UnityEngine;
using FPS_Manager;

namespace FPS
{
    public class Sniper01Controller : BaseWeapon
    {
        protected override void Awake()
        {
            base.Awake();
            weaponType = WeaponType.Sniper;
            auto = false;
            bwData = IC.GetData<Player_SO>().S01Data;
        }
        protected override void Start()
        {
            
            base.Start();

            base.Start();

        }
        private void OnEnable()
        {
            resuableData.weaponType = WeaponType.Sniper;
        
            IC.PlaySFX("TakeOutWeapon", player);
            resuableData.weaponId = weaponId;
            resuableData.aimMode = 0;
            resuableData.silencer = false;
            SwitchWeaponCommandData temp = new SwitchWeaponCommandData();
            temp.weaponId = weaponId;
            temp.Type = weaponType;
            IC.SendCommand<SwitchWeaponCommand>(temp);

        }
        protected override void InitFindGameObject()
        {
            bulletShootPoint = GameTools.GetSingleComponentInChild<Transform>(gameObject, "Bullet Spawn Point");
            base.InitFindGameObject();
        }
    }
}
