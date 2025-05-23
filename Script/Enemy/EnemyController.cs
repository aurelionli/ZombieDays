using BehaviorDesigner.Runtime;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
namespace FPS
{
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(NavMeshAgent))]
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(CapsuleCollider))]
    public class EnemyController : MonoBehaviour, IController, IDamage
    {
        private IController IC;
        // Start is called before the first frame update
        private BehaviorTree bt;

        private Animator animator;
        private CapsuleCollider capsule;
        private Rigidbody rb;

        public int health;

        private float time;
        private float randomCount;

        public LayerMask layerMask;

        private void Awake()
        {
            IC = this;
            time = 5f;
            animator = GetComponent<Animator>();
            bt = GetComponent<BehaviorTree>();
            rb = GetComponent<Rigidbody>();
            capsule = GetComponent<CapsuleCollider>();

            IC.RegisterAudioSource(gameObject,3);
            
        }
        private void OnDestroy()
        {
           IC.UnregisterAudioSource(gameObject);
      
        }
        private void OnEnable()
        {
            capsule.enabled = true;
        }
        void Start()
        {
            bt.SetVariableValue("Self", gameObject);
            bt.SetVariableValue("ViewDistance", 10f);
            bt.enabled = true;
        }
        public void SetAngry()
        {
            bt.SetVariableValue("Angry", true);
            bt.SetVariableValue("Player", GameObject.Find("Player"));
            
        }
        public void AngryOver()
        {
            bt.SetVariableValue("Angry", false);
            
        }
        // Update is called once per frame
        void Update()
        {
            if(time+randomCount<Time.time)
            {
                SetAttackSound();
            }
        }
        private void SetAttackSound()
        {
            int i = Random.Range(0, 4);
            IC.PlaySFX($"Call0{i}", gameObject,AudioClipType.Zombie, 0.1f, 130);
            randomCount = Random.Range(10, 20);
            time = Time.time;
        }
        public void Damage(int damage)
        {
            IC.TriggerEvent("HitEnemy");
            health -= damage;
            if(health<=0)
            {
                bt.SendEvent("Dead");
                IC.PlaySFX($"Dead", gameObject, AudioClipType.Zombie);
                capsule.enabled = false;
                StartCoroutine(ReturnObject());
                return;
            }
          
            bt.SendEvent("Hurt");
            int i = Random.Range(0, 4);
            IC.PlaySFX($"Hurt0{i}", gameObject, AudioClipType.Zombie);
        }
         private IEnumerator ReturnObject()
        {
            
            yield return new WaitForSeconds(5f);
            health = 50;
            IC.ReturnObject("Zombie", gameObject);
         
        }
        public void  PlayerDead()
        {
            health = 50;
            IC.ReturnObject("Zombie", gameObject);

        }
        //受伤后，视野45-55左右
        //平时，视野10-15

        //角色开枪，视野35
        //角色消音开枪,视野20

        //伤害暂定
        
        public void Anim_Atack(int hit)
        {
            IC.PlaySFX("Attack", gameObject, AudioClipType.Zombie, 1, 130);
            int i = Random.Range(0, 2);
            IC.PlaySFX($"AttackSound0{i}", gameObject, AudioClipType.Zombie, 1, 130);
          
            Collider[] colliders = Physics.OverlapSphere(transform.position+transform.forward* 1.23f+ Vector3.up,1f, layerMask);
            foreach(Collider collider in colliders)
            {
                Debug.Log(collider.name);
                if(collider.GetComponent<IDamage>()!=null)
                {
                    collider.GetComponent<IDamage>().Damage(hit);
                }
            }
        }
       
        public void SetCanSeeObjectValue(float value)
        {
            bt.SetVariableValue("ViewDistance", value);
        }

    }
}

