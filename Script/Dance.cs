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
            // ����ļ�·��
            if (!System.IO.File.Exists(filePath))
            {
                Debug.LogError($"File not found: {filePath}");
                yield break;
            }

            // ���ļ�·��ת��ΪURL��ʽ
            string fileUrl = "file://" + filePath;

            // ʹ��UnityWebRequest����MP3�ļ�
            using (UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip(fileUrl, AudioType.MPEG))
            {
                yield return www.SendWebRequest();

                // �������Ƿ�ɹ�
                if (www.result == UnityWebRequest.Result.Success)
                {
                    // ��ȡAudioClip
                    AudioClip audioClip = DownloadHandlerAudioClip.GetContent(www);

                    // ������Ƶ
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
            // ���AudioSource�����������Ƶ
            IC.PlayerBGMusic("Dance", true, AudioClipType.Normal);


        }
    }
}