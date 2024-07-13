using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BulletDefaultStatus", menuName = "Scriptable/BulletDefaultStatus")]
public class BulletDefaultStatus : ScriptableObject
{
    //�Ѿ��� �̵��ҋ� �ݸ��� �˻��� ���̾ �����մϴ�.

    [Header("�ǰݰ����� ����ü ���̾�")]
    public LayerMask structLayer;

    [Header("�ǰ� ������ �ǰݹ�(����, ���߹�) ���̾�")]
    public LayerMask hitableLayer;
}
