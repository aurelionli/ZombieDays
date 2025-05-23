using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FPS
{
    public class LapTopController : BaseInteractController,IController
    {

        public bool isOpen;
        public float time;

      
        private void Start()
        {

           
        
        }

        private void Update()
        {
  
        }
        IEnumerator BeginHorde()
        {
            IC.TriggerEvent("HordeStart");
            IC.OpenPanel<MessagePanel>(true,"HordeStart!");
            yield return new  WaitForSeconds(60f);
            IC.OpenPanel<MessagePanel>(true, "Run!");
            IC.TriggerEvent("GuideTarget", GameObject.Find("RunToTarget").transform);
          //  IC.TriggerEvent("HordeEnd");
            IC.TriggerEvent("RunToEvacuation");
        }
        public override void BePickUp()
        {
            if(isOpen)
            {
                return;
            }
            isOpen = true;
            time = Time.time;
            //todo：触发尸潮,坚持一段时间往一个地点撤离。
            StartCoroutine(BeginHorde());


        }

      
       
    }
}