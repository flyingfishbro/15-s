using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletCollisionEffect : EffectObject
{

    private ParticleSystem _Particle;
    private ParticleSystem particle => _Particle ?? (_Particle = GetComponent<ParticleSystem>());

    public override void Play(Vector3 pos)
    {
        SetEffect(true, pos);

        particle.Play();


    }

    public override void Play(Vector3 pos, int option, float optionValue) => Play(pos);

    public override void Stop()
    {
        SetEffect(false);

        particle.Stop();

    }
}
