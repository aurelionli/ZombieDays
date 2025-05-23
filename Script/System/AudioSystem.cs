using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Unity.VisualScripting.Member;


public enum AudioClipType { Weapon,Normal,UI,Zombie }
namespace FPS_Manager
{
    public class AudioSystem : MonoBehaviour,ISystem
    {
        private ISystem IC;
        public AudioSource bgMusicSource;
        private float musicFadeDuration = 0.5f;
        private int maxSFXSources = 4;//全局音效
      //  public int maxSFXSourcesFromPlayer = 10;
        public List<AudioSource> globalSFXSources = new List<AudioSource>();

        private readonly Dictionary<string, AudioClip> audioClips = new Dictionary<string, AudioClip>();
        private readonly Dictionary<string, AudioSource> stateDrivenAudioSources = new Dictionary<string, AudioSource>();
        private readonly Dictionary<AudioClipType, string> audioClipsPath = new Dictionary<AudioClipType, string>();

        private readonly Dictionary<GameObject, List<AudioSource>> registeredAudioSources = new Dictionary<GameObject, List<AudioSource>>();
        private void Awake()
        {
            IC =this;
            InitializeGlobalSFXSources();
            //  PlayerBGMusic("BG00");
            RegiserAudioPathDic();


       
        }
        private void Start()
        {
            IC.StartListening("SetBgMusic",SetBGMusicBVolume);
            IC.StartListening("SetMasterMusic",SetMasterVolume);
            IC.StartListening("SetSFXMusic",SetSFXVolume);
        }
        private void OnDestroy()
        {
            IC.StopListening("SetBgMusic", SetBGMusicBVolume);
            IC.StopListening("SetMasterMusic", SetMasterVolume);
            IC.StopListening("SetSFXMusic", SetSFXVolume);
        }
        private void RegiserAudioPathDic()
        {
            audioClipsPath.Add(AudioClipType.Normal, "Audio/");
            audioClipsPath.Add(AudioClipType.Weapon, "Audio/GunReload/");
            audioClipsPath.Add(AudioClipType.UI, "Audio/UI/");
            audioClipsPath.Add(AudioClipType.Zombie, "Audio/Zombie/");
        }
        /// <summary>
        /// 初始化全局音效
        /// </summary>
        private void InitializeGlobalSFXSources()
        {
            bgMusicSource = gameObject.AddComponent<AudioSource>();
            bgMusicSource.ignoreListenerPause = true;
            for (int i = 0; i < maxSFXSources; i++)
            {
                AudioSource source = gameObject.AddComponent<AudioSource>();
                source.playOnAwake = false;
                source.ignoreListenerPause = true;
                source.spatialBlend = 0;// 全局音效，无3D效果
                globalSFXSources.Add(source);
            }

        }



        

