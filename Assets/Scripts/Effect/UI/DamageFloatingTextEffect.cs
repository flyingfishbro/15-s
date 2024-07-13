using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageFloatingTextEffect : EffectObject
{
    private Animator _Animator;
    private Animator anim => _Animator ?? (_Animator = GetComponent<Animator>());


    private Text _Text;
    private Text text => _Text ?? (_Text = GetComponent<Text>());


    public override void Play(Vector3 pos) => Play(pos, 0, 0f);

    public override void Play(Vector3 pos, int option, float optionValue)
    {
        base.SetEffect(true, pos);

        anim.SetTrigger("_SetOn");



        text.text = optionValue.ToString();

        if (option == 0) //  Body
        {
            text.color = Color.white;
            text.fontSize = 55;
        }
        else // Head
        {
            text.color = Color.yellow;
            text.fontSize = 65;
        }

    }

    public override void Stop()
    {
        base.SetEffect(false);

    }
}
