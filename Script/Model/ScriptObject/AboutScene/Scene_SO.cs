using System;

using UnityEngine;


[CreateAssetMenu(fileName = "Player", menuName = "Custom/Characters/Scene")]
[Serializable]
public class Scene_SO : ScriptableObject
{
    [field: SerializeField] public int ShowSpeed { get; private set; }
    [field: SerializeField] public int HideSpeed { get; private set; }
    [field: SerializeField] public int CheckSpeed { get; private set; }
    [field: SerializeField] public float waitTime { get; private set; }

    [field: SerializeField] public Vector3 Fanwei { get; private set; }

    [field: SerializeField] public Vector3 spawnPoint { get; private set; }

    [field: SerializeField] public Vector3 rangePoint{ get; private set; }

}
