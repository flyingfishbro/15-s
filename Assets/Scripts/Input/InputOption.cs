using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "InputOptionScriptable", menuName = "Scriptable/InputOptionScriptable")]
public class InputOption : ScriptableObject
{
    [Header("에임 감도")]
    public float aimInputWeight;
}
