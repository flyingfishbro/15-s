using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class HoldingGun : BulletGun
{
    protected override bool hasShotMotion => false;

    protected bool shotInput = false;

    public override bool OnShot(bool set, out float delay, out bool shotMotion)
    {
        shotInput = set;
        return base.OnShot(set, out delay, out shotMotion);
    }

}
