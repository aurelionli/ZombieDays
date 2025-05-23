using System;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "UIPath", menuName = "Custom/UI/UIPath")]
[Serializable]
public class UIPath_SO : ScriptableObject
{
    [field: SerializeField] List<string> path;

}