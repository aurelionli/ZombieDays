using FPS;
using UnityEngine;

namespace FPS_Manager
{
    public interface ICommand
    {
        //1.可以改值，触发事件
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
        /// 访问ScriptableObject数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T GetData<T>() where T : ScriptableObject
        {
            return GameManager.Instance.ModelManager.GetData<T>();
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
        /// 打开绵绵
        /// </summary>
        /// <typeparam name="T">BaseUIPanel类</typeparam>
        /// <param name="isPopup">是否是弹窗，如果是弹窗就不进入栈</param>
        public void OpenPanel<T>(bool isPopup = false, string message = null) where T : BaseUIPanel
        {
            GameManager.Instance.UIManager.OpenPanel<T>(isPopup, message);
        }

    }
}
