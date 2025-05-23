using System.Collections.Generic;
using UnityEngine;


//1.长枪 2.手枪 3.冲锋枪 4.霰弹枪 5.狙击枪 6.rPG

public class NormalWeapon
{
    public int bulletMag =0;

    public int bulletNum =0;
   
}
public enum WeaponType { Arms, Smg, Hand, Sniper, Shot,Rpg }
namespace FPS_Manager
{
   
    public class CurrentWeaponModel :Imodel
    {
       
        //这个字典存储当前弹匣的子弹  id
        public Dictionary<int,int> weaponBulletMagDic= new Dictionary<int,int>();
        //这个字典存储不同类型的子弹
        public Dictionary<WeaponType, int> weaponBulletNumDic = new Dictionary<WeaponType, int>();
        public int currentWeaponId;//当前武器id

        public int throwNum;
        
        public WeaponType currentWeapon {  get;  set; }
        //同类型子弹通用，然后只是记录数量的话

        //这里是备弹
      /*  public int[] bulletNumGroup = new int[6];
        //这里是射击的弹
        public int[] bulletMag = new int[6];
       public int handGunBulletNum { get; set; }
        public int armsGunBulletNum { get; set; }
        public int smgGunBulletNum { get; set; }
        public int sniperGunBulletNum { get; set; }
        public int rpgGunBulletNum { get; set; }

        public int shotGunBulletNum { get; set; }*/

        /*
        public void TriggerEvent_SwitchWeapon(object data)//
        {
            int i = (int)data;
           // currentWeaponIndex = i; 
        }
        public void ResiterWeapon(NormalWeapon weapon)//获得这个武器的时候
        {
           // weapons.Add(weapon);
        }
        public void UnResiterWeapon(NormalWeapon weapon)//不要这个武器的时候
        {
            //weapons.Remove(weapon);
        }
        //1.手枪 2.长枪 3.冲锋枪 4.霰弹枪 5.狙击枪 6.rPG
        public int this[WeaponType weapon]
        {
            get
            {
                switch (weapon)
                {
                    case WeaponType.Hand:
                        return handGunBulletNum;
                    case WeaponType.Arms:
                        return armsGunBulletNum;
                    case WeaponType.Smg:
                        return smgGunBulletNum;
                    case WeaponType.Shot:
                        return shotGunBulletNum;
                    case WeaponType.Sniper:
                        return sniperGunBulletNum;
                    case WeaponType.Rpg:
                        return rpgGunBulletNum;
                    default:
                        return 0;
                }

            }
            set
            {
                switch (weapon)
                {
                    case WeaponType.Hand:
                        handGunBulletNum = value;
                        break;
                    case WeaponType.Arms:
                        armsGunBulletNum = value;
                        break;
                    case WeaponType.Smg:
                        smgGunBulletNum = value;
                        break;
                    case WeaponType.Shot:
                        shotGunBulletNum = value;    
                        break;
                    case WeaponType.Sniper:
                        sniperGunBulletNum = value;
                        break;
                    case WeaponType.Rpg:
                        rpgGunBulletNum = value;
                        break;
                }
            }
        }
        */
        Imodel IC;
        public CurrentWeaponModel InitEvent()
        {
            Debug.Log("链式调用");
            Imodel IC = this;
            throwNum =0;
             weaponBulletNumDic.Add(WeaponType.Hand, 0);
             weaponBulletNumDic.Add(WeaponType.Arms, 0);
             weaponBulletNumDic.Add(WeaponType.Smg, 0);
             weaponBulletNumDic.Add(WeaponType.Shot, 0);
             weaponBulletNumDic.Add(WeaponType.Sniper, 0);
            weaponBulletNumDic.Add(WeaponType.Rpg, 0);
            currentWeaponId = 1002;

            return this;
        }
       
    }
}
