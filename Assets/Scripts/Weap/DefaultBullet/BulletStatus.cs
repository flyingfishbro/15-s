
using UnityEngine;

[System.Serializable]
public class BulletStatus
{
    [Header("총알 둘레")]
    public float bulletRadius;

    [Header("날아가는 속도")]
    public float speed;

    [Header("총알 무게값(중력)")]
    public float weight;

    [Header("총알 유지시간(ObjectPooling)")]
    public float lifeTime;

}
