using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BattleManagerSetting", menuName = "Scriptable/BattleManagerSetting")]
public class BattleManagerSetting : ScriptableObject
{
    [Header("유닛 디폴트 거리값입니다.")]
    public float DefaultUnitBaseDistance = 6;

    public float DefaultUnitMaxDistance = 9;

    public float DefaultUnitMinDistance = 5;


    [Header("중력값의 영향받지 않는 토탈 유닛 디폴트 거리값입니다.")]
    public float TotalUnitMaxDistance = 12;

}


