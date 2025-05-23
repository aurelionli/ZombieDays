using FPS_Manager;

namespace FPS
{
  
    //完全回复状态，并且加一点血量上限/
    public class AddPlayerHealthCommand : Command
    {
        protected override void OnExecute(object data = null)
        {
            int i = (int)data;
            IC.GetModel<PlayerCurrentStateModel>().playerHealth += i;
            IC.GetModel<PlayerCurrentStateModel>().currentHealth = IC.GetModel<PlayerCurrentStateModel>().playerHealth;
            IC.TriggerEvent("PlayerHealthChange");
            

        }
    }
}
