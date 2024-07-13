using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitController : MonoBehaviour
{
    public Team.TeamType targetTeamType;

    public bool isLock { get; private set; } = true;

    public void SetLock(bool set) => isLock = set;


    private void Awake() => BattleManager.instance.SetUnitController(this);



    //private Unit targetFieldUnit => BattleManager.instance.GetFieldUnit(targetTeamType);


    private bool isValid(out Unit targetUnit)
    {
        targetUnit = null;
        if (isLock) return false;

        targetUnit = BattleManager.instance.GetFieldUnit(targetTeamType);
        return targetUnit != null;
    }



    public void SetAttackInput(bool set)
    {
        if (isValid(out Unit targetUnit))
            targetUnit.SetAttackInput(set);
    }

    public void AddAimRot(float value)
    {
        if (isValid(out Unit targetUnit)) 
            targetUnit.AddAimRot(value);
    }

    public void SetAimRot(float value)
    {
        if (isValid(out Unit targetUnit)) 
            targetUnit.SetAimRot(value);
    }

    public void OnShield()
    {
        if (isValid(out Unit targetUnit))
            targetUnit.OnShield();
            
    }

}
