using System.Collections.Generic;
using UnityEngine.InputSystem;

namespace FPS
{
    public class WeaponStateMachine:StateMachine {
        public BaseWeapon weapon;
        public InputAction fire;
        public InputAction vAttack;
        public InputAction reload;
        public InputAction change;
        public InputAction show;
        public InputAction aim;
        public InputAction throwT;
        public InputAction switchW;
        public FireState fireState;
        public IdleState idleState;
        public ReloadState reloadState;
        public ThrowState throwState;
        public VattackState vAttackState;
        public AimState aimState;
        public ChangeState changeState;
        public ShowState showState;
        public PlayerControls.WeaponActions weaponActions;
        public WeaponStateMachine(BaseWeapon player) {
            weapon = player;
            weaponActions = weapon.IC.GetWeaponControls();
            idleState = new IdleState(this);
            fireState = new FireState(this);
            reloadState = new ReloadState(this);
            throwState = new ThrowState(this);
            aimState = new AimState(this);
            vAttackState = new VattackState(this);
            changeState = new ChangeState(this);
            showState = new ShowState(this);
            fire = weaponActions.Fire;
            vAttack = weaponActions.VAttack;
            reload = weaponActions.Reload;
            change = weaponActions.Change;
            show = weaponActions.Show;
            aim = weaponActions.Aim;
            throwT = weaponActions.ThrowT;
            switchW = weaponActions.Switch;
        }
    }
}


