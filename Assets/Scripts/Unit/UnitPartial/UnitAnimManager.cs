using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitAnimManager : UnitPartial
{
    private Animator _Anim;
    private Animator anim => _Anim ?? (_Anim = GetComponent<Animator>());

    #region Animator Origin Func
    public void SetTrigger(string str) => anim.SetTrigger(str);

    public float GetFloat(string str) => anim.GetFloat(str);

    public void SetFloat(string str, float value) => anim.SetFloat(str, value);

    public void SetFloat(string str, float value, float lerpSpeed) => anim.SetFloat(str, Mathf.Lerp(anim.GetFloat(str), value, lerpSpeed));

    #endregion


    public void SetAimRot(float value)
    {
        anim.SetFloat("_AimRot", Mathf.Clamp(value, -1, 1));
    }


    private void Update()
    {

        SetShotWeight();


    }

    /// <summary>
    /// 샷 웨이트값을 조절합니다. 값이 1에 가까워질수록 반동의 의한 사격 모션을 취합니다.
    /// </summary>
    private void SetShotWeight()
    {
        if (unit.state.isShotMotion.state)     SetFloat("_ShotWeight", 1, .7f);
        else                                    SetFloat("_ShotWeight", 0, .05f);

    }


    public void OnDeathMotion()
    {
        anim.SetTrigger("_DeathToBack");
    }

    public void SetWaitMotion(bool set)
    {
        anim.SetTrigger(set? "_ChickenDance" : "_Aim");
    }


}