        /// <summary>
        /// 播放音效
        /// </summary>
        /// <param name="name">音效名字</param>
        /// <param name="volume">音量</param>
        /// <param name="priority">优先级</param>
        public void PlaySFX(string name, GameObject target = null, float volume = 1.0f, int priority = 128, AudioClipType type = AudioClipType.Normal)
        {
            AudioSource source = null;
            //Debug.Log(name + "调用"+ target.name);

            source = GetAvailableAudioSource(target);


            //AudioSource source = GetAvailableSFXSource();
            if (source == null) { Debug.LogError("没有找到限制的音效Source"); return; }
            if (audioClips.ContainsKey(name))
            {
               // PlayClip(source, audioClips[name], volume, priority);
                if (source != null)
                 {
                    PlayClip(source, audioClips[name], volume, priority);
                   /* source.clip = audioClips[name];
                     source.volume = volume;
                     source.priority = priority;
                    source.minDistance = 30f;
                     source.Play();*/
                 }
                return;
            }
     
            LoadAudioClip(name, (clip) =>
            {
                if (source != null)
                {
                    PlayClip(source,clip, volume, priority);
                 /*   source.clip = clip;
                    source.volume = volume;
                    source.priority = priority;
                    source.minDistance = 30f;
                    source.Play();*/
                }
            }, type
            );
        }
        /// <summary>
        /// 播放音频片段
        /// </summary>
        private void PlayClip(AudioSource source, AudioClip clip, float volume, int priority)
        {
            source.clip = clip;
            source.volume = volume;
            source.priority = priority;
            source.minDistance = 5f;
            source.maxDistance = 6f;
            source.Play();
        }
       /* /// <summary>
        /// 获取目标对象注册的所有音效源
        /// </summary>
        /// <param name="targetName">目标对象名称</param>
        /// <returns>音效源列表</returns>
        private List<AudioSource> GetRegisteredAudioSources(string targetName)
        {
           // List<AudioSource> sources = new List<AudioSource>();
            List<AudioSource> sources = registeredAudioSources[targetName];
           /* foreach (var pair in registeredAudioSources)
            {
                if (pair.Key.StartsWith(targetName))
                {
                    sources.Add(pair.Value);
                }
            }
            return sources;
        }*/
        /// <summary>
        /// 注册音效源
        /// </summary>
        public void RegisterAudioSource(GameObject target, int count = 1, float volume = 1.0f, int priority = 128)
        {
            Debug.Log($"{target.name}注册音效源，数量为{count}");
            if (target == null)
            {
                Debug.LogError("目标对象为空！");
                return;
            }
            if (registeredAudioSources.ContainsKey(target))
            {
                Debug.LogError($"{target.name}已经注册过音效源！");
                return;
            }
            List<AudioSource> sources = new List<AudioSource>();
            for(int i = 0; i < count; i++)
            {
                AudioSource source = target.AddComponent<AudioSource>();
                source.playOnAwake = false;
                source.spatialBlend = 1.0f; // 3D音效
                source.volume = volume;
                source.priority = priority;
                source.minDistance = 5f;
                source.maxDistance = 6f;
                sources.Add(source);
            }
            registeredAudioSources.Add(target, sources);
            /*
            for (int i = 0; i < count; i++)
            {
                string sourceName = $"{target.name}_AudioSource_{i}";
                Debug.LogWarning($"音效源 {sourceName} 注册！");
                if (registeredAudioSources.ContainsKey(sourceName))
                {
                    Debug.LogWarning($"音效源 {sourceName} 已注册！");
                    continue;
                }

                AudioSource source = target.AddComponent<AudioSource>();
                source.playOnAwake = false;
                source.spatialBlend = 1.0f; // 3D音效
                source.volume = volume;
                source.priority = priority;

                registeredAudioSources.Add(sourceName, source);
            }*/
        }

        /// <summary>
        /// 注销音效源
        /// </summary>
        public void UnregisterAudioSource(GameObject target)
        {
            if (target == null)
            {
                Debug.LogError("目标对象为空！");
                return;
            }
            List<AudioSource> audioSources = registeredAudioSources[target];
            foreach (AudioSource audioSource in audioSources)
            {
                Destroy(audioSource);
            }
            // 清空列表
            audioSources.Clear();
            registeredAudioSources.Remove(target);
          /*  List<string> keysToRemove = new List<string>();
            foreach (var pair in registeredAudioSources)
            {
                if (pair.Key.StartsWith(target.name))
                {
                    Destroy(pair.Value);
                    keysToRemove.Add(pair.Key);
                }
            }

            foreach (string key in keysToRemove)
            {
                registeredAudioSources.Remove(key);
            }*/
        }
        /// <summary>
        /// 获取闲置的音效播放器
        /// </summary>
        /// <returns></returns>
        private AudioSource GetAvailableAudioSource(GameObject target = null)
        {
            //Debug.Log($"获得{target.name}的音效源。");
            if(target != null && registeredAudioSources.ContainsKey(target))
            {
                // 获取目标对象的所有音效源
                List<AudioSource> targetSources = registeredAudioSources[target];
                // 查找可用的音效源
                foreach (AudioSource source in targetSources)
                {
                    if (!source.isPlaying)
                    {
                        return source;
                    }
                }
                //如果没有限制的播放器，就优先返回优先级最低的播放器
                AudioSource lowestPrioritySource = targetSources[0];
                foreach (AudioSource source in targetSources)
                {
                    if (source.priority < lowestPrioritySource.priority)
                    {
                        lowestPrioritySource = source;
                    }
                }
                lowestPrioritySource.Stop();
                return lowestPrioritySource;
            }
            // 如果没有指定目标对象，或者目标对象未注册音效源，使用全局音效源
            return GetAvailableGlobalSFXSource();
            //返回播放器
            /*  foreach (AudioSource source in sfxSources)
              {
                  if (!source.isPlaying)
                  {
                      return source;
                  }
              }
              //如果没有限制的播放器，就优先返回优先级最低的播放器
              AudioSource lowestPrioritySource = sfxSources[0];
              foreach (AudioSource source in sfxSources)
              {
                  if (source.priority < lowestPrioritySource.priority)
                  {
                      lowestPrioritySource = source;
                  }
              }
              lowestPrioritySource.Stop();
              return lowestPrioritySource;*/
        }
        /// <summary>
        /// 获取可用的全局音效源
        /// </summary>
        private AudioSource GetAvailableGlobalSFXSource()
        {
            foreach (AudioSource source in globalSFXSources)
            {
                if (!source.isPlaying)
                {
                    return source;
                }
            }

            // 如果没有可用的音效源，返回优先级最低的音效源
            AudioSource lowestPrioritySource = globalSFXSources[0];
            foreach (AudioSource source in globalSFXSources)
            {
                if (source.priority < lowestPrioritySource.priority)
                {
                    lowestPrioritySource = source;
                }
            }
            lowestPrioritySource.Stop();
            return lowestPrioritySource;
        }




