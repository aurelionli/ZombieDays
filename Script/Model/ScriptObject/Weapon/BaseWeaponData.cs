using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[Serializable]
public class BaseWeaponData
{
    

    [Tooltip("瞄准的size")][field: SerializeField] public float aimGunCameraView { get; private set; } = 15;//射程

    [Tooltip("瞄准的size")][field: SerializeField] public float normalGunCameraView { get; private set; } = 40;//射程
    [Tooltip("射程")][field: SerializeField] public float bulletForce { get; private set; }//射程

    [Tooltip("射速")][field: SerializeField] public float fireRate { get; private set; }

    [Tooltip("一梭子多少发")][field: SerializeField] public int bulletMag { get; private set; }

    [Tooltip("伤害")][field: SerializeField] public int damage { get; private set; }

    

}

