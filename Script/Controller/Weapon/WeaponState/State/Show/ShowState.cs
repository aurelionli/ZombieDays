using UnityEngine;
using UnityEngine.InputSystem;
namespace FPS
{
    //这个等动画结束就进入Idle
    //或者可以被动打断(点击左键就会打断状态)
    public class ShowState : NormalState
    {
      public ShowState(WeaponStateMachine stateMachine) : base(stateMachine){ }
        public override void Enter(){
            stateMachine.weapon.resuableData.cantRun = true;
            base.Enter();
            //todo:检测弹药
            stateMachine.weapon.animator.SetBool("Show", true);
        }
        public override void Update() {
            if (stateMachine.aim.triggered|| stateMachine.fire.triggered){
                stateMachine.ChangeState(stateMachine.idleState);
            }
            base.Update();
        }
        public override void OnAnimationExitEvent(){
            stateMachine.ChangeState(stateMachine.idleState);
        }
        public override void Exit() {
            stateMachine.weapon.animator.SetBool("Show", false);
            stateMachine.weapon.resuableData.cantRun = false;
            base.Exit();
        }
    }
}
