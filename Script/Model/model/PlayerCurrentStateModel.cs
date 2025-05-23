



namespace FPS_Manager
{
    public class PlayerCurrentStateModel :Imodel
    {
        public int playerHealth;
        public int currentHealth;

        public float playerEnergy;
        public float currentEnergy;
        public MovementState playerCurrentState;

        public PlayerCurrentStateModel InitEvent()
        {
            playerHealth = 100;
            currentHealth = 100;

            playerEnergy = 100f;
            currentEnergy = 100f;
            playerCurrentState = MovementState.IDLE;
            return this;
        }

    }
}
