using FPS_Manager;


namespace FPS
{

    //完全回复状态，并且加一点血量上限/
    public class SetSFXCommand : Command
    {
        protected override void OnExecute(object data = null)
        {
            IC.GetModel<MusicModel>().SFXVolume = (float)data;

            IC.TriggerEvent("SetSFXMusic");
        }
    }
}
