

using UnityEngine;

namespace FPS { 
    public class EnemyAppearController :MonoBehaviour,IController
    {
        private IController IC;

       


        private void Awake()
        {
            IC = this;
         
        }
        private void Start()
        {
          
        }
        private void OnTriggerEnter(Collider other)
        {
            if(other.tag=="Player")
            {
                IC.OpenPanel<GuidePanel>(true);
                IC.OpenPanel<TaskPanel>();
                IC.TriggerEvent("AppearEnemy");
               
                
                gameObject.GetComponent<Collider>().enabled = false;
            
            }
        }
    }
}
