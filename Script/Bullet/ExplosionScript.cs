﻿using UnityEngine;
using System.Collections;

namespace FPS
{
    //  IC.RegisterObjectPool("Explosion", "Object/Effect/Explosion Prefab");

    public class ExplosionScript : MonoBehaviour,IController
	{
		private IController IC;

		[Header("Customizable Options")]
		//How long before the explosion prefab is destroyed
		public float despawnTime = 10.0f;
		//How long the light flash is visible
		public float lightDuration = 0.02f;
		[Header("Light")]
		public Light lightFlash;

		[Header("Audio")]
		public AudioClip[] explosionSounds;
		public AudioSource audioSource;

        private void Awake()
        {
            IC = this;
        }
        private void OnEnable()
		{
            
            //Start the coroutines
            StartCoroutine(DestroyTimer());
			StartCoroutine(LightFlash());

			//Get a random impact sound from the array
			audioSource.clip = explosionSounds
				[Random.Range(0, explosionSounds.Length)];
			//Play the random explosion sound
			audioSource.Play();
		}

		private IEnumerator LightFlash()
		{
			//Show the light
			lightFlash.GetComponent<Light>().enabled = true;
			//Wait for set amount of time
			yield return new WaitForSeconds(lightDuration);
			//Hide the light
			lightFlash.GetComponent<Light>().enabled = false;
		}

		private IEnumerator DestroyTimer()
		{
			//Destroy the explosion prefab after set amount of seconds
			yield return new WaitForSeconds(despawnTime);
			IC.ReturnObject("Explosion", gameObject);//Destroy(gameObject);
		}
        private void OnDisable()
        {
            StopAllCoroutines();
        }
    }
}