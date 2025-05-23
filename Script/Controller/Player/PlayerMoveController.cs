
using DG.Tweening;
using FPS_Manager;
using UnityEngine;
using UnityEngine.InputSystem;
namespace FPS
{
    public class PlayerMoveController : MonoBehaviour, IController
    {
        public IController IC;
       // public enum MovementState { WALK, RUN, CROUCH, JUMP, IDLE }

        public PlayerController pC;
        // 组件引用
        private CharacterController characterController;
        private AudioSource audioSource;
        private Transform cameraC;

        
        public PlayerMoveData data;
        // 移动参数
        public LayerMask crouchLayerMask;

        // 状态变量
       // public MovementState state;
        private bool isGround;
        private bool isCanCrouch;
        private float currentHeight;
        private float cameraCurrentHeight;
        private Vector3 velocity; // 用于重力和跳跃的速度


        // 新输入系统
         private PlayerControls.PlayerActions playerControls;
        private Vector2 moveInput;

        private float time;//记录当前时间
        
        private void Awake() {
            IC = this;
            playerControls = IC.GetPlayerControls();
            // 初始化新输入系统
            playerControls.Move.started += OnMoveStart;
            playerControls.Move.performed += OnMovingPerform;
            playerControls.Move.canceled += OnMoveCanceled;
            IC.RegisterEvent("PlayerClimb");
            IC.StartListening("PlayerClimb", EventTrigger_ClimbingUp);
        }
        private void OnMoveStart(InputAction.CallbackContext context) {
            IC.PlayerStateAudioSource("Walking");
        }
        private void OnMovingPerform(InputAction.CallbackContext context) {
            moveInput = context.ReadValue<Vector2>();
        }
        // 处理Move.canceled事件
        private void OnMoveCanceled(InputAction.CallbackContext context) {
            moveInput = Vector2.zero;
            DOTween.To(() => pC.reuseableData.moveSpeed, x => pC.reuseableData.moveSpeed = x, 0f, 0.1f);
            IC.StopStateAudioSource("Walking");
        }
        private void OnDestory()
        {
            IC.StopListening("PlayerClimb", EventTrigger_ClimbingUp);
            IC.UnRegisterEvent("PlayerClimb");
          
        }
        private void EventTrigger_ClimbingUp(object data)
        {
            bool value = (bool)data;
            if (value)
            {
                pC.reuseableData.state = MovementState.CLIMB;
            }
            else
            {
                pC.reuseableData.state = MovementState.IDLE;
            }
        }

        private void OnDestroy()
        {
            playerControls.Move.started -= OnMoveStart;
            playerControls.Move.performed -= OnMovingPerform;
            playerControls.Move.canceled -= OnMoveCanceled;
            IC.UnRegisterStateAudioSource("Walking");
        }

        private void Start()
        {
            pC = GetComponent<PlayerController>();
            data = pC.player_SO.MoveData;

            characterController = GetComponent<CharacterController>();
            audioSource = GetComponent<AudioSource>();
            cameraC = transform.GetChild(0).transform;

            currentHeight = data.standHeight;
            cameraCurrentHeight = data.standCameraHeight;
            isGround = true;
           pC.reuseableData.state = MovementState.IDLE; // 初始状态为空闲
            velocity = Vector3.zero; // 初始化速度

            //IC.RegisterStateAudioSource("Running");
            IC.RegisterStateAudioSource(gameObject,"Walking");
        }
        private void OnMovePerformed(InputAction.CallbackContext context)
        {
            moveInput = context.ReadValue<Vector2>();

        }

        private void Update()
        {
            if (pC.reuseableData.state == MovementState.CLIMB)
            {
                HandleClimbing();
                return;
            }
            CheckGround();
            CanCrouch();
            HandleState();
            HandleMovement();
          
            //HandleFootSound();
            //PlaySound();

        }

        

        // 检测是否在地面
        private void CheckGround()
        {
            isGround = characterController.isGrounded;
            if (isGround && velocity.y < 0)
            {
                velocity.y = -2f; // 轻微向下速度，确保角色贴地
            }
        }

        // 检测是否可以下蹲
        private void CanCrouch()
        {
            Vector3 sphere = transform.position + Vector3.up * currentHeight;
            isCanCrouch = Physics.OverlapSphere(sphere, characterController.radius, crouchLayerMask).Length == 0;
        }

