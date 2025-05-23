
using System;
using System.Collections.Generic;
using UnityEngine;
using FPS_Manager;

namespace FPS
{
    public class Sniper03Controller : BaseWeapon
    {
        protected override void Awake()
        {
            base.Awake();
            weaponType = WeaponType.Sniper;
            auto = false;
            bwData = IC.GetData<Player_SO>().S03Data;
           
        }
        protected override void Start()
        {
          
          

            base.Start();
         

        }
        private void OnEnable()
        {
            resuableData.weaponType = WeaponType.Sniper;
            resuableData.weaponId = weaponId;
            resuableData.aimMode = 0;
            SwitchWeaponCommandData temp = new SwitchWeaponCommandData();
            temp.weaponId = weaponId;
            temp.Type = weaponType;
            resuableData.silencer = false;
            IC.SendCommand<SwitchWeaponCommand>(temp);
            IC.PlaySFX("TakeOutWeapon", player);
        }
        protected override void InitFindGameObject()
        {
            bulletShootPoint = GameTools.GetSingleComponentInChild<Transform>(gameObject, "Bullet Spawn Point");
            base.InitFindGameObject();
        }
    }
}
