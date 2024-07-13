using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletGun_CosmoSniper : ChargingGun
{
    public ParticleSystem[] _ChargingEffects;
    public float[] targetChargingEffectValue;


    public void Update()
    {
        if (!user.status.IsAlive)
        {
            SetChargingEffect(CalChargingRatio(0));
            enabled = false;
            return;
        }
        else
        {
            SetChargingEffect(CalChargingRatio(chargingValue));

        }


    }

    private void SetChargingEffect(float ratio)
    {
        for (int i = 0; i < _ChargingEffects.Length; ++i)
        {
            var em = _ChargingEffects[i].emission;
            em.rateOverTime = targetChargingEffectValue[i] * ratio;
        }
    }
}
