using UnityEngine;

using DG.Tweening;
using System.Collections;


namespace FPS
{
    public class TransitionPanel  :BaseUIPanel
    {
        private RectTransform BlackOccluderDown;

        private RectTransform BlackOccluderUp;


        protected override void Awake()
        {
            Level = PanelLevel.One;
            Type = PanelType.AutoClosePopupPanel;
            base.Awake();
            BlackOccluderDown = GameTools.GetSingleComponentInChild<RectTransform>(gameObject, "BlackOccluderDown");
            BlackOccluderUp = GameTools.GetSingleComponentInChild<RectTransform>(gameObject, "BlackOccluderUp");
        }
        private void Start()
        {
            StartCoroutine(OpenAndClose());
        }
        IEnumerator OpenAndClose()
        {
            yield return new WaitForSeconds(1f);
            Close();
        }

        public override void Close()
        {
            Sequence loopTween = DOTween.Sequence();
            //1.第一步，两块黑屏合并
            loopTween.Append(BlackOccluderDown.DOLocalMoveY(-270f, 1f));
            loopTween.Join(BlackOccluderUp.DOLocalMoveY(270f, 1f));
            loopTween.Play().OnComplete(
                () =>
                {
                    gameObject.SetActive(false);
                    Destroy(gameObject);
                }
           //3.打开
           /*   loopTween.Append(BlackOccluderDown.DOLocalMoveY(-810f, 1f));
              loopTween.Join(BlackOccluderUp.DOLocalMoveY(810f, 1f));
              loopTween.Play().OnComplete(

                  () =>
                  {
                      gameObject.SetActive(false);
                      Destroy(gameObject);
                  }*/
           /* loopTween.Kill(); loopTween = null;*/ //Destroy(gameObject);
           );

        }

        //这个加载面板，没有隐藏和显示，只有开启和关闭
        public override void Hide()
        {

        }
        public override void Show()
        {

        }
        public override void Open()
        {
            Sequence loopTween = DOTween.Sequence();
            gameObject.SetActive(true);
            loopTween.Append(BlackOccluderDown.DOLocalMoveY(-270f, 1f));
            loopTween.Join(BlackOccluderUp.DOLocalMoveY(270f, 1f));


           /* //2.第二步，两块黑屏打开
            loopTween.Append(BlackOccluderDown.DOLocalMoveY(-810f, 1f));
            loopTween.Join(BlackOccluderUp.DOLocalMoveY(810f, 1f));*/

            loopTween.Play();/*.OnComplete(() =>
            {
                loopTween.Kill();
                loopTween = null;
            });*/



        }
    }
}
