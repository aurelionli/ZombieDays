using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
namespace FPS
{
    public class SMG03Controller : SmgWeapon
    {

        protected override void Awake()
        {
            base.Awake();
            bwData = IC.GetData<Player_SO>().SMG03Data;

        }


        protected override void OnEnable()
        {
            base.OnEnable();
            SwitchWeaponCommandData temp = new SwitchWeaponCommandData();
            temp.weaponId = weaponId;
            temp.Type = weaponType;
            IC.SendCommand<SwitchWeaponCommand>(temp);

        }
    }
}