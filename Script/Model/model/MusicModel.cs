

namespace FPS_Manager
{
    internal class MusicModel:Imodel
    {
        Imodel IC;
        public float bgMusicVolume { get; set; }
        public float SFXVolume { get; set; }
        public float masterMusicVolume { get; set; }    


        public MusicModel InitEvent()
        {
            IC = this;
            bgMusicVolume = 1f;
            SFXVolume = 1f;
            masterMusicVolume =1f;
            IC.RegisterEvent("SetBgMusic");
            IC.RegisterEvent("SetMasterMusic");

            IC.RegisterEvent("SetSFXMusic");
            return this;
        }
        ~MusicModel()
        {
            IC.UnRegisterEvent("SetBgMusic");
            IC.UnRegisterEvent("SetMasterMusic");

            IC.UnRegisterEvent("SetSFXMusic");
        }
    }
}
