using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace FPS
{
    
    public class GameFinishPanel :BaseUIPanel
    {

        private Button returnMainMenuButton;
        public AudioSource audioSource;
        public Text lyricText01;
        public Text lyricText02;
        public TextAsset lrcFie;

        private Transform dance;

        private LrcParser lrcParser = new LrcParser();
        private int currentLineIndex = 0; //当前引索
        public float fadeDuration = 1f; // 淡入淡出时间
        protected override void Awake()
        {
            Cursor.lockState = CursorLockMode.None;//鼠标解锁并显示
            Level = PanelLevel.Two;
            Type = PanelType.NormalPanel;
            base.Awake();
            InitFindGameObject();

 
        
        }
        private void Start()
        {
            lrcFie = Resources.Load<TextAsset>("lyyy");
            lrcParser.Parse(lrcFie.text);


            IC.StopBGMusic();
            StartCoroutine(PlayLyrics());
           // lyricText.DOText($"asdasdasdasdasdasdasasdas", 1f);
        }
        public override void Open()
        {
            base.Open();
            IC.TriggerEvent("ControlInput",false);
        }
        public override void Close()
        {
            base.Close();
            Cursor.lockState = CursorLockMode.Locked;//鼠标解锁并显示
            IC.TriggerEvent("ControlInput", true);
        }
        private void InitFindGameObject()
        {
            audioSource = GetComponent<AudioSource>();
            returnMainMenuButton = GameTools.GetSingleComponentInChild<Button>(gameObject, "ReturnMainMenuButton");
            returnMainMenuButton.AddComponent<ButtonSoundController>();
            returnMainMenuButton.onClick.AddListener(()=>IC.LoadScene(0));
            returnMainMenuButton.gameObject.SetActive(false);
            lyricText01 = GameTools.GetSingleComponentInChild<Text>(gameObject, "LyricsText01");
            lyricText02= GameTools.GetSingleComponentInChild<Text>(gameObject, "LyricsText02");
            dance = GameTools.GetSingleComponentInChild<Transform>(gameObject, "Dance");
            dance.gameObject.SetActive(false);
        }
        private IEnumerator PlayLyrics()
        {
            yield return new WaitForSeconds(3f);
            returnMainMenuButton.gameObject.SetActive(true);
            dance.gameObject.SetActive(true);
            audioSource.Play();
            while (currentLineIndex < lrcParser.lyrics.Count-1)
            {
                float currentTime = audioSource.time;
                float lyricTime = lrcParser.lyrics[currentLineIndex].time;

                // 如果当前时间接近歌词时间，触发显示动画
                if (currentTime >= lyricTime - fadeDuration)
                {
                    lyricText01.text = "";
                    lyricText02.text = "";
                    string currentLyric = lrcParser.lyrics[currentLineIndex].text;
                    string nextLyric = lrcParser.lyrics[currentLineIndex + 1].text;
                    lyricText01.DOText(currentLyric, fadeDuration).SetEase(Ease.Linear);
                    lyricText02.DOText(nextLyric, fadeDuration).SetEase(Ease.Linear);
                    //ShowLyric(lrcParser.lyrics[currentLineIndex].text);
                    currentLineIndex +=2;
                }

                yield return null; // 等待下一帧
            }
        }

      
        private void Update()
        {
        
          /*  if (audioSource.isPlaying)
            {
                float currentTime = audioSource.time; // 当前播放时间
               
                // 查找当前应该显示的歌词
                for (int i = currentLineIndex; i < lrcParser.lyrics.Count-1; i+=2)
                {
                    if (currentTime >= lrcParser.lyrics[i].time)
                    {
                     
                        // 显示当前句和下一句歌词
                        string currentLyric = lrcParser.lyrics[i].text;
                        string nextLyric = lrcParser.lyrics[i + 1].text;
                        lyricText.text = $"{currentLyric}\n{nextLyric}"; // 更新歌词显示
                       // lyricText.DOText($"{currentLyric}\n{nextLyric}", 1f);
                        currentLineIndex = i;
                    }
                    else
                    {
                        break;
                    }
                }
            }*/
        }













        /// <summary>
        /// Text移动到目标位置
        /// </summary>
        /// <param name="target">目标位置</param>
        /// <param name="speed">到达目标的时间</param>
        /// <param name="complete">回调函数</param>
        /// <param name="size">相机size</param>
        /// <returns></returns>
        public IEnumerator MoveToTarget(Vector3 target, float time)
        {
            /*直接作为插值因子：适用于简单的、不需要精确控制的场景，但在帧率波动较大的情况下可能导致速度不稳定。
              基于时间的插值因子：提供了更稳定的移动速度和平滑的过渡效果，适用于需要精确控制移动时间和速度的场景。*/
            /* Debug.Log("进入移动摄像机协程");
             float duration = Vector3.Distance(transform.position, target) / speed;
             float elapsedTime = 0f;
              transform.position = target;
             while (elapsedTime < duration)
             {
                 // 使用 Lerp 进行平滑插值
                 transform.position = Vector3.Lerp(transform.position, target, elapsedTime / duration);
                 elapsedTime += Time.deltaTime;
                 yield return null; // 暂停当前帧，下一帧继续执行
             }
             transform.position = target;//确保位置精确。
             complete?.Invoke();
             Debug.Log("退出摄像机协程");*/
            //楼上这个会先快后慢.
            float speed = Vector3.Distance(transform.position, target) / time;

            while (Vector3.Distance(transform.position, target) > 0.01f)
            {
                transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);

                yield return null; // 暂停当前帧，下一帧继续执行

            }
            transform.position = target;//确保位置精确。
       

        }
    }
}
