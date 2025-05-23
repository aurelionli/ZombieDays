
using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace FPS
{
    //等射击完毕进入瞄准orIdle 需要ResuableData
    public class FireState : NormalState
    {
        public FireState(WeaponStateMachine stateMachine) : base(stateMachine)
        {
            
        }
        /*  IC.RegisterObjectPool("Bullet", "Object/BulletCasing/Bullet_Prefab");
            IC.RegisterObjectPool("GrenadeLauncher", "Object/BulletCasing/Grenade_Launcher_01_Projectile");
            //注册三个弹壳
            IC.RegisterObjectPool("BigCasing", "Object/BulletCasing/Big_Casing_Prefab");
            IC.RegisterObjectPool("SmallCasing", "Object/BulletCasing/Small_Casing_Prefab");
            IC.RegisterObjectPool("ShotCasing", "Object/BulletCasing/Shotgun_Shell_Prefab");*/

       
        
        public override void Enter()
        {
            stateMachine.weapon.IC.SendCommand<WeaponShootCommand>(stateMachine.weapon.resuableData.weaponType);

            stateMachine.weapon.IC.TriggerEvent("WeaponShoot");
            stateMachine.weapon.resuableData.cantRun = true;
            stateMachine.weapon.resuableData.enterFireTime = Time.time ;
        
            if(stateMachine.weapon.resuableData.silencer)
            {
                stateMachine.weapon.IC.PlaySFX("ShootSilencer", stateMachine.weapon.player, AudioClipType.Normal, 1, 130);
            }
            else
            {
                stateMachine.weapon.IC.PlaySFX("Shoot", stateMachine.weapon.player,AudioClipType.Normal,1,130);
            }
           
            SetFireMode();
            if(stateMachine.weapon.resuableData.weaponType==WeaponType.Shot)
            {
                ShootShot();
            }
            else
            {
                Shoot(stateMachine.weapon.bulletShootPoint);
            }
            
            base.Enter();
        }
        private void ShootShot()
        {


            //1.子弹减少

            //2.获得子弹生成位置
           
            //3.灯光闪烁 todo:协程关闭0.1f;
            stateMachine.weapon.muzzleflashLight.enabled = true;
            float targetValue = 0f;
            DOTween.To(() => targetValue, x => targetValue = x, 1f, 0.1f).OnComplete(() => stateMachine.weapon.muzzleflashLight.enabled = false);
            //4.两个粒子特效发射
            stateMachine.weapon.muzzlePatic.Emit(1);
            stateMachine.weapon.sparkPatic.Emit(UnityEngine.Random.Range(5, 8));
            //5.UI准心
            //6.射击偏移
            //7.生成子弹
            for (int i = 0; i < stateMachine.weapon.bulletShootPointGroup.Count; i++)
            {
                Debug.Log("生成子弹"+ stateMachine.weapon.bulletShootPointGroup.Count);
                Transform shootDirection = stateMachine.weapon.bulletShootPointGroup[i];
                GameObject bullet = stateMachine.weapon.IC.GetObject("Bullet", shootDirection.position, shootDirection.rotation);
                // GameObject bullet = GameObject.Instantiate(stateMachine.weapon.bullerPrefab, shootDirection.position, shootDirection.rotation);//生成子弹
                //8.为子弹添加力
                //  bullet.GetComponent<TrailRenderer>().Clear();
                bullet.GetComponent<BulletScript>().InitDamage(stateMachine.weapon.bwData.damage);
                bullet.GetComponent<Rigidbody>().velocity = shootDirection.forward * stateMachine.weapon.bwData.bulletForce;
            }


            // GameObject.Instantiate(stateMachine.weapon.casingPrefab, stateMachine.weapon.casingBulletSpawnPoint.position, stateMachine.weapon.casingBulletSpawnPoint.rotation);

            stateMachine.weapon.IC.GetObject("ShotCasing", stateMachine.weapon.casingBulletSpawnPoint.position, stateMachine.weapon.casingBulletSpawnPoint.rotation);
            //9.生成弹壳
            //10.

        }
        private void Shoot(Transform shootPoint)
        {


            //1.子弹减少

            //2.获得子弹生成位置
            Vector3 shootDirection = shootPoint.forward;
            //3.灯光闪烁 todo:协程关闭0.1f;
            stateMachine.weapon.muzzleflashLight.enabled = true;
            float targetValue = 0f;
            DOTween.To(() => targetValue, x => targetValue = x, 1f, 0.1f).OnComplete(()=> stateMachine.weapon.muzzleflashLight.enabled = false);
            //4.两个粒子特效发射
            stateMachine.weapon.muzzlePatic.Emit(1);
            stateMachine.weapon.sparkPatic.Emit(UnityEngine.Random.Range(5, 8));
            //5.UI准心
            //6.射击偏移
            //7.生成子弹
            // GameObject bullet = GameObject.Instantiate(stateMachine.weapon.bullerPrefab, shootPoint.position, shootPoint.rotation);//生成子弹
            GameObject bullet;
            if (stateMachine.weapon.resuableData.weaponType != WeaponType.Rpg)
            {
                bullet = stateMachine.weapon.IC.GetObject("Bullet", shootPoint.position, shootPoint.rotation);
                bullet.GetComponent<BulletScript>().InitDamage(stateMachine.weapon.bwData.damage);
                bullet.GetComponent<TrailRenderer>().Clear();
            }
            else
            {
                bullet = stateMachine.weapon.IC.GetObject("GrenadeLauncher", shootPoint.position, shootPoint.rotation);
                bullet.GetComponent<ProjectileScript>().InitDamage(stateMachine.weapon.bwData.damage);

            }
                
            //8.为子弹添加力
            bullet.GetComponent<Rigidbody>().velocity = shootDirection * stateMachine.weapon.bwData.bulletForce;
            //9.生成弹壳
            if (stateMachine.weapon.resuableData.weaponType != WeaponType.Rpg)
            {
                if(stateMachine.weapon.resuableData.weaponType==WeaponType.Hand|| stateMachine.weapon.resuableData.weaponType == WeaponType.Smg)
                {
                    stateMachine.weapon.IC.GetObject("SmallCasing", stateMachine.weapon.casingBulletSpawnPoint.position, stateMachine.weapon.casingBulletSpawnPoint.rotation);

                }
                if (stateMachine.weapon.resuableData.weaponType == WeaponType.Arms|| stateMachine.weapon.resuableData.weaponType == WeaponType.Sniper)
                {
                    stateMachine.weapon.IC.GetObject("BigCasing", stateMachine.weapon.casingBulletSpawnPoint.position, stateMachine.weapon.casingBulletSpawnPoint.rotation);
                }

                    // GameObject.Instantiate(stateMachine.weapon.casingPrefab, stateMachine.weapon.casingBulletSpawnPoint.position, stateMachine.weapon.casingBulletSpawnPoint.rotation);
            } //10.


        }

        private void SetFireMode()
        {
            Debug.Log($"此时aimMode{stateMachine.weapon.resuableData.aimMode}");
            if(stateMachine.weapon.resuableData.aiming)
            {
                switch (stateMachine.weapon.resuableData.aimMode)
                { 
                    case 0:
                        stateMachine.weapon.animator.CrossFadeInFixedTime("aim_fire", 0.1f);
                        break;
                    case 1:
                        stateMachine.weapon.animator.CrossFadeInFixedTime("aim_fire_scope_01", 0.1f);
                        break;
                    case 2:
                        stateMachine.weapon.animator.CrossFadeInFixedTime("aim_fire_scope_02", 0.1f);
                        break;
                    case 3:
                        stateMachine.weapon.animator.CrossFadeInFixedTime("aim_fire_scope_03", 0.1f);
                        break;
                    case 4:
                        stateMachine.weapon.animator.CrossFadeInFixedTime("aim_fire_scope_04", 0.1f);
                        break;
                }
                return;
            }
            stateMachine.weapon.animator.CrossFadeInFixedTime("fire", 0.1f);
        }

        public override void Exit()
        {
         
            base.Exit();

        }

       

        public override void Update()
        {
            base.Update();
           
            if (CheckAmmo()&&Time.time >= stateMachine.weapon.resuableData.enterFireTime + stateMachine.weapon.bwData.fireRate)
            {
              //  Debug.Log(Time.time + "  " + stateMachine.weapon.resuableData.enterFireTime + " " + stateMachine.weapon.bwData.fireRate);

                if (stateMachine.fire.phase == InputActionPhase.Performed && stateMachine.weapon.auto)
                {
                    stateMachine.ChangeState(stateMachine.fireState);
                   // stateMachine.weapon.resuableData.enterFireTime = Time.time; // 重置射击时间
                }
                else if (stateMachine.fire.triggered && !stateMachine.weapon.auto)
                {
                    stateMachine.ChangeState(stateMachine.fireState);
                   // stateMachine.weapon.resuableData.enterFireTime = Time.time; // 重置射击时间
                }
            }

        }
        public override void OnAnimationExitEvent()
        {
            if(stateMachine.weapon.resuableData.aiming)
            {
                stateMachine.ChangeState(stateMachine.aimState);
                stateMachine.weapon.resuableData.lastStateIsFire = true;
                return;
            }
            stateMachine.weapon.resuableData.lastStateIsFire = false;
            stateMachine.weapon.resuableData.cantRun = false;
            stateMachine.ChangeState(stateMachine.idleState);
            
        }

      
    }
}
