using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using FPS_Manager;
namespace FPS
{
    public class HandGun04Controller : HandWeapon
    {
        protected override void Awake()
        {
            base.Awake();
            /* silencerMode = false;
             weaponType = WeaponType.Hand;
             auto = false;*/
            // bwData = FindObjectOfType<PlayerController>().player_SO.HG02Data;
            bwData = IC.GetData<Player_SO>().HG04Data;


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
    }
}