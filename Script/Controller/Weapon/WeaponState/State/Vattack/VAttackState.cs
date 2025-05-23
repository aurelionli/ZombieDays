

using UnityEngine;
using UnityEngine.InputSystem.HID;

namespace FPS
{
    public class VattackState : NormalState
    {
        private int enmeyMask = 1 << 11;
        public VattackState(WeaponStateMachine stateMachine) : base(stateMachine)
        {
        }
        public override void Enter()
        {
            stateMachine.weapon.resuableData.cantRun = true;
            base.Enter();

            int i = Random.Range(0, 2);
            switch (i)
            {
                case 0:
                    stateMachine.weapon.animator.CrossFadeInFixedTime("knife_attack_1", 0.1f);
                    break;
                case 1:
                    stateMachine.weapon.animator.CrossFadeInFixedTime("knife_attack_2", 0.1f);
                    break;
                default:
                    Debug.LogError("随机出错!");
                    return;
            }

     
        }
        public override void OnAnimationTransitionEvent()
        {
            int i = Random.Range(0, 2); 
            stateMachine.weapon.IC.PlaySFX($"Knife0{i}", stateMachine.weapon.gameObject, AudioClipType.Normal, 1, 130);
            Collider[] colliders = Physics.OverlapSphere(stateMachine.weapon.transform.position + stateMachine.weapon.transform.forward * 1.23f + Vector3.up, 1f, enmeyMask);
            foreach (Collider collider in colliders)
            {
                Debug.Log(collider.name);
                if (collider.GetComponent<IDamage>() != null)
                {
                    collider.GetComponent<IDamage>().Damage(200);
                    stateMachine.weapon.Sc();
                }
            }
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
