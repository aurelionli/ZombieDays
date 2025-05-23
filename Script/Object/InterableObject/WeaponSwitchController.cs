
using UnityEngine;

namespace FPS
{
    public interface Interactable
    {
  


    }
    public class WeaponSwitchController : BaseInteractController
    {
     
       

        public int Id;

        protected override void Awake()
        {
            base.Awake();
        }

        /* public void BeSelect()
         {
           //  if (Interactable.gameObject.activeSelf) { return; }
             Interactable.gameObject.SetActive(true);
         }
         public void UnSelect()
         {
             Interactable.gameObject.SetActive(false);
         }*/


        public override void BePickUp()
        {
            IC.TriggerEvent("PickUpWeapon", Id);
            IC.OpenPanel<MessagePanel>(true, " Get Weapon");
        }
    }
}
