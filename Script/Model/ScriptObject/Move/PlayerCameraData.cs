using System;
using UnityEngine;

[Serializable]
public class PlayerCameraData
{
    [field: SerializeField] public float mouseSenstivity = 100f; // 鼠标灵敏度

    [field: SerializeField] public float walkShakeAmount = 0.5f;//滑动幅度
    [field: SerializeField] public float walkShakeFrequency = 10f;//晃动频率,越大频率越快

    [field: SerializeField] public float runShakeAmount = 0.5f;//滑动幅度
    [field: SerializeField] public float runShakeFrequency = 10f;//晃动频率,越大频率越快

    [field: SerializeField] public float crouchShakeAmount = 0.5f;//滑动幅度
    [field: SerializeField] public float crouchShakeFrequency = 10f;//晃动频率,越大频率越快

    [field: SerializeField] [Range(1f,0.01f)] public float mouseAimSenstivityMultpulity = 0.5f; // 瞄准鼠标灵敏度

    [field: SerializeField][Range(1f, 0.01f)] public float mouseGunSenstivityMultpulity = 0.5f; // 瞄准灵敏度


}