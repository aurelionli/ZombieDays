using FPS_Manager;


namespace FPS
{
    public class ReloadAmmoCommand : Command
    {
        protected override void OnExecute(object data = null)
        {
            //希望传过来的是什么？一梭子多少发
            int bullet = (int)data;
            CurrentWeaponModel temp = IC.GetModel<CurrentWeaponModel>();
            //1.获得需要的子弹数量= 
           
            int needBullet = bullet - temp.weaponBulletMagDic[temp.currentWeaponId];
           /* Debug.Log($"当前武器类型为{temp.currentWeapon}");
            Debug.Log($"一梭子{bullet}发，所以需要{bullet}-{temp.weaponsDir[temp.currentWeapon].bulletMag}={needBullet}发");*/
            //2.如果备弹不够了
            if (needBullet > temp.weaponBulletNumDic[temp.currentWeapon])
            {
                //2.1 当前子弹+上当前备弹
                //当前备弹减去自己
                temp.weaponBulletMagDic[temp.currentWeaponId] += temp.weaponBulletNumDic[temp.currentWeapon];
                temp.weaponBulletNumDic[temp.currentWeapon] -= temp.weaponBulletNumDic[temp.currentWeapon];
            }
            //3.如果子弹够
            else
            {
                //3.1
                temp.weaponBulletMagDic[temp.currentWeaponId] += needBullet;
                temp.weaponBulletNumDic[temp.currentWeapon] -= needBullet;
            }



            IC.TriggerEvent("WeaponAmmo");
        }
    }
}
