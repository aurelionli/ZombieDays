using System;

using UnityEngine;
namespace FPS_Manager
{
    public abstract class Command : IDisposable, ICommand
    {
        protected ICommand IC;
        private DateTime lastUseTime; //最后使用的时间
        public TimeSpan TTL { get; set; } = TimeSpan.FromMinutes(5);

        public bool IsExpired => DateTime.UtcNow - lastUseTime > TTL;
        protected Command()
        {
            IC =this;
            RefreshTTL();
        }
        public void Execute(object data = null)
        {
            OnExecute(data);
            RefreshTTL();
        }

        protected abstract void OnExecute(object data = null);

        public void RefreshTTL()
        {
            lastUseTime = DateTime.UtcNow;
        }
        public void Dispose()
        {
            OnDispose();
        }
        protected virtual void OnDispose()
        {
            //如果需要立即回收，这里可以留着取消订阅事件
            Debug.Log("执行命令回收");
        }
    }
}
