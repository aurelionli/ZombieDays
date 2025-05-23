

using UnityEngine;

public enum MovementState { WALK, RUN, CROUCH, JUMP, IDLE ,CLIMB }
namespace FPS
{
   
    public class PlayerReuseableData
    {


        public bool lastStateIsFire { get; set; } = false;
        public bool canSwitchWeapon {  get;  set; }//限制切换武器出现的Bug
        public MovementState state { get; set; }

        public WeaponType weaponType { get; set; }

        public int weaponId {  get; set; }  
        public float moveSpeed {  get; set; }

        public bool cantRun {  get; set; }

        public int aimMode { get; set; } = 0;

        public bool aiming {  get; set; }   

        public bool silencer {  get; set; } 

        public float enterFireTime { get; set; } 
    }
}
