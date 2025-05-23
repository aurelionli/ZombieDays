

using FPS_Manager;
using UnityEngine;

namespace FPS
{
    //这个等动画结束就进入Idle
    //或者可以被动打断(点击左键就会打断状态)
    public class ReloadState : NormalState
    {
        private bool reloadOver;
        public ReloadState(WeaponStateMachine stateMachine) : base(stateMachine)
        {
        }
        public override void Enter()
        {
            reloadOver = false;
            stateMachine.weapon.resuableData.cantRun = true;
            base.Enter();
            //todo:检测弹药
            if(stateMachine.weapon.resuableData.weaponType==WeaponType.Shot)//喷子
            {
                stateMachine.weapon.IC.PlaySFX("ShotOpen", stateMachine.weapon.player,AudioClipType.Weapon);
                stateMachine.weapon.animator.SetBool("Reload", true);
                return;
            }
            if(stateMachine.weapon.weaponId == 5001)  //特殊狙击
            {
                stateMachine.weapon.IC.PlaySFX("SniperOpen", stateMachine.weapon.player, AudioClipType.Weapon);
                stateMachine.weapon.animator.SetBool("Reload", true);
                return;
            }
            
            if(stateMachine.weapon.resuableData.weaponType== WeaponType.Hand)
            {
                if (CheckBullerMag())
                {
                    stateMachine.weapon.IC.PlaySFX("Hand1001Left", stateMachine.weapon.player, AudioClipType.Weapon);
                    stateMachine.weapon.animator.CrossFadeInFixedTime("reload_ammo_left", 0.1f);
                }
                else
                {
                    stateMachine.weapon.IC.PlaySFX("Hand1001Out",stateMachine.weapon.player, AudioClipType.Weapon);
                    stateMachine.weapon.animator.CrossFadeInFixedTime("reload_out_of_ammo", 0.1f);
                }
                return;
            }
            if (CheckBullerMag())
            {
                stateMachine.weapon.IC.PlaySFX($"{stateMachine.weapon.resuableData.weaponType}{stateMachine.weapon.resuableData.weaponId}Left",  stateMachine.weapon.player,AudioClipType.Weapon);
                stateMachine.weapon.animator.CrossFadeInFixedTime("reload_ammo_left", 0.1f);
            }
            else
            {
                stateMachine.weapon.IC.PlaySFX($"{stateMachine.weapon.resuableData.weaponType}{stateMachine.weapon.resuableData.weaponId}Out", stateMachine.weapon.player, AudioClipType.Weapon);
                stateMachine.weapon.animator.CrossFadeInFixedTime("reload_out_of_ammo", 0.1f);
            }

        }
        public override void Update()
        {
            if(stateMachine.weapon.resuableData.weaponType != WeaponType.Shot|| stateMachine.weapon.weaponId == 5001) { return; }
            if (stateMachine.aim.triggered || stateMachine.fire.triggered )
            {
                stateMachine.weapon.animator.SetBool("Reload", false);
                reloadOver = true;
                // stateMachine.ChangeState(stateMachine.idleState);
            }
            base.Update();
         
        }
        public override void OnAnimationEnterEvent()
        {
            if(reloadOver)
            {
                if (stateMachine.weapon.resuableData.weaponType == WeaponType.Shot)
                {
                    stateMachine.weapon.IC.PlaySFX("ShotClose",  stateMachine.weapon.player,AudioClipType.Weapon);

                }
                else if (stateMachine.weapon.weaponId == 5001)
                {
                    stateMachine.weapon.IC.PlaySFX("SniperClose", stateMachine.weapon.player, AudioClipType.Weapon);
                }
            }
            else
            {
                if (stateMachine.weapon.resuableData.weaponType == WeaponType.Shot)
                {
                    stateMachine.weapon.IC.PlaySFX("ShotInsert", stateMachine.weapon.player, AudioClipType.Weapon);

                }
                else if (stateMachine.weapon.weaponId == 5001)
                {
                    stateMachine.weapon.IC.PlaySFX("SniperInsert", stateMachine.weapon.player,AudioClipType.Weapon);
                }
            }
            
        }
        public override void OnAnimationTransitionEvent()
        {
            Debug.Log("换弹成功");
            
            //散弹这里检测子弹满了没
            if (stateMachine.weapon.resuableData.weaponType == WeaponType.Shot || stateMachine.weapon.weaponId == 5001)
            {
                stateMachine.weapon.IC.SendCommand<ReloadShotGunAmmoCommand>();
                //检测
                CheckReloadForShotGun();
          
            }
            else
            {
                stateMachine.weapon.IC.SendCommand<ReloadAmmoCommand>(stateMachine.weapon.bwData.bulletMag);
            }
        }
        private void CheckReloadForShotGun()
        {
            //1.第一步检查是否还有余的子弹
            if(CheckRelaod())
            {
                //2.如果有，那就检查当前塞满没
                if (stateMachine.weapon.IC.GetModel<CurrentWeaponModel>().weaponBulletMagDic[stateMachine.weapon.resuableData.weaponId]==stateMachine.weapon.bwData.bulletMag)
                {
                    stateMachine.weapon.animator.SetBool("Reload", false);
                    reloadOver = true;

                }
            }
            else
            {
                stateMachine.weapon.animator.SetBool("Reload", false);
                reloadOver = true;
            }
        }
        private bool CheckBullerMag()
        {
            if (stateMachine.weapon.IC.GetModel<CurrentWeaponModel>().weaponBulletMagDic[stateMachine.weapon.resuableData.weaponId]>0)
            {
                return true;
            }
            return false;
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
