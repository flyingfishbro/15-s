
using UnityEngine;

[System.Serializable]
public class BulletStatus
{
    [Header("�Ѿ� �ѷ�")]
    public float bulletRadius;

    [Header("���ư��� �ӵ�")]
    public float speed;

    [Header("�Ѿ� ���԰�(�߷�)")]
    public float weight;

    [Header("�Ѿ� �����ð�(ObjectPooling)")]
    public float lifeTime;

}
