using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FPS;
using System;
namespace FPS_Manager

{
    public class BuffSystem : MonoBehaviour
    {
       // private readonly List<BaseBuff> activeBuffs = new List<BaseBuff>();
        
        private readonly Dictionary<Type,BaseBuff> activeBuffs = new Dictionary<Type,BaseBuff>();
        private void Awake()
        {


        }
        private void Start()
        {

            AddBuff<EnergyRegenBuff>(-1);
        }
        /* 持续时间，触发间隔 ，是不是持续性buff  */
      /*  public T CreateBuff<T>(float duration) where T : BaseBuff , new()
        {
            var buff = new T();
            buff.duration = duration;
            return buff;
        }*/
        public void AddBuff<T>(float duration) where T : BaseBuff,new()
        {
            Type t = typeof(T);
            if(activeBuffs.ContainsKey(t)){
                activeBuffs[t].elapsedTime = 0;
                activeBuffs[t].OnApply();
            }
            else{
                var buff = new T();
                buff.duration = duration;
                buff.OnApply();
                activeBuffs.Add(t, buff);
            }
        }

        
        public void RemoveBuff(Type t)
        {
            if (activeBuffs.ContainsKey(t))
            {

                activeBuffs.Remove(t);
            }
        }

        private void Update()
        {
            foreach (var t in activeBuffs)
            {
 
                if (t.Value.isKeepWorking)  //如果是持续性buff。
                {
                    t.Value.count += Time.deltaTime;
                    if (t.Value.count > t.Value.tickInterval)
                    {
                        t.Value.OnTick();
                        t.Value.count = 0f;
                    }
                }
                t.Value.elapsedTime += Time.deltaTime;
                if (t.Value.duration > 0f && t.Value.elapsedTime >= t.Value.duration)//如果持续时间不是-1永久
                {
                    t.Value.OnRemove();
                    activeBuffs.Remove(t.Key);
                    return;
                }
            }
        }
    }
}
