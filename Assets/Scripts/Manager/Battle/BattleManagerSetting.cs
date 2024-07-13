using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BattleManagerSetting", menuName = "Scriptable/BattleManagerSetting")]
public class BattleManagerSetting : ScriptableObject
{
    [Header("���� ����Ʈ �Ÿ����Դϴ�.")]
    public float DefaultUnitBaseDistance = 6;

    public float DefaultUnitMaxDistance = 9;

    public float DefaultUnitMinDistance = 5;


    [Header("�߷°��� ������� �ʴ� ��Ż ���� ����Ʈ �Ÿ����Դϴ�.")]
    public float TotalUnitMaxDistance = 12;

}


