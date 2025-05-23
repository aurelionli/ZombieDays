

using UnityEngine;

namespace FPS
{
    public class PlayerController : MonoBehaviour,IController,IDamage
    {
        public IController IC;
        public PlayerReuseableData reuseableData = new PlayerReuseableData();

        public Player_SO player_SO;


        public PlayerControls inputActions;
        public PlayerControls.PlayerActions player;
        public PlayerControls.WeaponActions weapon;

        private CharacterController characterController;

        private void Awake()
        {
          //  player_SO = IC.GetData<Player_SO>();
            IC = this;
            player = IC.GetPlayerControls();
            weapon = IC.GetWeaponControls();
            
            IC.RegisterAudioSource(gameObject, 2);
            characterController = GetComponent<CharacterController>();
         


           
        }
       
        public void Damage(int i)
        {
            IC.TriggerEvent("PlayerBeHurt");
            IC.SendCommand<DecreasePlayerHealthCommand>(i);

        }

        // Start is called before the first frame update
        void Start()
        {
            //IC.StartListening("PlayerDead", (obj) => { Destroy(gameObject); });
        }

        // Update is called once per frame
        void Update()
        {
            if(player.OpenUI.triggered)
            {
           
                IC.OpenPanel<SettingPanel>();
            }
        }
        void OnDestroy()
        {
            //IC.StopListening("PlayerDead", (obj) => { Destroy(gameObject); });
        }
       
    }
}