using System;
using UnityEngine;

[Serializable]
public class PlayerMoveData
{
    [field: SerializeField] public float walkSpeed { get; private set; } = 5f;
    [field: SerializeField] public float runSpeed { get; private set; } = 10f;
    [field: SerializeField] public float crouchSpeed { get; private set; } = 2.5f;
    [field: SerializeField] public float jumpForce { get; private set; } = 5f;
    [field: SerializeField] public float gravity { get; private set; } = -9.81f;
    [field: SerializeField] public float standHeight { get; private set; } = 2f;
    [field: SerializeField] public float crouchHeight { get; private set; } = 1f;
    [field: SerializeField] public float standCameraHeight { get; private set; } = 1.6f;
    [field: SerializeField] public float crouchCameraHeight { get; private set; } = 0.8f;
    [field: SerializeField] public float interpolationSpeed { get; private set; } = 5f;   //归位速度

    [field: SerializeField] public float climbingSpeed { get; private set; } = 5f;   //归位速度


}