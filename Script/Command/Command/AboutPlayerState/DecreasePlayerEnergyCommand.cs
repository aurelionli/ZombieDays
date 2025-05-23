using FPS_Manager;

namespace FPS
{

    //完全回复状态，并且加一点血量上限/
    public class DecreasePlayerEnergyCommand : Command
    {
        protected override void OnExecute(object data = null)
        {
            float i = (float)data;
            PlayerCurrentStateModel temp = IC.GetModel<PlayerCurrentStateModel>();
            temp.currentEnergy -= i;
          /*  if (temp.currentEnergy <= 0)
            {
                temp.currentEnergy = 0;
            }*/
            IC.TriggerEvent("PlayerEnergyChange");

        }
    }
}
