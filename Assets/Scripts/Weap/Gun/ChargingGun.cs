using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class ChargingGun : BulletGun
{
    protected const float DEFAULT_MIN_CHARGINGVALUE = .5f;


    private bool lastShotInput;

    protected float chargingValue { get; private set; } 

    /// <summary>
    /// 기본 차징시간 미만은 기본샷으로 인정하고, 해당 시간 이상부터는 최대차징의 비율값을 반환합니다.
    /// </summary>
    /// <param name="chargingValue"></param>
    /// <returns></returns>
    protected float CalChargingRatio(float chargingValue)
    {
        float ret = (chargingValue - DEFAULT_MIN_CHARGINGVALUE);
        ret = ret < 0 ? 0 : ret;

        return Mathf.Clamp01(ret / (status.maxChargingTime - DEFAULT_MIN_CHARGINGVALUE));
    }



    public virtual float CalChargingDamage(Vector3 shotPos, Vector3 collisionPos, float chargingRatio)
    {
        float defDamage = base.CalDamage(shotPos, collisionPos);

        float sub = status.maxDamage - status.damage;
        sub = sub < 0 ? 0 : sub;

        defDamage += sub * chargingRatio;

        return defDamage;
    }



    public float CalShockFigure(Vector3 shotPos, Vector3 collisionPos, float chargingRatio)
    {
        float shockWeight = status.shockWeight + (status.maxShockFigure - status.shockWeight) * chargingRatio;
        
        return shockWeight * (1 - Vector3.Distance(shotPos, collisionPos) * .025f);
    }





    protected override void BulletShot(Bullet bullet, Vector3 shotDirection)
    {
        bullet.Shot(shotDirection, CalChargingRatio(chargingValue));
        chargingValue = 0;
    }



    public override bool OnShot(bool set, out float delay, out bool shotMotion)
    {
        bool shotSet = !set && lastShotInput;
        lastShotInput = set;

        if (set)
        {
            chargingValue += Time.deltaTime;

            //print($"{Mathf.Round(chargingValue - DEFAULT_MIN_CHARGINGVALUE / status.maxChargingTime + DEFAULT_MIN_CHARGINGVALUE))}");
        }

        


        return base.OnShot(shotSet, out delay, out shotMotion);
    }

}
