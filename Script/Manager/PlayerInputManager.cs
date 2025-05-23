
namespace FPS_Manager
{
    public class PlayerInputManager:ISystem
    {
        private ISystem IC;
        public PlayerControls playerControls;

        public PlayerInputManager()
        {
            playerControls = new PlayerControls();
            playerControls.Enable();
            IC = this;
            IC.RegisterEvent("ControlInput");

            IC.StartListening("ControlInput", ControlInput);
        }


        public void ControlInput(object data)
        {
            bool value = (bool)data;
            if (value) { playerControls.Enable(); }
           else { playerControls.Disable(); }
        }
    }
}
