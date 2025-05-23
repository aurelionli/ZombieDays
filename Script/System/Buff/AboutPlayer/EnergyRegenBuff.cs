
using FPS_Manager;
using System;
using UnityEngine;

namespace FPS
{
    internal class EnergyRegenBuff : BaseBuff
    {
        public float energyAmount = 5f;
        public float coolDown; //奔跑完后一秒才能回复体力
        public EnergyRegenBuff() : base()
        {
            //而这里设置是不是永久buff，还有触发间隔。
            tickInterval = 0.05f;
            isKeepWorking = true;
        }

        public override void OnApply()
        {
           
        }

        public override void OnRemove()
        {

        }

        public override void OnTick()
        {
            if(IC.GetModel<PlayerCurrentStateModel>().playerCurrentState==MovementState.RUN)
            {
                coolDown = Time.time;
                return;
            }
            if(coolDown+1f>Time.time)
            {
                return;
            }
            //Debug.Log("玩家恢复体力");
            if (IC.GetModel<PlayerCurrentStateModel>().currentEnergy == IC.GetModel<PlayerCurrentStateModel>().playerEnergy)
            {
                //Debug.Log("玩家体力已满");
                return;
            }
  
           IC.SendCommand<AddPlayerEnergyCommand>(energyAmount);

        }
    }
}
