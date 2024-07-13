using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitEffectManager : UnitPartial
{
    public ParticleSystem _JectpackEffect { get; private set; }
    public void SetJetpackEffect(ParticleSystem effect) => _JectpackEffect = effect;


    private void Start()
    {
        unit.state.isJump.AddSetStateStaticListener(true, OnJetpackEffect);
        unit.state.isJump.AddSetStateStaticListener(false, OffJetpackEffect);


        unit.state.isShield.AddSetStateStaticListener(true, OnShieldState);
        unit.state.isShield.AddSetStateStaticListener(false, OffShieldState);
    }




    public void OnJetpackEffect() => _JectpackEffect.Play();
    public void OffJetpackEffect() => _JectpackEffect.Stop();



    private void SetShield(bool set)
    {
        if (shieldEffectCo != null)
        {
            StopCoroutine(shieldEffectCo);
            shieldEffectCo = null;
        }


        if (set)
        {
            unit.shieldPos.gameObject.SetActive(true);

            StartShieldTimer();
            SetShieldEffect(true);


        }
        else
        {

            unit.shieldPos.GetComponent<Collider>().enabled = false;
            SetShieldEffect(false);


        }
    }

    private void OnShieldState() => SetShield(true);
    private void OffShieldState() => SetShield(false);



    private Coroutine shieldLifeTimeCo;

    private void StartShieldTimer()
    {
        if (shieldLifeTimeCo != null)
        {
            StopCoroutine(shieldLifeTimeCo);
            shieldLifeTimeCo = null;
        }

        shieldLifeTimeCo = StartCoroutine(ShieldLifeTimeCoroutine());
    }

    private IEnumerator ShieldLifeTimeCoroutine()
    {
        yield return new WaitForSeconds(Unit.unitDefaultStatus.shieldDuration);
        unit.state.isShield.SetState(false);
    }




    #region Shield Effect
    private Coroutine shieldEffectCo;

    private void SetShieldEffect(bool set)
    {
        if (shieldEffectCo != null)
        {
            StopCoroutine(shieldEffectCo);
            shieldEffectCo = null;
        }
        shieldEffectCo = StartCoroutine(ShieldEffectCoroutine(set ? SHIELD_ON_EFFECTVALUE : SHIELD_OFF_EFFECTVALUE));
    }

    private IEnumerator ShieldEffectCoroutine(float targetValue)
    {
        while (Mathf.Abs(shieldEffectValue - targetValue) > .1f)
        {
            yield return null;
            shieldEffectValue = Mathf.Lerp(shieldEffectValue, targetValue, setSpeed);

            SetShieldEffect(shieldEffectValue);
        }

        if (shieldEffectValue <= .1f) unit.shieldPos.gameObject.SetActive(false);
    }
    


    private Renderer _ShieldRenderer;
    private Renderer shieldRenderer => _ShieldRenderer ?? (_ShieldRenderer = unit.shieldPos.GetComponent<Renderer>());

    private float shieldEffectValue = 0;

    private float setSpeed = .05f;

    private const float SHIELD_ON_EFFECTVALUE = 3;
    private const float SHIELD_OFF_EFFECTVALUE = 0;

    private void SetShieldEffect(float value) => shieldRenderer.material.SetFloat("_FresnelPower", value);

    #endregion

}
