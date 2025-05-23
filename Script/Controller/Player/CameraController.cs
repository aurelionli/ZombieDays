using DG.Tweening;

using UnityEngine;



namespace FPS
{
    
    public class CameraController : MonoBehaviour,IController
    {
        public Transform mainCamera; // 主相机
        public Transform gunCamera;  // 枪械相机
                                     //    public Control control;     // 控制脚本
        public PlayerControls.PlayerActions playerActions;
      
 

        //private float yRotation;    // 上下旋转角度
       // private float xRotation;    // 左右旋转角度

        public PlayerController pC;
        private PlayerCameraData data;

       // public float mouseSenstivity = 100f; // 鼠标灵敏度

        

        public IController IC;

        Vector2 mouseInput;
        Vector2 moveInput;

        //gunCamera的参数
        private float gunSmoothTime = 0.1f;
        private Vector3 targetOffset = Vector3.zero;//目标偏移
        private Vector3 currentOffset = Vector3.zero;   //当前偏移
        private Vector3 velocity = Vector3.zero;//速度
        private float mouseShakeSenstivity = 0.001f;

        //mainCamera的参数
        private float shakeAmount = 0.5f;//滑动幅度
        private float shakeFrequency = 10f;//晃动频率,越大频率越快

        private bool isOpenWeaponChangeUI;

        private PlayerReuseableData reuseableData;


        private Tween shakeTween;


        private float yRotation;
        private float xRotation;
        private float currentYRotation;
        private float currentXRotation;
        public float smoothTime = 0.01f;
        private void Awake()
        {
            isOpenWeaponChangeUI = false;
            IC = this;

            IC.RegisterEvent("SniperAim");
            IC.RegisterEvent("NorAim");
            IC.RegisterEvent("SwitchGunCamera");
            IC.RegisterEvent("PlayerBeHurt");
            IC.StartListening("SwitchGunCamera", TriggerEvent_SwitchGunCamera);
        }
        
        private void Start()
        {
            reuseableData = FindObjectOfType<PlayerController>().reuseableData;
            pC = GetComponent<PlayerController>();
            data = pC.player_SO.CameraData;
            playerActions = IC.GetPlayerControls();
            // 确保只有主相机启用Audio Listener

            Debug.Log("摄像机控制类监听事件");
            IC.StartListening("OpenChangWeaponPanel", TriggerEvent_OpenChangWeaponPanel);
            IC.StartListening("SniperAim", TriggerEvent_SniperAim);
            IC.StartListening("NorAim", TriggerEvent_AimAboutNormalCamera);
            IC.StartListening("PlayerBeHurt",TriggerEvent_PlayerBeHurt);
      
            
            playerActions.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
            playerActions.Move.canceled += ctx => moveInput = Vector2.zero;
        }
        private void OnDestroy()
        {
            IC.StopListening("OpenChangWeaponPanel", TriggerEvent_OpenChangWeaponPanel);
            IC.StopListening("SniperAim", TriggerEvent_SniperAim);
            IC.StopListening("NorAim", TriggerEvent_AimAboutNormalCamera);
            IC.StopListening("SwitchGunCamera", TriggerEvent_SwitchGunCamera);
            IC.StopListening("PlayerBeHurt", TriggerEvent_PlayerBeHurt);
            IC.UnRegisterEvent("SwitchGunCamera");
            IC.UnRegisterEvent("SniperAim");
            IC.UnRegisterEvent("PlayerBeHurt");
            IC.UnRegisterEvent("NorAim");
        }
        private void TriggerEvent_PlayerBeHurt(object obj)
        {
            shakeTween?.Kill();
            shakeTween= mainCamera.DOShakePosition(1, new Vector3(0.3f, 0.3f, 0));
        }
        private void TriggerEvent_SwitchGunCamera(object obj)
        {
           
              gunCamera = (Transform)obj;
            
        }

        private void TriggerEvent_OpenChangWeaponPanel(object data)
        {
            isOpenWeaponChangeUI = !isOpenWeaponChangeUI;
        }
        private void TriggerEvent_SniperAim(object data)
        {
            float i = (float)data;
            DOTween.To(() => gunCamera.GetComponent<Camera>().fieldOfView, x => gunCamera.GetComponent<Camera>().fieldOfView = x, i, 0.2f);
        }
        private void TriggerEvent_AimAboutNormalCamera(object data)
        {
            float i = (float)data;
            DOTween.To(() => mainCamera.GetComponent<Camera>().fieldOfView, x => mainCamera.GetComponent<Camera>().fieldOfView = x, i, 0.2f);
        }
        private void LateUpdate()
        {
            updataView();
        }
        public void updataView()
        {
            // 获取鼠标输入
          mouseInput = playerActions.Look.ReadValue<Vector2>();

            // 更新相机转向
            UpdateCameraRotation();

            // 更新枪械相机偏移
            UpdateGunCameraOffset();

            // 更新主相机摇晃
            UpdateMainCameraShake();
        }
        
