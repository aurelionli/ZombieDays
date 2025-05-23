using FPS_Manager;


namespace FPS
{

    //完全回复状态，并且加一点血量上限/
    public class SetMasterMusicCommand : Command
    {
        protected override void OnExecute(object data = null)
        {
            IC.GetModel<MusicModel>().masterMusicVolume = (float)data;
            IC.TriggerEvent("SetMasterMusic");

            

        }
    }
}
