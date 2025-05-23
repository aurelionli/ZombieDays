using FPS_Manager;
namespace FPS
{
    public class WeaponShootCommand : Command
    {
        protected override void OnExecute(object data = null)
        {

            CurrentWeaponModel temp = IC.GetModel<CurrentWeaponModel>();
           
            temp.weaponBulletMagDic[temp.currentWeaponId]--;
            IC.TriggerEvent("WeaponAmmo");


           
        }
    }
}