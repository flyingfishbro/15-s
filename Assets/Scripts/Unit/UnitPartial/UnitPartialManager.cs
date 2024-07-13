using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitPartialManager : MonoBehaviour
{
    public Unit unit => GetComponentInParent<Unit>();


    private UnitAnimManager _AnimManager;
    public UnitAnimManager animManager => _AnimManager ?? (_AnimManager = unit.GetComponentInChildren<UnitAnimManager>());


    private UnitAimManager _AimManager;
    public UnitAimManager aimManager => _AimManager ?? (_AimManager = GetComponent<UnitAimManager>());


    private UnitAttackProcess _AttackProcess;
    public UnitAttackProcess attackProcess => _AttackProcess ?? (_AttackProcess = GetComponent<UnitAttackProcess>());


    private UnitHitProcess _HitProcess;
    public UnitHitProcess hitProcess => _HitProcess ?? (_HitProcess = GetComponent<UnitHitProcess>());


    private UnitEffectManager _EffectManager;
    public UnitEffectManager effectManager => _EffectManager ?? (_EffectManager = GetComponent<UnitEffectManager>());


    private UnitWaitProcess _WaitProcess;
    public UnitWaitProcess waitProcess => _WaitProcess ?? (_WaitProcess = GetComponent<UnitWaitProcess>());


}
