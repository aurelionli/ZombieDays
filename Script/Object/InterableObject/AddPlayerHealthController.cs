using System.Collections;
using UnityEngine;

namespace FPS
{
    public class AddPlayerHealthController : BaseInteractController
    {
      
        public bool isOpen;
        public int health;


        private void OnEnable()
        {
            isOpen = false;
          
          
        }


        public override void BePickUp()
        {
            if (isOpen)
            {
                return;
            }
            isOpen = true;

            //发送
            IC.SendCommand<AddPlayerHealthCommand>(health);
            IC.OpenPanel<MessagePanel>(true, "Player Health Up");
            Destroy(gameObject);
        }
        
    }
}
