using FPS_Manager;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;


namespace FPS
{
    public class AimState : NormalState
    {
        public AimState(WeaponStateMachine stateMachine) : base(stateMachine)
        {
        }

        public override void Enter()
        {
            stateMachine.weapon.IC.TriggerEvent("WeaponCrossHairHideShow",false);
            stateMachine.weapon.resuableData.cantRun = true;
            stateMachine.weapon.resuableData.aiming = true;
            stateMachine.weapon.animator.SetInteger("AimMode",stateMachine.weapon.aimMode);
           
            stateMachine.weapon.animator.SetBool("Aim",true);

           // if(stateMachine.weapon.resuableData.type ==WeaponType.Sniper)
            
            stateMachine.weapon.IC.TriggerEvent("SniperAim", stateMachine.weapon.bwData.aimGunCameraView);
            stateMachine.weapon.IC.TriggerEvent("NorAim", 40f);
            if (!stateMachine.weapon.resuableData.lastStateIsFire)
            {
                stateMachine.weapon.IC.PlaySFX("Aim", stateMachine.weapon.player);
            }
            
            base.Enter();
        }

        public override void Update()
        {
            base.Update();
            if(CheckAmmo()&&Time.time >= stateMachine.weapon.resuableData.enterFireTime + stateMachine.weapon.bwData.fireRate)
            {
                if (stateMachine.weapon.auto && stateMachine.fire.phase == InputActionPhase.Performed)
                {
                    stateMachine.ChangeState(stateMachine.fireState);
                    return;

                }
                if (!stateMachine.weapon.auto && stateMachine.fire.triggered)
                {
                    stateMachine.ChangeState(stateMachine.fireState);
                    return;
                }
            }
           
            if (stateMachine.aim.phase == InputActionPhase.Waiting)
            {
              //  if (stateMachine.weapon.resuableData.type == WeaponType.Sniper)
                {
                    stateMachine.weapon.IC.TriggerEvent("SniperAim", stateMachine.weapon.bwData.normalGunCameraView);
                    stateMachine.weapon.IC.TriggerEvent("NorAim", 60f);
                }
                stateMachine.weapon.IC.TriggerEvent("WeaponCrossHairHideShow",true);
                stateMachine.weapon.IC.PlaySFX("Aim",stateMachine.weapon.player);
                stateMachine.weapon.resuableData.cantRun = false;
                stateMachine.weapon.resuableData.aiming = false;
                stateMachine.weapon.animator.SetBool("Aim", false);
                stateMachine.ChangeState(stateMachine.idleState);
               
            }
            
        }

        public override void Exit()
        {
           
            base.Exit();
        }

    }
}
