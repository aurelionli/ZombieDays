using UnityEngine;


namespace FPS
{
    public class PlayerInteractionController :MonoBehaviour,IController
    {
        private IController IC;
        [Header("Settings")]
        private float checkDistance = 3f;   //检测距离
        private int  interactLayer=1<<3;    //交互图层
        private GameObject pickupPrompt;    //捡枪提示UI
     

        private Camera playerCamera;

        private BaseInteractController currentWeapon;

        private int frameCount;

        private PlayerControls.PlayerActions playerActions;
        private void Awake()
        {
            IC =this;
        }
        private void Start()
        {
            playerActions=IC.GetPlayerControls();
            playerCamera = GameTools.GetSingleComponentInChild<Camera>(gameObject, "MainCamera");
        }
        private void Update()
        {

            HandlePickupInput();

            CheckWeaponInSight();
        }
        private void FixedUpdate()
        {
            
            
        }
        private void CheckWeaponInSight()
        {
            //. Camera.ViewportToWorldPoint:将视口坐标（Viewport Point）转换为世界坐标（World Point）喵~
            //pos：视口坐标（Vector3 类型），范围是 [0, 1]。depth：世界坐标的深度（Z轴值）。
            //返回世界空间中的坐标（Vector3 类型）。
            /*Vector3 worldPos = Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 10f));
            Debug.Log("屏幕中心的世界坐标: " + worldPos);*/

            //Camera.WorldToViewportPoint：将世界坐标（World Point）转换为视口坐标（Viewport Point）喵~
            //用于判断物体是否在屏幕内，或者计算物体在屏幕上的位置。
            /*Vector3 viewportPos = Camera.main.WorldToViewportPoint(transform.position);
            if (viewportPos.x >= 0 && viewportPos.x <= 1 && viewportPos.y >= 0 && viewportPos.y <= 1)
            {
                  Debug.Log("物体在屏幕内喵~");
            }*/

            //将屏幕的视口坐标（Viewport Point）转换为一条射线（Ray），用于射线检测喵~
            //pos：视口坐标（Vector3 类型），范围是 [0, 1]。(0, 0) 表示屏幕左下角。(1, 1) 表示屏幕右上角。
            //返回一条从相机出发的射线（Ray 类型）。
            Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, checkDistance, interactLayer))
            {
               // Debug.Log("检测到"+hit.collider.gameObject.name);
                currentWeapon = hit.collider.GetComponent<BaseInteractController>();
                currentWeapon.BeSelect();
                //这里会检测到目标图层
                /*Weapon weapon = hit.collider.GetComponent<Weapon>();
            if (weapon != null)
            {
                nearestWeapon = weapon;
                ShowPrompt(weapon);
                return;
            }*/
                return;
            }
            if(currentWeapon != null)
            {
                currentWeapon.UnSelect();
                currentWeapon = null;
            }
           

        }

        //这个是显示捡取提示
        private void ShowPrompt()
        {

        }
        //处理拾取输入
        private void HandlePickupInput()
        {
            if(currentWeapon != null&& playerActions.Interact.triggered)
            {
                Debug.Log("这里点击了交互按钮");
                currentWeapon.BePickUp();
            }
        }
    }
}
