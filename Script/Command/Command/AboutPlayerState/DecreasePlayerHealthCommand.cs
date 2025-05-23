using FPS_Manager;

using UnityEngine;

namespace FPS
{

    //完全回复状态，并且加一点血量上限/
    public class DecreasePlayerHealthCommand : Command
    {
        protected override void OnExecute(object data = null)
        {
            int i = (int)data;
            IC.GetModel<PlayerCurrentStateModel>().currentHealth -= i;
            Debug.Log(IC.GetModel<PlayerCurrentStateModel>().currentHealth);
            IC.TriggerEvent("PlayerHealthChange");

            if(IC.GetModel<PlayerCurrentStateModel>().currentHealth<=0)
            {
              
                IC.OpenPanel<DeadPanel>();

                //todo:停止僵尸一切行动，

                IC.TriggerEvent("PlayerDead");
                Debug.Log("玩家死亡");
            }


        }
    }
}
