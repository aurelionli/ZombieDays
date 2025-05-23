

using UnityEngine;

namespace FPS
{
    public abstract class BaseInteractController:MonoBehaviour,IController
    {
        protected IController IC;
        public Transform Interactable;
        //如果父类和子类都定义了Awake方法，并且子类没有显式调用父类的Awake方法，则只有子类的Awake会被执行。
        protected virtual void Awake()
        {
            IC = this;
            Interactable = transform.GetChild(0);
            Interactable.gameObject.SetActive(false);
        }
        public virtual void BeSelect()
        {
            //  if (Interactable.gameObject.activeSelf) { return; }
            Interactable.gameObject.SetActive(true);
        }
        public virtual void UnSelect()
        {
            Interactable.gameObject.SetActive(false);
        }
        public abstract void BePickUp();
        
    }
}
