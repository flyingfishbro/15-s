using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_CosmoSniper : Bullet
{
    
    protected override float damage => (gun as ChargingGun).CalChargingDamage(shotStartPoint, transform.position, chargingRatio);

    protected override float shockFigure => (gun as ChargingGun).CalShockFigure(shotStartPoint, transform.position, chargingRatio);

    public override void Shot(Vector3 shotDirection, float chargingRatio)
    {
        base.Shot(shotDirection, chargingRatio);
        transform.localScale = Vector3.one + Vector3.one * chargingRatio;
    }
}
