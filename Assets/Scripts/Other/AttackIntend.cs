
using UnityEngine;

public struct AttackIntend
{

    public float damage { get; private set; }

    public Vector3 hitPos { get; private set; }

    public float shockFigure { get; private set; }

    public float weight { get; private set; }

    public float staggerPercent { get; private set; }

    public AttackIntend(float dmg, Vector3 pos, float shockWeight, float weight, float staggerPercent)
    {
        this.damage = dmg;
        this.hitPos = pos;
        this.shockFigure = shockWeight;
        this.weight = weight;
        this.staggerPercent = staggerPercent;
    }

}

