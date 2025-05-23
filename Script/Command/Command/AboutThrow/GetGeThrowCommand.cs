using FPS_Manager;

namespace FPS
{
    public class GetGeThrowCommand : Command
    {
        protected override void OnExecute(object data = null)
        {

            if (IC.GetModel<CurrentWeaponModel>().throwNum >= 5)
            {
                return;
            }

            IC.GetModel<CurrentWeaponModel>().throwNum++;
            IC.TriggerEvent("ThrowGe");


        }
    }
}
