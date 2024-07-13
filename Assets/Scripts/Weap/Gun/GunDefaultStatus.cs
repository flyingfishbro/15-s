using UnityEngine;


[CreateAssetMenu(fileName = "GunDefaultStatus", menuName = "Scriptable/GunDefaultStatus")]
public class GunDefaultStatus : ScriptableObject
{
    [Header("�Ÿ���� ������ ������(%)")]
    public float damageReductionRate;

    [Header("�Ÿ���� ������ �ִ밨����(%)")]
    public float maxDamageReductionRate;

    [Header("�ּ� ��ݰ�")]
    public float minShockFigure;

}
