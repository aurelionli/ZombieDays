using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

namespace FPS
{
    public class Dance : MonoBehaviour,IController
    {
        private IController IC;
     

        private void Start()
        {
            IC = this;
            
       //     IC.PlayerBGMusic("Dance", true, AudioClipType.Normal);
        }

       /* private IEnumerator LoadAndPlayMP3(string filePath)
        {
            // 检查文件路径
            if (!System.IO.File.Exists(filePath))
            {
                Debug.LogError($"File not found: {filePath}");
                yield break;
            }

            // 将文件路径转换为URL格式
            string fileUrl = "file://" + filePath;

            // 使用UnityWebRequest加载MP3文件
            using (UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip(fileUrl, AudioType.MPEG))
            {
                yield return www.SendWebRequest();

                // 检查加载是否成功
                if (www.result == UnityWebRequest.Result.Success)
                {
                    // 获取AudioClip
                    AudioClip audioClip = DownloadHandlerAudioClip.GetContent(www);

                    // 播放音频
                    PlayAudioClip(audioClip);
                }
                else
                {
                    Debug.LogError($"Failed to load MP3 file: {www.error}");
                }
            }
        }*/

        private void Ainm_PlayAudioClip()
        {
            // 添加AudioSource组件并播放音频
            IC.PlayerBGMusic("Dance", true, AudioClipType.Normal);


        }
    }
}