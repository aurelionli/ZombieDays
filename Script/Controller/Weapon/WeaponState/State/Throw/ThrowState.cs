

using UnityEngine;

namespace FPS
{
    //这个等动画结束就进入Idle
    public class ThrowState : NormalState
    {
        public ThrowState(WeaponStateMachine stateMachine) : base(stateMachine)
        {
        }
        public override void Enter()
        {
            stateMachine.weapon.resuableData.cantRun = true;
            base.Enter();
            //todo:检测炸药
            stateMachine.weapon.animator.CrossFadeInFixedTime("grenade_throw", 0.1f);
        }
        public override void OnAnimationTransitionEvent()
        {
            Debug.Log("换弹成功");
            stateMachine.weapon.IC.SendCommand<DecreaseGeThrowCommand>();
            stateMachine.weapon.IC.GetObject("HandGrenade", stateMachine.weapon.grenadeSpawnPoint.position, stateMachine.weapon.transform.rotation);//生成子弹 );
        //   GameObject.Instantiate(stateMachine.weapon.grenadePrefab, stateMachine.weapon.grenadeSpawnPoint.position, stateMachine.weapon.transform.rotation);//生成子弹
         
        }

        public override void OnAnimationExitEvent()
        {
           stateMachine.ChangeState(stateMachine.idleState);
          
        }
        public override void Exit()
        {
            stateMachine.weapon.resuableData.cantRun = false;
            base.Exit();
        }
    }
}
