using FPS_Manager;


namespace FPS
{


    public class SetPlayerCurrentStateCommand : Command
    {
        protected override void OnExecute(object data = null)
        {
            MovementState i = (MovementState)data;
            IC.GetModel<PlayerCurrentStateModel>().playerCurrentState = i;



        }
    }
}
