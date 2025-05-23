
using System;
using System.Collections.Generic;
using UnityEngine;

namespace FPS_Manager
{
    public class ModelManager
    {
        private readonly Dictionary<string, Imodel> dirModel = new Dictionary<string, Imodel>();

       // private readonly Dictionary<string, ScriptableObject> dirData = new Dictionary<string, ScriptableObject>();
        //   private readonly Dictionary<Type, Imodel> dird = new Dictionary<Type, Imodel>();

   
        public ModelManager()
        {
            InitModel();
           
        }

        private void InitModel()
        {
            dirModel.Add("CurrentWeaponModel", new CurrentWeaponModel().InitEvent());
            dirModel.Add("PlayerCurrentStateModel", new PlayerCurrentStateModel().InitEvent());
            dirModel.Add("MusicModel", new MusicModel().InitEvent());
            //   dir.Add("PlayerStatesModel", new PlayerStatesModel());
            /*  dir.Add(typeof(PlayerCurrentStatesModel).Name, LoadModel<PlayerCurrentStatesModel>());
              dir.Add(typeof(TotalSceneModel).Name, LoadModel<TotalSceneModel>());
              dir.Add(typeof(PlayerNormalModel).Name, LoadModel<PlayerNormalModel>());
              dir.Add(typeof(CurrentBossModel).Name, new CurrentBossModel());*/
        }

        public T LoadModel<T>() where T : class, Imodel,new()
        {
            //todo 此处使用存档工具
            T model = SaveTool.LoadData<T>();
            if (model == null)
            {
                model = new T();
            }
            return model;
        }
        public T LoadModels<T>() where T :  BaseModel, new()
        {
            //todo 此处使用存档工具
            T model = SaveTool.LoadData<T>();
            if (model == null)
            {
                model = new T();
            }
            return model;
        }
        /* public Imodel GetModel(Imodel model)
         {
             Imodel imodel;
             if(dir.TryGetValue(model.ToString(),out imodel))
             {
                 Debug.Log($"找到{model.ToString()}");
                 return imodel;
             }
             Debug.LogError($"没有注册{model.ToString()}");
             return null;

         }*/
        public T GetModels<T>() where T : BaseModel,new() 
        {
            
            if (dirModel.ContainsKey(typeof(T).Name))
            {
                return (T)dirModel[typeof(T).Name];
            }
            else
            {
                var data = LoadModel<T>().InitModel();
                dirModel.Add(typeof(T).Name, data);
                return (T)dirModel[typeof(T).Name];
            }
            Debug.LogError($"没有注册{typeof(T).ToString()}");
            return default;
        }
        public T GetModel<T>() where T : Imodel
        {
            if (dirModel.ContainsKey(typeof(T).Name))
            {
                return (T)dirModel[typeof(T).Name];
            }
            Debug.LogError($"没有注册{typeof(T).ToString()}");
            return default;
        }
        /// <summary>
        /// 加载ScriptableObject数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T GetData<T> ()where T : ScriptableObject
        {
           // if(dirData.ContainsKey(typeof(T).Name))
           var data = Resources.Load<T>($"Data/{typeof(T).Name}");
            Debug.Log(data.name);
            if(data == null )
            {
                Debug.LogError($"没有数据data{typeof(T).ToString()}");
                return default;
            }
            return data;



        }
    }
}
