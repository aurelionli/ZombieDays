using FPS_Manager;


namespace FPS
{
    public class GetTheSameWeaponCommand : Command
    {
        protected override void OnExecute(object data = null)
        {
            IC.GetModel<CurrentWeaponModel>().weaponBulletNumDic[IC.GetModel<CurrentWeaponModel>().currentWeapon] += 30;

            if (IC.GetModel<CurrentWeaponModel>().weaponBulletNumDic[IC.GetModel<CurrentWeaponModel>().currentWeapon] > 300)
            {
                IC.GetModel<CurrentWeaponModel>().weaponBulletNumDic[IC.GetModel<CurrentWeaponModel>().currentWeapon] = 300;
            }

            IC.TriggerEvent("WeaponAmmo");
        }
    }
}
