using System;

namespace FPS_Manager
{
    //1.注册事件，激活事件
    public interface Imodel
    {
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
    }
}
