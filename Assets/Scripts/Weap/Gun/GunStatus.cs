
using System;
using UnityEngine;

[System.Serializable]
public class GunStatus
{
    [Header("�⺻ ���ط�")]
    public float damage;

    [Header("�⺻ ��ݰ�")]
    public float shockWeight;

    [Header("���� �ӵ�")]
    public float attackSpeed;

    [Header("ġ��Ÿ ����ġ")]
    public float staggerWeight;


    [Header("�ִ�/�ּ� �ݵ�")]
    public float minRecoil;

    public float maxRecoil;


    [Header("�ִ�(��¡) ������")]
    public float maxDamage;

    [Header("�ִ�(��¡) ��ݰ�")]
    public float maxShockFigure;

    [Header("�ִ� ��¡�ð�")]
    public float maxChargingTime;

}
