using UnityEngine;

[CreateAssetMenu(fileName = "UnitDefaultStatus", menuName = "Scriptable/UnitDefaultStatus")]
public class UnitDefaultStatus : ScriptableObject
{
    [Header("최대체력")]
    public float maxHp;

    [Header("다가가는 힘")]
    public float toCloseJumpPower;

    [Header("멀어지는 힘")]
    public float toFarJumpPower;


    [Header("최대/최소 에임각도")]
    public float maxAimRot;
    public float minAimRot;


    [Header("헤드샷 판정 추가데미지 비율")]
    public float headShotAddDamage;

    [Header("쉴드 지속시간")]
    public float shieldDuration;


    [Header("Stagger판정 부위별 퍼센트")]
    public float staggerPer_Head;

    public float staggerPer_Body;





    public LayerMask unitLayer;

    public LayerMask unitHitLayer;

}
