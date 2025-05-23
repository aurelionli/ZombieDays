using FPS_Manager;
using UnityEngine;

namespace FPS
{
    public class DecreaseGeThrowCommand : Command
    {
        protected override void OnExecute(object data = null)
        {
            if (IC.GetModel<CurrentWeaponModel>().throwNum <=0)
            {
                Debug.LogError("扔雷出错！");
                return;
            }
            IC.GetModel<CurrentWeaponModel>().throwNum--;
            IC.TriggerEvent("ThrowGe");
        }
    }
}
