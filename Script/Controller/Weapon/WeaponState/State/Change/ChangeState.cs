using UnityEngine;
using UnityEngine.InputSystem;
namespace FPS
{
    //这个等动画结束就进入Idle
    //或者可以被动打断(点击左键就会打断状态)
    public class ChangeState : NormalState
    {
        public ChangeState(WeaponStateMachine stateMachine) : base(stateMachine)
        {
        }
        private bool canExit;
        public override void Enter()
        {
            canExit = false;
            stateMachine.weapon.resuableData.cantRun = true;
            base.Enter();
            //todo:检测弹药
            stateMachine.weapon.animator.SetBool("Change", true);
        }
        public override void OnAnimationTransitionEvent()
        {
            Debug.Log("打开改装面板");
            canExit = true;
            stateMachine.weapon.IC.TriggerEvent("OpenChangWeaponPanel"); 
        }

        public override void Update()
        {
            if(stateMachine.change.triggered&& canExit)
            {
                stateMachine.weapon.IC.TriggerEvent("OpenChangWeaponPanel");
                stateMachine.ChangeState(stateMachine.idleState);
              
            }
            base.Update();
        }
        public override void Exit()
        {
            stateMachine.weapon.animator.SetBool("Change", false);
            stateMachine.weapon.resuableData.cantRun = false;
            base.Exit();
        }
    }
}
