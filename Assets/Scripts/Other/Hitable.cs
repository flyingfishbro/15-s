using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Hitable : MonoBehaviour
{
    public Guid ID {  get; private set; }

    public abstract bool OnDamaged(AttackIntend attackIntend, int option);

    public virtual void Initialized()
    {
        ID = Guid.NewGuid();
    }
}
