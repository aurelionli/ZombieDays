using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using static UnityEngine.UI.ContentSizeFitter;
using FPS_Manager;
namespace FPS
{
    public class BaseWeapon : MonoBehaviour, IController
    {
        [HideInInspector] public List<Transform> bulletShootPointGroup;//这是给霰弹的
        [HideInInspector] public Transform bulletShootPoint;//子弹特效位置
        [HideInInspector] public Transform casingBulletSpawnPoint;//抛壳位置
        [HideInInspector] public Transform grenadeSpawnPoint;//手雷位置
        [HideInInspector] public ParticleSystem muzzlePatic;//第一个例子火焰特效
        [HideInInspector] public ParticleSystem sparkPatic;//第二个例子特效
        [HideInInspector] public Light muzzleflashLight;//光照


        public int weaponId;

        [HideInInspector] public GameObject player;

        /* public GameObject grenadePrefab;//手雷
         public GameObject casingPrefab; //弹壳 
         public GameObject bullerPrefab;//子弹*/

         public int aimMode;
         public bool silencerMode;

        /*1.aim_fire，fire的EXIT
         * 2.扔雷的 transi，exit
         * 3.show的exit
         * 改装的trans
         * 刀的tran exit
         * 换弹的tran exit
         */
         public WeaponType weaponType;
        [HideInInspector] public bool auto;

        [HideInInspector] public Animator animator;

        [HideInInspector] public PlayerReuseableData resuableData;

        [HideInInspector] public WeaponStateMachine stateMachine;

        [HideInInspector] public IController IC;

        [HideInInspector] public BaseWeaponData bwData;

     
        protected virtual void Awake()
        {

            //1.初始化接口引用
            IC = this;
            
            //2.建立武器状态机
            stateMachine = new WeaponStateMachine(this);
           //3.初始化：寻找组件
            InitFindGameObject();


            player = FindObjectOfType<PlayerController>().gameObject;
            //1.获得外部循环组件
            resuableData = FindObjectOfType<PlayerController>().reuseableData;
        }
        protected virtual void InitFindGameObject()
        {
            grenadeSpawnPoint = GameTools.GetSingleComponentInChild<Transform>(gameObject, "Grenade_Spawn_Point");
            casingBulletSpawnPoint = GameTools.GetSingleComponentInChild<Transform>(gameObject, "Casing Spawn Point");
            muzzlePatic = GameTools.GetSingleComponentInChild<ParticleSystem>(gameObject, "Muzzleflash Particles");
            sparkPatic = GameTools.GetSingleComponentInChild<ParticleSystem>(gameObject, "SparkParticles");
            muzzleflashLight = GameTools.GetSingleComponentInChild<Light>(gameObject, "Muzzleflash Light");
            animator = GetComponent<Animator>();


        //    grenadePrefab = Resources.Load<GameObject>("Object/BulletCasing/Hand_Grenade_Prefab");
          //  bullerPrefab = Resources.Load<GameObject>("Object/BulletCasing/Bullet_Prefab");

        }
        protected virtual void Start()
        {
            aimMode = 0;
            //2.初始化进入idle状态
            stateMachine.ChangeState(stateMachine.idleState);
           
        }

        private void Update()
        {
            stateMachine.Update();
        }
        private void FixedUpdate()
        {
            stateMachine.PhysicsUpdate();
        }

        private void Anim_Enter()
        {
            stateMachine.OnAnimationEnterEvent();
        }
        private void Anim_Transition()
        {
            stateMachine.OnAnimationTransitionEvent();
        }
        private void Anim_Exit()
        {
            stateMachine.OnAnimationExitEvent();
        }
        public void Sc()
        {
            StartCoroutine(PauseFrame(pauseDuration));

        }

        public float pauseDuration = 0.1f;  // 顿帧持续时间

        IEnumerator PauseFrame(float duration)
        {
            Time.timeScale = 0f;  // 暂停游戏
            yield return new WaitForSecondsRealtime(duration);  // 等待指定的时间
            Time.timeScale = 1f;  // 恢复游戏
        }


    }
}
