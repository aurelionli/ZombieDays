using FPS_Manager;

namespace FPS
{
    
    public class ReloadShotGunAmmoCommand : Command
    {
        //注册枪，首先需要武器类型,和一梭子多少
        protected override void OnExecute(object data = null)
        {

            IC.GetModel<CurrentWeaponModel>().weaponBulletMagDic[IC.GetModel<CurrentWeaponModel>().currentWeaponId]++;
            IC.GetModel<CurrentWeaponModel>().weaponBulletNumDic[IC.GetModel<CurrentWeaponModel>().currentWeapon]--;
            IC.TriggerEvent("WeaponAmmo");



        }
    }
}