        #region 音频的加载
        /// <summary>
        /// 放在协程就是可以监控进度，停止或继续加载，而放在普通就是单纯异步。
        /// </summary>
        /// <param name="name"></param>
        /// <param name="onLoaded"></param>
        private void LoadAudioClip(string name, Action<AudioClip> onLoaded, AudioClipType type = AudioClipType.Normal)
        {
            string path = audioClipsPath[type] + name;
            Debug.Log(path);
            ResourceRequest request = Resources.LoadAsync<AudioClip>(path);
            //operation 表示当前资源加载操作的实例。
            /*在这个特定的回调中，operation 并没有被直接使用，
             * 因为 request 已经包含了加载的资源（通过 request.asset 获取）。
                但在其他场景中，operation 可以用于检查异步操作的状态或结果。
             */
            request.completed += (Operation) =>
            {
                AudioClip clip = request.asset as AudioClip;

                if (clip != null)
                {
                    audioClips[name] = clip;
                    onLoaded(clip);
                }
                else
                {
                    Debug.LogError($"音频资源{name}加载失败");
                }
            };
        }
        #endregion
        /*  /// <summary>
          /// 注册音效源
          /// </summary>
          public void RegisterAudioSource(GameObject target, float volume = 1.0f, int priority = 128)
          {
              if (target == null)
              {
                  Debug.LogError("目标对象为空！");
                  return;
              }

              if (registeredAudioSources.ContainsKey(target.name))
              {
                  Debug.LogWarning($"目标对象 {target.name} 已注册音效源！");
                  return;
              }

              AudioSource source = target.AddComponent<AudioSource>();
              source.playOnAwake = false;
              source.spatialBlend = 1.0f; // 3D音效
              source.volume = volume;
              source.priority = priority;

              registeredAudioSources.Add(target.name, source);
          }*/
        #region 背景音乐
        /// <summary>
        /// 播放背景音乐
        /// </summary>
        /// <param name="name">名字</param>
        /// <param name="loop">是否循环</param>
        public void PlayerBGMusic(string name, bool loop = true, AudioClipType type = AudioClipType.Normal)
        {
            //如果字典里有
            if (audioClips.ContainsKey(name))
            {
                StartCoroutine(FadeMusic(audioClips[name], loop));
            }
            else
            {
                LoadAudioClip(name, (clip) => StartCoroutine(FadeMusic(clip, loop)), type);
            }
        }
        /// <summary>
        /// 停止播放背景音乐
        /// </summary>
        public void StopBGMusic()
        {
            StartCoroutine(FadeMusic(null));
        }
        /// <summary>
        /// 背景音乐的淡入淡出
        /// </summary>
        /// <param name="newClip"></param>
        /// <param name="loop"></param>
        /// <returns></returns>
        private IEnumerator FadeMusic(AudioClip newClip, bool loop = true)
        {
      
            if (bgMusicSource.isPlaying)
            {
                float startVolume = bgMusicSource.volume;
                float elapsed = 0;

                while (elapsed < musicFadeDuration)
                {
                    bgMusicSource.volume = Mathf.Lerp(startVolume, 0, elapsed / musicFadeDuration);
                    elapsed += Time.deltaTime;
                
                    yield return null;
                }
                bgMusicSource.Stop();
            }
            else
            {
                yield return new WaitForSeconds(musicFadeDuration);
            
            }
            //淡入音乐
            if (newClip != null)
            {
                bgMusicSource.clip = newClip;
                bgMusicSource.loop = loop;
                bgMusicSource.Play();
            

                float elapsed = 0;
                while (elapsed < musicFadeDuration)
                {
                    bgMusicSource.volume = Mathf.Lerp(0, 1, elapsed / musicFadeDuration);
                    elapsed += Time.deltaTime;
                
                    yield return null;
                }
            }
            
        }
        #endregion

