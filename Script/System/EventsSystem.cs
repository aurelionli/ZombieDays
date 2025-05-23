using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace FPS_Manager
{
    public class EventsSystem
    {
        private Dictionary<string, Action<object>> eventDic = new Dictionary<string, Action<object>>();

        private Dictionary<string, Delegate> eventDicc = new Dictionary<string, Delegate>();
        public void StartListening<T>(string name, Action<T> listener)
        {
            if (!eventDicc.ContainsKey(name))
            {
                eventDicc[name] = null;
            }
            //eventDicc[name] +=  listener;
            eventDicc[name] = (Action<T>)eventDicc[name] + listener;
        }

        public void StopListening<T>(string name, Action<T> listener)
        {
            if (eventDicc.ContainsKey(name))
            {
                eventDicc[name] = (Action<T>)eventDicc[name] - listener;
                if (eventDicc[name] == null)
                {
                    eventDicc.Remove(name);
                }
            }
        }

        public void TriggerEvent<T>(string name, T obj)
        {
            if (eventDicc.ContainsKey(name))
            {
                //((Action<T>)eventDicc[name])?.Invoke(obj);
                ((Action<T>)eventDicc[name])?.Invoke(obj);
            }
            else
            {
                Debug.LogWarning($"没有监听{name}事件");
            }
        }
    

        public void RegisterEvent(string name)
        {
            if (eventDic.ContainsKey(name))
            {
                Debug.LogError($"已有{name}事件");
                return;
            }
            eventDic.Add(name, null);
        }
        public void StartListening(string name, Action<object> lisenter)
        {
            if (eventDic.ContainsKey(name))
            {
                eventDic[name] += lisenter;
            }
            else
            {
                Debug.LogError($"没有注册{name}事件");
            }
        }

        public void StopListening(string name, Action<object> listener)
        {
            if (eventDic.ContainsKey(name))
            {
                eventDic[name] -= listener;
            }
        }

        public void TriggerEvent(string name, object obj = null)
        {
            if (eventDic.ContainsKey(name))
            {
                eventDic[name]?.Invoke(obj);
            }
        }

        public void UnRegisterEvent(string name)
        {
            if (eventDic.ContainsKey(name))
            {
                eventDic.Remove(name);
            }
            else
            {
                Debug.LogError($"没有找到{name}事件");
            }
        }
    }
}
