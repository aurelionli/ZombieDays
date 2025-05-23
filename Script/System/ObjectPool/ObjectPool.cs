using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool 
{
    private GameObject prefab;
   // private int initialSize = 10; //初始大小

    private Queue<GameObject> pool = new Queue<GameObject>();

    private int index=0;

    private Transform parent;

    private string path;
    public ObjectPool(string path)
    {
        this.path = path;
        parent = new GameObject(path).transform;
        prefab = Resources.Load<GameObject>(path);
        if( prefab == null )
        {
            Debug.LogError($"{path}路径没有物体！");
            return;
        }
       /* for (int i = 0; i < initialSize; i++)
        {
            GameObject obj = GameObject.Instantiate(prefab, parent);
            obj.transform.position = Vector3.zero;
            obj.SetActive(false);
            obj.name = $"{obj.name}{index}";
            index++;
            pool.Enqueue(obj);
        }*/
    }

    public void ClearPool()
    {
        index = 0; pool.Clear();parent = null;
    }
    public GameObject GetObject()
    {
        if(parent == null)
        {
            parent = new GameObject(path).transform;
        }
        if (pool.Count > 0)
        {
            GameObject obj = pool.Dequeue();
            obj.SetActive(true);
            return obj;
        }
        else
        {
            // 如果池中没有闲置物体，创建一个新的
            GameObject obj = GameObject.Instantiate(prefab, parent);
            obj.name = $"{obj.name}{index}";
            index++;
            return obj;
        }
    }
    public GameObject GetObject(Vector3 pos,Quaternion quaternion)
    {
        if (parent == null)
        {
            parent = new GameObject(path).transform;
        }
        if (pool.Count > 0)
        {
            GameObject obj = pool.Dequeue();
            obj.transform.position = pos;
            obj.transform.rotation = quaternion;
            if (obj.GetComponent<TrailRenderer>()!=null)
            {
                obj.GetComponent<TrailRenderer>().Clear();
            }
            obj.SetActive(true);
            return obj;
        }
        else
        {
            // 如果池中没有闲置物体，创建一个新的
            GameObject obj = GameObject.Instantiate(prefab, pos, quaternion, parent);
            obj.name = $"{obj.name}{index}";
            index++;
            return obj;
        }
    }

    public void ReturnObject(GameObject obj)
    {
        obj.SetActive(false);
        pool.Enqueue(obj);
    }


}
