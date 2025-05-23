using System;
using System.Reflection;
using UnityEngine;

    public static class GameTools
    {
        public static T GetSingleComponentInChild<T>(GameObject obj, string ComponentName) where T : Component
        {
            Transform[] transforms = obj.GetComponentsInChildren<Transform>();
            foreach (Transform tra in transforms)
            {
                if (tra.gameObject.name == ComponentName)
                {
                    return tra.gameObject.GetComponent<T>();

                }
            }

            Debug.LogWarning($"没有在{obj.name}中找到{ComponentName}物体！");
            return null;


        }
        /*
        type.GetProperties(...)：调用 Type 对象的 GetProperties 方法，
        获取该类型的所有公共实例属性（BindingFlags.Public | BindingFlags.Instance）。
        BindingFlags.Public：只获取公共属性。
        BindingFlags.Instance：只获取实例属性（非静态属性）。
        properties：存储所有符合条件的 PropertyInfo 对象数组。每个 PropertyInfo 对象包含有关单个属性的信息
        property.CanRead：检查属性是否有公共的 get 访问器（即是否可以读取）。
        property.CanWrite：检查属性是否有公共的 set 访问器（即是否可以写入）。
        if：确保只有既可读又可写的属性才会被处理。
         */
        /// <summary>
        /// 类的浅拷贝，只有公共属性
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source">赋值者</param>
        /// <param name="target">被赋值者</param>
        public static void CopyProperties<T>(T source, T target)
        {
            Type type = typeof(T);
            PropertyInfo[] properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (PropertyInfo property in properties)
            {
                if (property.CanRead && property.CanWrite)
                {
                    object value = property.GetValue(source);
                    property.SetValue(target, value);
                }
            }
        }
    }





