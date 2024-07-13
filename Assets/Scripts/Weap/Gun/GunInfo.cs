using System;
using UnityEngine;

[Serializable]
public class GunInfo 
{
    [Header("건 코드")]
    public string id;

    [Header("건 이미지")]
    public Sprite sprite;

    [Header("사격 방식")]
    public Gun.ShotType type;

    [Header("건 이름")]
    public string gunName;

    [Header("건 스탯")]
    public GunStatus gunStatus;

    [Header("불렛 스탯")]
    public BulletStatus bulletStatus;

}
