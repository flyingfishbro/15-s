
using System;
using UnityEngine;

[System.Serializable]
public class GunStatus
{
    [Header("기본 피해량")]
    public float damage;

    [Header("기본 충격값")]
    public float shockWeight;

    [Header("공격 속도")]
    public float attackSpeed;

    [Header("치명타 가중치")]
    public float staggerWeight;


    [Header("최대/최소 반동")]
    public float minRecoil;

    public float maxRecoil;


    [Header("최대(차징) 데미지")]
    public float maxDamage;

    [Header("최대(차징) 충격값")]
    public float maxShockFigure;

    [Header("최대 차징시간")]
    public float maxChargingTime;

}
