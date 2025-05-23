

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace FPS_Manager
{
    [CreateAssetMenu(fileName = "Player", menuName = "Custom/Characters/UI")]
    [Serializable]
    public class UI_SO :ScriptableObject
    {
        [field: SerializeField]public List<WeaponUI> w = new List<WeaponUI>();
    }

    [Serializable]
    public class WeaponUI
    {
        [field: SerializeField] public int weaponID;
        [field: SerializeField] public Sprite weaponImage;
        [field: SerializeField] public Sprite casingBulletImage;
    }

}
