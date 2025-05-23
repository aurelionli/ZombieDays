using System.Collections.Generic;
using UnityEngine;
using FPS;
namespace FPS_Manager
{
    public class ObjectPoolSystem
    {
        public readonly Dictionary<string, ObjectPool> pools = new Dictionary<string, ObjectPool>();

        public void RemoveObjectPoolGameObject()
        {
            foreach(ObjectPool pool in pools.Values)
            {
                pool.ClearPool();
            }
        }

        public void RegisterObjectPool(string id,string path)
        {
            
            if(pools.ContainsKey(id))
            {
                Debug.LogWarning("已经有该对象池");
                return;
            }
            Debug.Log($"对象池{id}创建");
            pools.Add(id, new ObjectPool(path));
        }

        public GameObject GetObject(string id)
        {
            if(pools.ContainsKey(id))
            {
                return pools[id].GetObject();
            }
            Debug.LogError($"没有{id}对象池,无法获得");
            return null;
        }
        public GameObject GetObject(string id,Vector3 pos,Quaternion quaternion)
        {
            if (pools.ContainsKey(id))
            {
                GameObject temp =  pools[id].GetObject(pos,quaternion);
             
                return temp;
            }
            Debug.LogError($"没有{id}对象池,无法获得");
            return null;
        }
        public void ReturnObject(string id,GameObject obj)
        {
            if(pools.ContainsKey(id))
            {
              
                pools[id].ReturnObject(obj);
                return;
            }
            Debug.LogError($"没有{id}对象池,无法回收");
            
        }
    }
}