        #region 状态音效
        /// <summary>
        /// 注册音效，与销毁对应。这种是持续性音效
        /// </summary>
        /// <param name="id"></param>
        /// <param name="volume"></param>
        /// <param name="priority"></param>
        public void RegisterStateAudioSource(GameObject g, string id, float volume = 1.0f, int priority = 128, AudioClipType type = AudioClipType.Normal)
        {
            if (!stateDrivenAudioSources.ContainsKey(id))
            {
                AudioSource temp = g.AddComponent<AudioSource>();
                LoadAudioClip(id, (clip) =>
                {
                    temp.clip = clip;
                    temp.loop = true;
                    temp.volume = volume;
                    temp.priority = priority;
                    temp.spatialBlend = 1;
                    stateDrivenAudioSources.Add(id, temp);
                }, type);

                return;
            }
            Debug.LogWarning($"已经注册了这个状态音效{id}");
        }
        /// <summary>
        /// 注销持续性音效
        /// </summary>
        /// <param name="id">音效名字</param>
        public void UnRegisterStateAudioSource(string id)
        {
            AudioSource temp = stateDrivenAudioSources[id];
            Destroy(temp);
            stateDrivenAudioSources.Remove(id);
        }

        /// <summary>
        /// 调整状态驱动音效的音量
        /// </summary>
        /// <param name="id"></param>
        /// <param name="i"></param>
        public void SetPlayerStateAudioSource(string id, float i)
        {
            if (stateDrivenAudioSources.ContainsKey(id))
            {
                stateDrivenAudioSources[id].volume = i;
            }
            else
            {
                Debug.Log($"{id}没有被注册！");
            }
        }
        /// <summary>
        /// 播放状态驱动音效
        /// </summary>
        /// <param name="id">音效名字</param>
        public void PlayerStateAudioSource(string id)
        {
            if (stateDrivenAudioSources.ContainsKey(id))
            {
                stateDrivenAudioSources[id].Play();
            }
            else
            {
                Debug.Log($"{id}没有被注册！");
            }
        }
        /// <summary>
        /// 停止状态驱动音效
        /// </summary>
        /// <param name="id">音效名字</param>
        public void StopStateAudioSource(string id)
        {
            if (stateDrivenAudioSources.ContainsKey(id))
            {
                stateDrivenAudioSources[id].Pause();
            }
            else
            {
                Debug.Log($"{id}没有被注册！");
            }
        }
        #endregion

        #region 音量调节
        /// <summary>
        /// 全局音量
        /// </summary>
        /// <param name="volume"></param>
        public void SetMasterVolume(object data)
        {
            AudioListener.volume = IC.GetModel<MusicModel>().masterMusicVolume;
        }
        /// <summary>
        /// 背景音乐
        /// </summary>
        /// <param name="volume"></param>
        public void SetBGMusicBVolume(object data)
        {
            bgMusicSource.volume = IC.GetModel<MusicModel>().bgMusicVolume;
        }
        /// <summary>
        /// 音效
        /// </summary>
        /// <param name="volume"></param>
        public void SetSFXVolume(object data)
        {
            foreach (AudioSource source in globalSFXSources)
            {
                source.volume = IC.GetModel<MusicModel>().SFXVolume;
            }
            
            foreach(List<AudioSource>  items in registeredAudioSources.Values)
            {
                foreach(AudioSource item in items)
                {
                    item.volume = IC.GetModel<MusicModel>().SFXVolume;
                }
            }

            foreach (var item in stateDrivenAudioSources.Values)
            {
                item.volume = IC.GetModel<MusicModel>().SFXVolume;
            }


        }
        #endregion
    }
}
