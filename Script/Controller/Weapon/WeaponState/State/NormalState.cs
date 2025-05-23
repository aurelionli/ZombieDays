

using FPS_Manager;
using UnityEngine;

namespace FPS
{
    public class NormalState : IWeaponState
    {
        protected WeaponStateMachine stateMachine;

        public NormalState(WeaponStateMachine stateMachine)
        {
            this.stateMachine = stateMachine;

        }
        public virtual void Enter()
        {
            AddCallbackActions();
        }

        public virtual void Exit()
        {
            RemoveCallbackActions();
        }

        public virtual void HandleInput()
        {
            
        }

        public virtual void OnAnimationEnterEvent()
        {
           
        }

        public virtual void OnAnimationExitEvent()
        {
           
        }

        public virtual void OnAnimationTransitionEvent()
        {
            
        }

        public virtual void PhysicsUpdate()
        {
            
        }

        public virtual void Update()
        {
            
            UpdataMoveSpeed();
        }


        protected virtual void AddCallbackActions()
        {
            

        }

        /// <summary>
        /// 回调函数中，只有移动输入performed和canceled
        /// </summary>
        protected virtual void RemoveCallbackActions()
        {
           


        }
       
        protected bool CheckAmmo()
        {
          //  Debug.Log($"当前武器类型为{stateMachine.weapon.resuableData.weaponType},还有弹药还剩下{stateMachine.weapon.IC.GetModel<CurrentWeaponModel>().weaponsDir[stateMachine.weapon.resuableData.weaponType].bulletMag}");
            if (stateMachine.weapon.IC.GetModel<CurrentWeaponModel>().weaponBulletMagDic[stateMachine.weapon.resuableData.weaponId] <=0)
            {
                return false;
            }
            return true;
        }
        protected bool CheckRelaod()
        {
            if (stateMachine.weapon.IC.GetModel<CurrentWeaponModel>().weaponBulletNumDic[stateMachine.weapon.resuableData.weaponType] <= 0)
            {
                return false;
            }
            return true;
        }
        private void UpdataMoveSpeed()
        {
         
            stateMachine.weapon.animator.SetFloat("MoveSpeed", stateMachine.weapon.resuableData.moveSpeed);
           // stateMachine.weapon.IC.TriggerEvent("IsMoving", stateMachine.weapon.resuableData.moveSpeed);
        }
    }
}
