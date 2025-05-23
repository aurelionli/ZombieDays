
using UnityEngine;

namespace FPS
{
    public class LadderClimbingController :MonoBehaviour ,IController
    {
        public Transform ladderTop; // 梯子顶部空物体
        private Transform playerCamera; // 玩家的摄像头
        public float lookThreshold = 0.5f; // 看向梯子的阈值（范围0到1）
        private IController IC;

        private void Awake()
        {
            IC = this;
        }
        private void Start()
        {
          
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                playerCamera = Camera.main.transform;
            }
        }
        private void OnTriggerStay(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                if (IsLookingAtLadder())
                {
                    //todo 告知，进入攀爬区域
                    IC.TriggerEvent("PlayerClimb", true);
                }
                else
                {
                    IC.TriggerEvent("PlayerClimb", false);
                }

            }
        }
        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                //todo 告知，进入攀爬区域
                IC.TriggerEvent("PlayerClimb", false);
            }
        }

        private bool IsLookingAtLadder()
        {

            // 计算玩家摄像头到梯子的方向
            Vector3 toLadder = (ladderTop.position - playerCamera.position).normalized;

            // 计算玩家摄像头的正前方
            Vector3 cameraForward = playerCamera.forward;

            // 计算点积，判断是否看向梯子
            float dot = Vector3.Dot(cameraForward, toLadder);

            // 如果点积大于阈值，则认为玩家看向梯子
            return dot > lookThreshold;
        }
       
    }
}
