using System;
using System.Collections.Generic;
using UnityEngine;


namespace FPS
{
    public abstract class BaseBuff:IController
    {
        protected IController IC;

        public float duration; //持续时间，-1表示永久
        public float elapsedTime; //buff 已持续时间
        public float count;     //触发计数
        public float tickInterval;//触发间隔
        public bool isKeepWorking;//是不是持续性buff


        public BaseBuff()
        {
            IC = this;
       
            elapsedTime = 0f;
           
        }



        public abstract void OnApply();//buff 生效的时候

        public abstract void OnRemove();//buff 移除的时候

        public abstract void OnTick();//持续触发的逻辑
       
    }

}