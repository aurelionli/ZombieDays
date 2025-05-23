using FPS_Manager;

using UnityEngine;

namespace FPS
{
    public struct RegisterWeaponCommandData
    {
        public WeaponType weaponType;
        public int bulletMag;
        public int weaponId;
    }
    public class RegisterWeaponCommand : Command
    {
        //注册枪，首先需要武器类型,和一梭子多少
        protected override void OnExecute(object data = null)
        {
            RegisterWeaponCommandData d = (RegisterWeaponCommandData)data;
            Debug.Log($"注册的武器是{d.weaponType}，ID是{d.weaponId}弹匣为{d.bulletMag}");
            CurrentWeaponModel temp = IC.GetModel<CurrentWeaponModel>();
            
            if(temp.weaponBulletMagDic.ContainsKey(d.weaponId))//就是三把枪换完后，又换回之前出现过的枪的bug
            {
                temp.weaponBulletMagDic[d.weaponId] = d.bulletMag;
                temp.weaponBulletNumDic[d.weaponType] += 50;
            }
            else
            {
                temp.weaponBulletMagDic.Add(d.weaponId, d.bulletMag);
                temp.weaponBulletNumDic[d.weaponType] += 50;
            }
            if(temp.weaponBulletNumDic[d.weaponType]>300)
            {
                temp.weaponBulletNumDic[d.weaponType] = 300;
            }
            IC.TriggerEvent("WeaponAmmo");






        }
    }
}
