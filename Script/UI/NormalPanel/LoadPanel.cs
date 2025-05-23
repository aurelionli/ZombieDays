
using System.Linq;

using UnityEngine;
using TMPro;
using DG.Tweening;
using System.Collections;
namespace FPS
{
    public class LoadPanel :BaseUIPanel
    {
        private TMP_Text loadingText;

        private RectTransform BlackOccluderDown;

        private RectTransform BlackOccluderUp;

        private CanvasGroup BackGround;
        protected override void Awake()
        {
            Level = PanelLevel.Two;
            Type = PanelType.NormalPanel;
            base.Awake();
            InitFindGameObject();
        }
        private void Start()
        {
    
        }



        private void InitFindGameObject()
        {
            BlackOccluderDown = GameTools.GetSingleComponentInChild<RectTransform>( gameObject,"BlackOccluderDown");
            BlackOccluderUp = GameTools.GetSingleComponentInChild<RectTransform>(gameObject, "BlackOccluderUp");
            BackGround = GameTools.GetSingleComponentInChild<CanvasGroup>(gameObject, "BackGround");
        }
        public override void Close()
        {
            Sequence loopTween = DOTween.Sequence();
            //1.第一步，两块黑屏合并
            loopTween.Append(BlackOccluderDown.DOLocalMoveY(-270f, 1f));
            loopTween.Join(BlackOccluderUp.DOLocalMoveY(270f, 1f));
            //2.淡出黑屏
            loopTween.Append(BackGround.DOFade(0f, 0.5f));
            //3.打开
            loopTween.Append(BlackOccluderDown.DOLocalMoveY(-810f, 1f));
            loopTween.Join(BlackOccluderUp.DOLocalMoveY(810f, 1f));
            loopTween.Play().OnComplete(
                
                () =>
                {
                    gameObject.SetActive(false);
                    Destroy(gameObject);
                }
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
            //1.第一步，淡入黑屏
            loopTween.Append(BackGround.DOFade(1f, 0.5f));

            //2.第二步，两块黑屏打开
            loopTween.Append( BlackOccluderDown.DOLocalMoveY(-810f, 1f));
           loopTween.Join(BlackOccluderUp.DOLocalMoveY(810f, 1f));

            loopTween.Play();/*.OnComplete(() =>
            {
                loopTween.Kill();
                loopTween = null;
            });*/



        }
        //我发现DOLocalMoveY的最终坐标判定，都是以锚点在中心点的时候判定的。
    }
}