        // 处理状态切换
        private void HandleState()
        {
            
            // 跳跃优先级最高
            if (playerControls.Jump.triggered && isGround && isCanCrouch)
            {
                pC.reuseableData.state = MovementState.JUMP;
                velocity.y = Mathf.Sqrt(data.jumpForce * -2f * data.gravity); // 计算跳跃初速度
                return;
            }

            // 下蹲优先级次高
            if (playerControls.Crouch.ReadValue<float>() > 0 && isCanCrouch)
            {
                pC.reuseableData.state = MovementState.CROUCH;
                return;
            }

            // 跑步优先级高于走路
            if (playerControls.Run.ReadValue<float>() > 0 && IC.GetModel<PlayerCurrentStateModel>().currentEnergy >= 0&&moveInput.y>0&& !pC.reuseableData.cantRun)
            {
                pC.reuseableData.state = MovementState.RUN;
                if (time +0.1f <= Time.time)
                {
                    time = Time.time;
                    IC.SendCommand<DecreasePlayerEnergyCommand>(1f);
                }
                return;
            }

            // 走路优先级最低
            if (moveInput.magnitude > 0)
            {
              
                pC.reuseableData.state = MovementState.WALK;
                return;
            }

            // 默认状态为空闲
            pC.reuseableData.state = MovementState.IDLE;
        }

        // 处理移动逻辑
        private void HandleMovement()
        {
            IC.SendCommand<SetPlayerCurrentStateCommand>(pC.reuseableData.state);
            // 获取输入
            Vector3 moveDirection = (transform.right * moveInput.x + transform.forward * moveInput.y).normalized;

            // 根据状态设置速度

        
            switch (pC.reuseableData.state)
            {
                case MovementState.WALK:
                    DOTween.To(() => pC.reuseableData.moveSpeed, x => pC.reuseableData.moveSpeed = x, data.walkSpeed, 0.1f);
                    IC.SetPlayerStateAudioSource("Walking", 0.5f);
                    // pC.reuseableData.moveSpeed = data.walkSpeed;
                    break;
                case MovementState.RUN:
                  
                    DOTween.To(() => pC.reuseableData.moveSpeed, x => pC.reuseableData.moveSpeed = x, data.runSpeed, 0.1f);
                    IC.SetPlayerStateAudioSource("Walking", 1f);
                    break;
                case MovementState.CROUCH:
                    DOTween.To(() => pC.reuseableData.moveSpeed, x => pC.reuseableData.moveSpeed = x, data.crouchSpeed, 0.1f);
                    IC.SetPlayerStateAudioSource("Walking", 0.2f);
                    break;
                case MovementState.JUMP:
                    // Debug.Log("跳跃这一侧"+pC.reuseableData.moveSpeed);
             
                    //DOTween.To(() => pC.reuseableData.moveSpeed, x => pC.reuseableData.moveSpeed = x, data.walkSpeed, 0.1f); // 跳跃时保持行走速度
                    break;
            }
            

            // 应用水平移动
            characterController.Move(moveDirection * pC.reuseableData.moveSpeed  * Time.deltaTime);

            // 处理重力和跳跃
            if (!isGround)
            {
                velocity.y += data.gravity * Time.deltaTime; // 应用重力
            }
            characterController.Move(velocity * Time.deltaTime); // 应用垂直速度

            // 处理下蹲
            if (pC.reuseableData.state == MovementState.CROUCH)
            {
                currentHeight = data.crouchHeight;
                characterController.height = data.crouchHeight;
                characterController.center = new Vector3(0, data.crouchHeight / 2, 0);
                cameraCurrentHeight = Mathf.Lerp(cameraCurrentHeight, data.crouchCameraHeight, data.interpolationSpeed * Time.deltaTime);
            }
            else
            {
                currentHeight = data.standHeight;
                characterController.height = data.standHeight;
                characterController.center = new Vector3(0, data.standHeight / 2, 0);
                cameraCurrentHeight = Mathf.Lerp(cameraCurrentHeight, data.standCameraHeight, data.interpolationSpeed * Time.deltaTime);
            }

            // 更新摄像机高度
            cameraC.transform.localPosition = new Vector3(0, cameraCurrentHeight, 0);
        }

        private void HandleClimbing()
        {
            if (pC.reuseableData.state==MovementState.CLIMB)
            {
                

                if (  Input.GetKey(KeyCode.W)) // 抬头且按W键
                {
                    transform.Translate(Vector3.up * data.climbingSpeed * Time.deltaTime);
                }
                else if (Input.GetKey(KeyCode.S)) // 按S键
                {
                    transform.Translate(Vector3.down * data.climbingSpeed * Time.deltaTime);
                }
            }
        }
  
    }
}