using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BulletDefaultStatus", menuName = "Scriptable/BulletDefaultStatus")]
public class BulletDefaultStatus : ScriptableObject
{
    //총알이 이동할떄 콜리션 검사대상 레이어를 정의합니다.

    [Header("피격가능한 구조체 레이어")]
    public LayerMask structLayer;

    [Header("피격 가능한 피격물(유닛, 폭발물) 레이어")]
    public LayerMask hitableLayer;
}
