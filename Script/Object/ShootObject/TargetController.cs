using UnityEngine;
using System.Collections;

namespace FPS
{
    public class TargetController : MonoBehaviour,IController
    {
        private IController IC;
        float randomTime;
        bool routineStarted = false;
        bool isShowDistance = false;
        //Used to check if the target has been hit
        public bool isHit = false;

        [Header("Customizable Options")]
        //Minimum time before the target goes back up
        public float minTime;
        //Maximum time before the target goes back up
        public float maxTime;

        [Header("Audio")]
        public AudioClip upSound;
        public AudioClip downSound;

        public AudioSource audioSource;

        private void Awake()
        {
            IC = this;
        }

        private void Update()
        {

            //Generate random time based on min and max time values
            randomTime = Random.Range(minTime, maxTime);

            //If the target is hit
            if (isHit == true)
            {
                if (!isShowDistance)
                {
                    float Dis = Vector3.Distance(transform.position, FindObjectOfType<PlayerController>().transform.position);
                    Debug.Log("击中！距离为：" + Dis);
                    IC.TriggerEvent("HitEnemy");
                    IC.OpenPanel<MessagePanel>(true,$"Hit!Distance:{Dis}");
                    isShowDistance = true;
                }
                //Debug.Log("击中！距离为：" + Vector3.Distance(transform.position, FindObjectOfType<PlayerController>().transform.position));
                if (routineStarted == false)
                {
                    //Animate the target "down"
                    gameObject.GetComponent<Animation>().Play("A_Target_Down");

                    //Set the downSound as current sound, and play it
                    audioSource.GetComponent<AudioSource>().clip = downSound;
                    audioSource.Play();

                    //Start the timer
                    StartCoroutine(DelayTimer());
                    routineStarted = true;
                }
            }
        }

        //Time before the target pops back up
        private IEnumerator DelayTimer()
        {
            //Wait for random amount of time
            yield return new WaitForSeconds(randomTime);
            //Animate the target "up" 
            gameObject.GetComponent<Animation>().Play("A_Target_Up");

            //Set the upSound as current sound, and play it
            audioSource.GetComponent<AudioSource>().clip = upSound;
            audioSource.Play();

            //Target is no longer hit
            isHit = false;
            routineStarted = false;
            isShowDistance = false;
        }
    }
}