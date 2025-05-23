using FPS_Manager;
using UnityEngine;
namespace FPS
{

    public struct SwitchWeaponCommandData
    {
        public WeaponType Type;
        public int weaponId;
    }
    //这个是切换枪
    public class SwitchWeaponCommand : Command
    {
        protected override void OnExecute(object data = null)
        {
            SwitchWeaponCommandData type = (SwitchWeaponCommandData)data;
            IC.GetModel<CurrentWeaponModel>().currentWeapon = type.Type;
            IC.GetModel<CurrentWeaponModel>().currentWeaponId =type.weaponId;
            Debug.Log("SwitchWeaponCommand:" + type+ "SwitchWeapon事件触发");
            IC.TriggerEvent("SwitchWeapon");
        }
    }
}
