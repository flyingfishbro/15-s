using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletGun_MicroUzi : HoldingGun
{

    public ParticleSystem smokeEffect;

    protected override bool CanShot => base.CanShot && !isOverHeating;


    public float speed = 1;


    public float targetMaxRateOverTime;

    private float overHeating = 0;

    public float _MaxOverHeating = 0;


    public float reductionTime;


    public bool isOverHeating;



    private void Update()
    {
        var em = smokeEffect.emission;

        if (!isOverHeating)
        {
            if (shotInput)
            {
                overHeating += Time.deltaTime * speed;

            }
            else
            {
                overHeating -= Time.deltaTime * speed * _MaxOverHeating / reductionTime;

            }


            overHeating = Mathf.Clamp(overHeating, 0, _MaxOverHeating);



            isOverHeating = overHeating >= _MaxOverHeating;

        }
        else
        {
            overHeating -= Time.deltaTime * speed * _MaxOverHeating / reductionTime;

            overHeating = Mathf.Clamp(overHeating, 0, _MaxOverHeating);
            isOverHeating = overHeating > 0;
        }


        
        em.rateOverTime = overHeating / _MaxOverHeating * targetMaxRateOverTime;
    }




}
