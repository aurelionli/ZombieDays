using FPS_Manager;


namespace FPS
{
    public struct GetBulletCommandData
    {
        public WeaponType type;
        public int num;
    }
    public class GetBulletCommand : Command
    {
        protected override void OnExecute(object data = null)
        {
            GetBulletCommandData temp = (GetBulletCommandData)data;

            IC.GetModel<CurrentWeaponModel>().weaponBulletNumDic[temp.type] += temp.num;
            IC.TriggerEvent("WeaponAmmo");

        }
    }
}
