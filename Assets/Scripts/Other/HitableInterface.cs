using UnityEngine;

public abstract class HitableInterface : MonoBehaviour
{
    public Hitable _Hitable;
    public Hitable hitable => _Hitable ?? (_Hitable = GetComponentInParent<Hitable>());

    public virtual bool OnDamaged(AttackIntend attackIntend) => OnDamaged(attackIntend, 0);

    public virtual bool OnDamaged(AttackIntend attackIntend, int option) => hitable.OnDamaged(attackIntend, option);
}
