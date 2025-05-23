
using System.Collections;
using UnityEngine;

namespace FPS
{
    public class GameOverController :MonoBehaviour,IController
    {
        private IController IC;
        private BoxCollider BoxCollider;
        private Camera Camera;
        private void Awake()
        {
            IC = this;
         
            BoxCollider =GetComponent<BoxCollider>();
            BoxCollider.enabled = false;
            Camera = GetComponentInChildren<Camera>();
            Camera.gameObject.SetActive(false);
            IC.RegisterEvent("RunToEvacuation");
            IC.StartListening("RunToEvacuation", (obj) => { BoxCollider.enabled = true; });
        }
        private void OnDestroy()
        {
            IC.StopListening("RunToEvacuation", (obj) => { BoxCollider.enabled = true; });
            IC.UnRegisterEvent("RunToEvacuation");
        }

        private void OnTriggerEnter(Collider other)
        {
            if(other.tag== "Player")
            {
                BoxCollider.enabled = false;
                IC.TriggerEvent("PlayerDead");
                Camera.gameObject.SetActive(true);
                //todo游戏结束
                // IC.OpenPanel<GameFinishPanel>();
                StartCoroutine(GameOver());
            }
        }

        IEnumerator GameOver()
        {
            IC.PlaySFX("ToEnd", null, AudioClipType.Normal);
            IC.TriggerEvent("CloseGuidePanel");
            yield return MoveToTarget(new Vector3(0.721817017f, 0.403826475f, -21f),3f);
            IC.OpenPanel<TransitionPanel>();
            yield return new WaitForSeconds(1.1f);

            IC.OpenPanel<GameFinishPanel>();

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

            float speed = Vector3.Distance(Camera.transform.localPosition, target) / time;

            while (Vector3.Distance(Camera.transform.localPosition, target) > 0.1f)
            {
                Camera.transform.localPosition = Vector3.MoveTowards(Camera.transform.localPosition, target, speed * Time.deltaTime);

                yield return null; // 暂停当前帧，下一帧继续执行
                Debug.Log(Vector3.Distance(Camera.transform.localPosition, target));
            }
            Camera. transform.localPosition = target;//确保位置精确。


        }

    }
}
