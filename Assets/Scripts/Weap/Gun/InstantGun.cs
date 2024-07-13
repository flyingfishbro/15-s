using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InstantGun : BulletGun
{
    private bool lastShotInput = false;

    public override bool OnShot(bool set, out float delay, out bool shotMotion)
    {
        bool shotSet = set && !lastShotInput;
        lastShotInput = set;

        return base.OnShot(shotSet, out delay, out shotMotion);
    }

}
