
using System.Collections;
using UnityEngine;

namespace FPS
{
    public class GetAmmoController : BaseInteractController
    {
        public WeaponType Type;
        private bool isOpen;

        public int bulletNum;
        public Transform toOpen;
        public Transform beOpen;


        protected override void Awake()
        {
            base.Awake();
            toOpen = transform.GetChild(1);
            beOpen = transform.GetChild(2);

            beOpen.gameObject.SetActive(false);
        }
        private void OnEnable()
        {
            isOpen = false;
            //body=GetComponent<Rigidbody>();
         //   StartCoroutine(Open());
        }
        private void OnDisable()
        {
            StopAllCoroutines();
        }
        IEnumerator Destory()
        {
            yield return new WaitForSeconds(200f);
            IC.ReturnObject(gameObject.name,gameObject);
        }
        public override void BeSelect()
        {
            if (isOpen)
            {
              //  Debug.Log("已经打开过子弹");
                return;
            }
            base.BeSelect();
        }
        public override void BePickUp()
        {
            if (isOpen)
            {
               // Debug.Log("已经打开过子弹");
                return;
            }
            isOpen = true;
            toOpen.gameObject.SetActive(false);
            beOpen.gameObject.SetActive(true);
            IC.OpenPanel<MessagePanel>(true, $"Player Get {Type}Ammo");
            GetBulletCommandData temp = new GetBulletCommandData();
            temp.num = bulletNum;
            temp.type = Type;
            IC.SendCommand<GetBulletCommand>(temp);
            Debug.Log("获得子弹");
        }
    }
}
