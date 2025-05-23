using System;
using System.Security.Cryptography;
using FPS_Manager;
using UnityEngine;
namespace FPS
{
    //1.获得Model
    //2.发送Command
    //3.注册事件
    //4.音效
    public interface IController
    {

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
        /// 访问Command层发送
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        public void SendCommand<T>(object data = null) where T : Command, new()
        {
            GameManager.Instance.CommandManager.SendCommand<T>(data);
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
        /// 播放音效
        /// </summary>
        /// <param name="name">名字</param>
        /// <param name="volume">音量(可选)</param>
        /// <param name="priority">优先级(可选)</param>
        public void PlaySFX(string name, GameObject obj=null,AudioClipType type = AudioClipType.Normal, float volume = 1.0f, int priority = 128)
        {
            GameManager.Instance.AudioSystem.PlaySFX(name,obj, volume, priority, type);   
        }
        /// <summary>
        /// 注册音效源
        /// </summary>
        /// <param name="target"></param>
        /// <param name="count"></param>
        /// <param name="volume"></param>
        /// <param name="priority"></param>
        public void RegisterAudioSource(GameObject target, int count = 1, float volume = 1.0f, int priority = 128)
        {
            GameManager.Instance.AudioSystem.RegisterAudioSource(target, count, volume, priority);
        }
        /// <summary>
        /// 注销音效源
        /// </summary>
        /// <param name="target"></param>
        public void UnregisterAudioSource(GameObject target)
        {
            GameManager.Instance.AudioSystem.UnregisterAudioSource(target);
        }
        public PlayerControls.PlayerActions GetPlayerControls()
        {
            return GameManager.Instance.inputManager.playerControls.Player;
            
        }
        public PlayerControls.WeaponActions GetWeaponControls()
        {
            return GameManager.Instance.inputManager.playerControls.Weapon;
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
        /// 停止播放BGM
        /// </summary>
        public void StopBGMusic()
        {
            GameManager.Instance.AudioSystem.StopBGMusic();
        }
        /// <summary>
        /// 播放可控
        /// </summary>
        /// <param name="id"></param>
        public void PlayerStateAudioSource(string id)
        {
             GameManager.Instance.AudioSystem.PlayerStateAudioSource( id);
        }
        /// <summary>
        /// 播放可控
        /// </summary>
        /// <param name="id"></param>
        public void StopStateAudioSource(string id)
        {
            GameManager.Instance.AudioSystem.StopStateAudioSource(id);
        }
        /// <summary>
        /// 注册
        /// </summary>
        /// <param name="id"></param>
        /// <param name="volume"></param>
        /// <param name="priority"></param>
        public void RegisterStateAudioSource(GameObject obj,string id, float volume = 1.0f, int priority = 128)
        {
            GameManager.Instance.AudioSystem.RegisterStateAudioSource(obj,id,  volume, priority);
        }
        /// <summary>
        /// 注销
        /// </summary>
        /// <param name="id"></param>
        /// <param name="volume"></param>
        /// <param name="priority"></param>
        public void UnRegisterStateAudioSource(string id)
        {
            GameManager.Instance.AudioSystem.UnRegisterStateAudioSource( id);
        }
        /// <summary>
        /// 调整状态音效的音量
        /// </summary>
        /// <param name="id"></param>
        /// <param name="i">0----1</param>
        public void SetPlayerStateAudioSource(string id, float i)
        {
            GameManager.Instance.AudioSystem.SetPlayerStateAudioSource(id, i);
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
            return GameManager.Instance.objectPoolSystem.GetObject(id,pos,quaternion);
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
       /* /// <summary>
        /// 调整全局音量
        /// </summary>
        /// <param name="volume">0-1</param>
        public void SetMasterVolume(float volume)
        {
            GameManager.Instance.AudioSystem.SetMasterVolume(volume);
        }
       /// <summary>
        /// 调整背景音量
        /// </summary>
        /// <param name="volume">0-1</param>
        public void SetBGMusicBVolume(float volume)
        {
            GameManager.Instance.AudioSystem.SetBGMusicBVolume(volume);
        }
        /// <summary>
        /// 调整音效音量
        /// </summary>
        /// <param name="volume">0-1</param>
        public void SetSFXVolume(float volume)
        {
            GameManager.Instance.AudioSystem.SetSFXVolume(volume);
        }*/
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
        public void OpenPanel<T>(bool isPopup = false,string message =null ) where T : BaseUIPanel
        {
            GameManager.Instance.UIManager.OpenPanel<T>(isPopup, message);
        }
        /// <summary>
        /// 加载场景
        /// </summary>
        /// <param name="sceneName"></param>
        public void LoadScene(int sceneName)
        {
            
            GameManager.Instance.sceneSystem.LoadScene(sceneName);
        }
    }
}
