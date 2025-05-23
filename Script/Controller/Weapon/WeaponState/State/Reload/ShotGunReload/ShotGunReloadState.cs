using System;
using System.Collections.Generic;


using UnityEngine;

namespace FPS
{
    //这个等动画结束就进入Idle
    //或者可以被动打断(点击左键就会打断状态)
    public class ShotGunReloadState : NormalState
    {
        public ShotGunReloadState(WeaponStateMachine stateMachine) : base(stateMachine)
        {
        }

    
        public override void Enter()
        {
        
            stateMachine.weapon.resuableData.cantRun = true;
            base.Enter();
            //todo:检测弹药
            stateMachine.weapon.animator.SetBool("Reload",true);
        }
        public override void Update()
        {
         
            if (stateMachine.aim.triggered || stateMachine.fire.triggered)
            {
                stateMachine.weapon.animator.SetBool("Reload", false);
                // stateMachine.ChangeState(stateMachine.idleState);
            }
            base.Update();
          
        }
      
        public override void OnAnimationTransitionEvent()
        {
            Debug.Log("换弹成功");
        }

        public override void OnAnimationExitEvent()
        {
           // stateMachine.ChangeState(stateMachine.idleState);
        }
        public override void Exit()
        {
            stateMachine.weapon.resuableData.cantRun = false;
            base.Exit();
        }
    }
}
