
using FPS_Manager;
using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace FPS
{
    //这个就需要与移动配合。
    public class IdleState : NormalState
    {
        public IdleState(WeaponStateMachine stateMachine) : base(stateMachine)
        {

        }

       
        public override void Enter()
        {
           stateMachine.weapon.resuableData.canSwitchWeapon = true;
            base.Enter();
        }
        public override void HandleInput()
        {
            //1.瞄准(根据aimmode)，近战(两个随机一个)，扔雷(现有声音再有动作)，改装(等再按一次才进入idle)，检查(随时打断)，射击(腰射)
            

            
        }

        public override void Update()
        {
            if(stateMachine.reload.phase == InputActionPhase.Performed&& CheckRelaod()&& CheckAmmoEnough())
            {
                
                stateMachine.ChangeState(stateMachine.reloadState);
            }
            if (stateMachine.throwT.phase == InputActionPhase.Performed&& stateMachine.weapon.IC.GetModel<CurrentWeaponModel>().throwNum > 0)
            {
                stateMachine.ChangeState(stateMachine.throwState);
                
            }
            if (stateMachine.vAttack.phase == InputActionPhase.Performed)
            {
                stateMachine.ChangeState(stateMachine.vAttackState);
               
            }
            if (stateMachine.aim.phase == InputActionPhase.Performed)
            {
                stateMachine.ChangeState(stateMachine.aimState);
              
            }
            if(stateMachine.change.triggered)
            {
               stateMachine.ChangeState(stateMachine.changeState);
              
            }
            if (stateMachine.show.triggered)
            {
                stateMachine.ChangeState(stateMachine.showState);
            
            }
            if(CheckAmmo())
            {
                if (stateMachine.weapon.auto)//自动
                {
                    if (stateMachine.fire.phase == InputActionPhase.Performed && Time.time >= stateMachine.weapon.resuableData.enterFireTime + stateMachine.weapon.bwData.fireRate)
                    {
                        stateMachine.ChangeState(stateMachine.fireState);
                        return;
                    }
                }
                else
                {
                    if (stateMachine.fire.triggered && Time.time >= stateMachine.weapon.resuableData.enterFireTime + stateMachine.weapon.bwData.fireRate)
                    {
                        stateMachine.ChangeState(stateMachine.fireState);

                    }
                }
            }
            
           
            
          base.Update();
        }
        public override void Exit()
        {
            stateMachine.weapon.resuableData.canSwitchWeapon = false;
            base.Exit();

        }
        /// <summary>
        /// 防止重复装弹
        /// </summary>
        /// <returns></returns>
        private bool CheckAmmoEnough()
        {
            if (stateMachine.weapon.IC.GetModel<CurrentWeaponModel>().weaponBulletMagDic[stateMachine.weapon.resuableData.weaponId] >=stateMachine.weapon.bwData.bulletMag)
            {
                return false;
            }
            return true;
        }
       
        protected override void AddCallbackActions()
        {
            stateMachine.fire.performed += Fire;
        }

        private void Fire(InputAction.CallbackContext context)
        {
           
        }
         

        protected override void RemoveCallbackActions()
        {
            stateMachine.fire.performed -= Fire;
        }

      
    }
}
