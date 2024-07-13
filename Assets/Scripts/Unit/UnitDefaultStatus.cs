using UnityEngine;

[CreateAssetMenu(fileName = "UnitDefaultStatus", menuName = "Scriptable/UnitDefaultStatus")]
public class UnitDefaultStatus : ScriptableObject
{
    [Header("�ִ�ü��")]
    public float maxHp;

    [Header("�ٰ����� ��")]
    public float toCloseJumpPower;

    [Header("�־����� ��")]
    public float toFarJumpPower;


    [Header("�ִ�/�ּ� ���Ӱ���")]
    public float maxAimRot;
    public float minAimRot;


    [Header("��弦 ���� �߰������� ����")]
    public float headShotAddDamage;

    [Header("���� ���ӽð�")]
    public float shieldDuration;


    [Header("Stagger���� ������ �ۼ�Ʈ")]
    public float staggerPer_Head;

    public float staggerPer_Body;





    public LayerMask unitLayer;

    public LayerMask unitHitLayer;

}
