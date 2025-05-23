using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
namespace FPS
{
    public class SMG05Controller : SmgWeapon
    {
        private Transform smg_05_iron_sights;


        protected override void Awake()
        {
            base.Awake();
            bwData = IC.GetData<Player_SO>().SMG05Data;


        }

        protected override void OnEnable()
        {
            base.OnEnable();


            SwitchWeaponCommandData temp = new SwitchWeaponCommandData();
            temp.weaponId = weaponId;
            temp.Type = weaponType;
            IC.SendCommand<SwitchWeaponCommand>(temp);

        }


        

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
                    resuableData.aimMode = 0;
                    aimMode = 0;
                    smg_05_iron_sights.gameObject.SetActive(true);
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
            aimMode = i + 1;
            smg_05_iron_sights.gameObject.SetActive(false);
            resuableData.aimMode = i + 1;
        }

    }
}