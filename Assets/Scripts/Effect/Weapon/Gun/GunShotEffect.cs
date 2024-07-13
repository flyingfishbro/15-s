using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GunShotEffect : EffectObject
{

    private ParticleSystem _Particle;
    private ParticleSystem particle => _Particle ?? (_Particle = GetComponent<ParticleSystem>());


    public override void Play(Vector3 pos)
    {
        SetEffect(true, pos);

        if (particle != null) particle.Play();

    }

    public override void Stop()
    {
        SetEffect(false);

        if (particle != null) particle.Stop();

    }

    public override void Play(Vector3 pos, int option, float optionValue) => Play(pos);
}
