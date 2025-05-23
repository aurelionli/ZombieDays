using UnityEngine;
using System.Collections;
namespace FPS
{
	public class ImpactScript : MonoBehaviour,IController
	{
		private IController IC;
        public enum ImpactType { Blood,Concrete,Dirt,Metal}

        public ImpactType type;

		[Header("摧毁时间")]

		public float despawnTimer = 2.0f;

		[Header("Audio")]
		public AudioClip[] impactSounds;
		public AudioSource audioSource;

        private void Awake()
        {
            IC = this;
        }
        private void OnEnable() 
        {

           
            StartCoroutine(DespawnTimer());


			audioSource.clip = impactSounds[Random.Range(0, impactSounds.Length)];

			audioSource.Play();
		}

		private IEnumerator DespawnTimer() {

			yield return new WaitForSeconds(despawnTimer);

			ReturnGameObject();//Destroy(gameObject);
		}
        /*IC.RegisterObjectPool("Blood", "Object/Effect/Blood Impact Prefab");
            IC.RegisterObjectPool("Concrete", "Object/Effect/Concrete Impact Prefab");
            IC.RegisterObjectPool("Dirt", "Object/Effect/Dirt Impact Prefab");
            IC.RegisterObjectPool("Metal", "Object/Effect/Metal Impact Prefab");
            IC.RegisterObjectPool("Explosion", "Object/Effect/Explosion Prefab");*/
        private void ReturnGameObject()
		{
          
            switch ( type)
			{
				case ImpactType.Blood:
					IC.ReturnObject("Blood",gameObject);
                
                    break;
                case ImpactType.Concrete:
                    IC.ReturnObject("Concrete", gameObject);
                    break;
                case ImpactType.Dirt:
                    IC.ReturnObject("Dirt", gameObject);
                    break;
                case ImpactType.Metal:
                    IC.ReturnObject("Metal", gameObject);
                    break;
                default:
                    Debug.LogError("回收失败！");
                    break;

            }



		}
      /*  private void OnDisable()
        {
            StopAllCoroutines();
        }*/
    }
}
