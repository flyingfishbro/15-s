using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitHitInterface : HitableInterface
{
    public enum HitBoxType
    {
        BODY = 0,
        HEAD = 1
    }

    public HitBoxType hitBoxType;

    public override bool OnDamaged(AttackIntend attackIntend) => base.OnDamaged(attackIntend, (int)hitBoxType);

}
