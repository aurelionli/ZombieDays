
using FPS_Manager;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace FPS
{
    public class SettingPanel :BaseUIPanel
    {
      
        private Button returnButton;
        private Button closeButton;

        private Slider masterVolume;
        private Slider bgMusicVolume;
        private Slider SFXVolume;
        protected override void Awake()
        {
            Cursor.lockState = CursorLockMode.None;//鼠标解锁并显示
            Level = PanelLevel.Three;
            Type = PanelType.NormalPanel;
            base.Awake();
            InitFindGameObject();
        }
        private void InitFindGameObject()
        {
            returnButton = GameTools.GetSingleComponentInChild<Button>(gameObject, "ReturnMainMenuButton");
            closeButton = GameTools.GetSingleComponentInChild<Button>(gameObject, "CloseButton");

            returnButton.AddComponent<ButtonSoundController>();
            closeButton.AddComponent<ButtonSoundController>();

            masterVolume = GameTools.GetSingleComponentInChild<Slider>(gameObject, "MasterVolumeSlider");
            bgMusicVolume = GameTools.GetSingleComponentInChild<Slider>(gameObject, "BGMusicVolumeSlider");
            SFXVolume = GameTools.GetSingleComponentInChild<Slider>(gameObject, "SFXVolumeSlider");

            masterVolume.value = IC.GetModel<MusicModel>().masterMusicVolume;
            bgMusicVolume.value = IC.GetModel<MusicModel>().bgMusicVolume;
            SFXVolume.value =IC.GetModel<MusicModel>().SFXVolume;
        }

        private void Start()
        {
            ListenEvent();
        }
        private void ListenEvent()
        {
            returnButton.onClick.AddListener(OnClicked_ReturnMainMenu);
            closeButton.onClick.AddListener(OnClicked_ClosePanel);

            masterVolume.onValueChanged.AddListener(OnClicked_MasterVolumeSlider);
            bgMusicVolume.onValueChanged.AddListener(OnClicked_BGMusicVolumeSlider);
            SFXVolume.onValueChanged.AddListener(OnClicked_SFXVolume);
        }


        public override void Hide()
        {
            base.Hide();
            Cursor.lockState = CursorLockMode.Locked;
            IC.TriggerEvent("ControlInput", true);
        }
        public override void Open()
        {
            base.Open();
            Cursor.lockState = CursorLockMode.None;
            IC.TriggerEvent("ControlInput", false);
        }
        public override void Close()
        {
            base.Close();
            Cursor.lockState = CursorLockMode.Locked;
            IC.TriggerEvent("ControlInput", true);
        }
        private void OnClicked_ReturnMainMenu()
        {
         
            IC.LoadScene(0);
        }
        private void OnClicked_ClosePanel()
        {
            IC.ClosePanel();
        }
        private void OnClicked_MasterVolumeSlider(float value)
        {
            // IC.SetMasterVolume(value);
            IC.SendCommand<SetMasterMusicCommand>(value);
        }
        private void OnClicked_BGMusicVolumeSlider(float value)
        {
            // IC.SetBGMusicBVolume(value);
            IC.SendCommand<SetBgMusicCommand>(value);
        }
        private void OnClicked_SFXVolume(float value)
        {
            IC.SendCommand<SetSFXCommand>(value);
          //  IC.SetSFXVolume(value);
        }
 
    }
}
