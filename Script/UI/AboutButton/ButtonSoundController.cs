using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
namespace FPS
{
    public class ButtonSoundController : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler,IController
    {
        private IController IC;
        private Button bt;
        private void Awake()
        {
            IC = this;
            bt = GetComponent<Button>();
        }
        public void OnPointerEnter(PointerEventData eventData)
        {
            if(bt.interactable)
            {
                IC.PlaySFX("Hover",null, AudioClipType.UI);
            }
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (bt.interactable)
            {
                IC.PlaySFX("Click",null, AudioClipType.UI);
            }
        }
        
    }
}
