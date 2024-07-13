using System;
using UnityEngine;

[Serializable]
public class GunInfo 
{
    [Header("�� �ڵ�")]
    public string id;

    [Header("�� �̹���")]
    public Sprite sprite;

    [Header("��� ���")]
    public Gun.ShotType type;

    [Header("�� �̸�")]
    public string gunName;

    [Header("�� ����")]
    public GunStatus gunStatus;

    [Header("�ҷ� ����")]
    public BulletStatus bulletStatus;

}
