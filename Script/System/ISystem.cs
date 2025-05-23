
using FPS;
using System;
using UnityEngine;

namespace FPS_Manager
{
    public interface ISystem
    {
        /// <summary>
        /// 播放音效
        /// </summary>
        /// <param name="name">名字</param>
        /// <param name="volume">音量(可选)</param>
        /// <param name="priority">优先级(可选)</param>
        public void PlaySFX(string name, GameObject obj=null, AudioClipType type = AudioClipType.Normal, float volume = 1.0f, int priority = 128)
        {
            GameManager.Instance.AudioSystem.PlaySFX(name, obj, volume, priority, type);
        }
        /// <summary>
        /// 访问ScriptableObject数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T GetData<T>() where T : ScriptableObject
        {
            return GameManager.Instance.ModelManager.GetData<T>();
        }
        /// <summary>
        /// 访问Model层，但是不能赋值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T GetModel<T>() where T : Imodel
        {
            return GameManager.Instance.ModelManager.GetModel<T>();
        }
        /// <summary>
        /// 注册事件
        /// </summary>
        /// <param name="name">事件名</param>
        public void RegisterEvent(string name)
        {
            GameManager.Instance.EventSystem.RegisterEvent(name);

        }
        /// <summary>
        /// 监听事件
        /// </summary>
        /// <param name="name">事件名</param>
        /// <param name="lisenter">Action(Object)</param>
        public void StartListening(string name, Action<object> lisenter)
        {
            GameManager.Instance.EventSystem.StartListening(name, lisenter);
        }
        /// <summary>
        /// 停止监听某事件
        /// </summary>
        /// <param name="name">事件名</param>
        /// <param name="listener">Action(Object)</param>
        public void StopListening(string name, Action<object> listener)
        {
            GameManager.Instance.EventSystem.StopListening(name, listener);
        }
        /// <summary>
        /// 触发某事件
        /// </summary>
        /// <param name="name">事件名</param>
        /// <param name="obj">可以带数据</param>
        public void TriggerEvent(string name, object obj = null)
        {
            GameManager.Instance.EventSystem.TriggerEvent(name, obj);
        }
        /// <summary>
        /// 注销某事件
        /// </summary>
        /// <param name="name">事件名</param>
        public void UnRegisterEvent(string name)
        {
            GameManager.Instance.EventSystem.UnRegisterEvent(name);
        }
        /// <summary>
        /// 注册对象池
        /// </summary>
        /// <param name="id">名字</param>
        /// <param name="path">加载路径</param>
        public void RegisterObjectPool(string id, string path)
        {
            GameManager.Instance.objectPoolSystem.RegisterObjectPool(id, path);
        }
        /// <summary>
        /// 获得对象
        /// </summary>
        /// <param name="id">名字</param>
        /// <returns></returns>
        public GameObject GetObject(string id)
        {
            return GameManager.Instance.objectPoolSystem.GetObject(id);
        }
        /// <summary>
        /// 获得对象的重载版本
        /// </summary>
        /// <param name="id">名字</param>
        /// <param name="pos">位置</param>
        /// <param name="quaternion">旋转</param>
        /// <returns></returns>
        public GameObject GetObject(string id, Vector3 pos, Quaternion quaternion)
        {
            return GameManager.Instance.objectPoolSystem.GetObject(id, pos, quaternion);
        }

        /// <summary>
        /// 回收对象
        /// </summary>
        /// <param name="id">名字</param>
        /// <param name="obj">物体</param>
        public void ReturnObject(string id, GameObject obj)
        {
            GameManager.Instance.objectPoolSystem.ReturnObject(id, obj);
        }
        /// <summary>
        /// 关闭所有panel
        /// </summary>
        public void CloseAllPanel()
        {
            GameManager.Instance.UIManager.CloseAllPanel();
        }
        /// <summary>
        /// 关闭当前面板
        /// </summary>
        public void ClosePanel()
        {
            GameManager.Instance.UIManager.ClosePanel();
        }
        /// <summary>
        /// 打开绵绵
        /// </summary>
        /// <typeparam name="T">BaseUIPanel类</typeparam>
        /// <param name="isPopup">是否是弹窗，如果是弹窗就不进入栈</param>
        public void OpenPanel<T>(bool isPopup = false, string message = null) where T : BaseUIPanel
        {
            GameManager.Instance.UIManager.OpenPanel<T>(isPopup, message);
        }
       /// <summary>
       /// 背景音乐
       /// </summary>
       /// <param name="name">名字</param>
       /// <param name="loop">循环</param>
       /// <param name="type">类型</param>
        public void PlayerBGMusic(string name, bool loop = true, AudioClipType type = AudioClipType.Normal)
        {
            GameManager.Instance.AudioSystem.PlayerBGMusic(name, loop, type);
        }
        /// <summary>
        /// 停止播放背景音乐
        /// </summary>
        public void StopBGMusic()
        {
            GameManager.Instance.AudioSystem.StopBGMusic();
        }
        /// <summary>
        /// 加载场景时清空对象池
        /// </summary>
        public void RemoveObjectPoolGameObject()
        {
            GameManager.Instance.objectPoolSystem.RemoveObjectPoolGameObject();
        }

    }
}

