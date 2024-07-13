using UnityEngine;


[CreateAssetMenu(fileName = "GunDefaultStatus", menuName = "Scriptable/GunDefaultStatus")]
public class GunDefaultStatus : ScriptableObject
{
    [Header("거리비례 데미지 감소율(%)")]
    public float damageReductionRate;

    [Header("거리비례 데미지 최대감소율(%)")]
    public float maxDamageReductionRate;

    [Header("최소 충격값")]
    public float minShockFigure;

}
