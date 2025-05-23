



using System.Collections;
using UnityEngine;

namespace FPS
{
    public class GetCasingBulletController : BaseInteractController
    {
        public WeaponType Type;
        public bool isOpen;
        public int num;

        private void OnEnable()
        {
            isOpen = false;


        }


        public override void BePickUp()
        {
            if(isOpen)
            {
                return;
            }
            isOpen = true;

            //发送
            GetBulletCommandData temp = new GetBulletCommandData();
            temp.type = Type;
            temp.num = num;
            IC.SendCommand<GetBulletCommand>(temp);
            IC.OpenPanel<MessagePanel>(true, $"Player Get {Type}Ammo");
            Destroy(gameObject);
        }
    }
}
