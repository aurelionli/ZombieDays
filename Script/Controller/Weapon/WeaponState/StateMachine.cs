
using Unity.VisualScripting;
using UnityEngine;

namespace FPS
{
    public class StateMachine{
        protected IWeaponState currentState;
        public void ChangeState(IWeaponState newState) {
            currentState?.Exit();
            currentState = newState;
            Debug.Log(newState);
            currentState.Enter();
        }
        public void HandleInput() {
            currentState?.HandleInput();
        }

        public void Update() {
            currentState?.Update();
        }

        public void PhysicsUpdate(){
            currentState?.PhysicsUpdate();
        }
        public void OnAnimationEnterEvent() {
            currentState?.OnAnimationEnterEvent();
        }
        public void OnAnimationExitEvent() {
            currentState?.OnAnimationExitEvent();
        }
        public void OnAnimationTransitionEvent(){
            currentState?.OnAnimationTransitionEvent();
        }
    }
}