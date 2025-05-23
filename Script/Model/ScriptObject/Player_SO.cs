using System;

using UnityEngine;


[CreateAssetMenu(fileName = "Player", menuName = "Custom/Characters/PlayerMove")]
[Serializable]
public class Player_SO :ScriptableObject
{
    [field: SerializeField] public PlayerMoveData MoveData { get; private set; }
    [field: SerializeField] public PlayerCameraData CameraData { get; private set; }

    [field: SerializeField] public ArmsAssaultRifle01Data AAR01Data { get; private set; }
    [field: SerializeField] public GeData AAR02Data { get; private set; }
    [field: SerializeField] public GeData AAR03Data { get; private set; }

    [field: SerializeField] public HandGun01Data HG01Data { get; private set; }

    [field: SerializeField] public HandGun01Data HG02Data { get; private set; }

    [field: SerializeField] public HandGun01Data HG03Data { get; private set; }

    [field: SerializeField] public HandGun01Data HG04Data { get; private set; }
    [field: SerializeField] public Sniper02Data S01Data { get; private set; }

    [field: SerializeField] public Sniper02Data S02Data { get; private set; }
    [field: SerializeField] public Sniper02Data S03Data { get; private set; }


    [field: SerializeField] public ShotGunData SData { get; private set; }

    [field: SerializeField] public GeData Geata { get; private set; }

    [field: SerializeField] public GeData SMG01Data { get; private set; }
    [field: SerializeField] public GeData SMG02Data { get; private set; }
    [field: SerializeField] public GeData SMG03Data { get; private set; }
    [field: SerializeField] public GeData SMG04Data { get; private set; }
    [field: SerializeField] public GeData SMG05Data { get; private set; }
}

