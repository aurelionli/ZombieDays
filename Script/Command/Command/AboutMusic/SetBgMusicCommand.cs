using FPS_Manager;


namespace FPS
{

    //完全回复状态，并且加一点血量上限/
    public class SetBgMusicCommand : Command
    {
        protected override void OnExecute(object data = null)
        {
            IC.GetModel<MusicModel>().bgMusicVolume = (float)data;
            IC.TriggerEvent("SetBgMusic");
          

        }

       


    }
}