        private void UpdateCameraRotation()
        {
            if (isOpenWeaponChangeUI) return;

            // 灵敏度系数
            float multipule = reuseableData.aiming ? data.mouseAimSenstivityMultpulity : 1f; // 假设瞄准灵敏度为30%

            // 输入处理（带死区）
            float deadZone = 0f;
            float mouseY = Mathf.Abs(mouseInput.y) > deadZone ? mouseInput.y : 0;
            float mouseX = Mathf.Abs(mouseInput.x) > deadZone ? mouseInput.x : 0;

            // 计算目标角度
            yRotation -= mouseY * data.mouseSenstivity * multipule * Time.deltaTime;
            xRotation += mouseX * data.mouseSenstivity * multipule * Time.deltaTime;
            yRotation = Mathf.Clamp(yRotation, -60f, 60f);

            // 平滑过渡
            currentYRotation = Mathf.Lerp(currentYRotation, yRotation, Time.deltaTime / smoothTime);
            currentXRotation = Mathf.Lerp(currentXRotation, xRotation, Time.deltaTime / smoothTime);

            // 应用旋转
            transform.GetChild(0).localRotation = Quaternion.Euler(currentYRotation, 0, 0); // 摄像机上下
            transform.rotation = Quaternion.Euler(0, currentXRotation, 0); // 身体左右

            /*float multipule = reuseableData.aiming ? data.mouseAimSenstivityMultpulity : 1f;
            //旋转是本体，上下是身体和相机。
            if (isOpenWeaponChangeUI) { return; }
            yRotation -= mouseInput.y * data.mouseSenstivity* multipule * Time.deltaTime;
            xRotation = mouseInput.x * data.mouseSenstivity* multipule * Time.deltaTime;
            yRotation = Mathf.Clamp(yRotation, -60, 60f);
         
            transform.GetChild(0).localRotation = Quaternion.Euler(yRotation, 0, 0);//这是位置
            transform.Rotate(xRotation * Vector3.up);//这是增量
            */
            //  if (!control.resuableData.isOpenUI)
            /*  {
                  // 上下旋转
                  yRotation -= mouseInput.y * mouseSenstivity * Time.deltaTime;
                  yRotation = Mathf.Clamp(yRotation, -60f, 60f); // 限制上下旋转角度
                  transform.localRotation = Quaternion.Euler(yRotation, 0, 0);

                  // 左右旋转
                  xRotation = mouseInput.x * mouseSenstivity * Time.deltaTime;
                  transform.Rotate(xRotation * Vector3.up);
              }*/
        }

        private void UpdateGunCameraOffset()
        {
            if(gunCamera==null)
            {
                Debug.Log("摄像机为空");
                return;
            }

            float multipule = reuseableData.aiming?data.mouseGunSenstivityMultpulity :1f;

            
            // 计算目标偏移
            targetOffset.x += mouseInput.x * mouseShakeSenstivity* multipule;
            targetOffset.z += mouseInput.y * mouseShakeSenstivity* multipule;
            targetOffset.x = Mathf.Clamp(targetOffset.x, -0.05f, 0.05f);
            targetOffset.z = Mathf.Clamp(targetOffset.z, -0.05f, 0.05f);

            // 平滑过渡到目标偏移
            currentOffset = Vector3.SmoothDamp(currentOffset, targetOffset, ref velocity, gunSmoothTime);
            gunCamera.localPosition = currentOffset;

            // 逐渐恢复偏移到零
            targetOffset = Vector3.Lerp(targetOffset, Vector3.zero, Time.deltaTime * (1 / gunSmoothTime));
        }

        private void UpdateMainCameraShake()
        {
            if (moveInput != Vector2.zero)
            {
                switch (pC.reuseableData.state)
                {
                    case MovementState.WALK:
                        shakeAmount = data.walkShakeAmount;
                        shakeFrequency = data.walkShakeFrequency;
                        break;
                    case MovementState.RUN:
                        shakeAmount = data.runShakeAmount;
                        shakeFrequency = data.runShakeFrequency;
                        break;
                    case MovementState.CROUCH:
                        shakeAmount = data.crouchShakeAmount;
                        shakeFrequency = data.crouchShakeFrequency;
                        break;
                    case MovementState.JUMP:
                        shakeAmount = data.walkShakeAmount;
                        shakeFrequency = data.walkShakeFrequency;
                        break;
                }
                // 计算摇晃偏移
                float horizontalShake = shakeAmount * Mathf.Sin(Time.time * shakeFrequency);
                float verticalShake = shakeAmount * Mathf.Sin(Time.time * shakeFrequency);
                Quaternion shakeOffset = Quaternion.Euler(verticalShake, horizontalShake, 0);

                // 应用摇晃
                mainCamera.localRotation = Quaternion.identity * shakeOffset;
            }
            else
            {
                // 逐渐恢复相机旋转到默认状态
                mainCamera.localRotation = Quaternion.Lerp(mainCamera.localRotation, Quaternion.identity, Time.deltaTime * (1 / smoothTime));
            }
        }
    }
}
