using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleMode_OneToOne : BattleMode
{

    public override ModeType mode => ModeType.ONE_TO_ONE;

    public override void SetFieldUnit(Unit unit)
    {
        if (!battleManager.fieldIsValid) return;

        //BattleSceneUI.instance.FieldUnitRegister(unit);
        BattleManager.instance.GetBattleCam().SetCamMode(BattleCam.BattleCamMode.BATTLE);

        //StartCoroutine(battleManager.GetTeam(Team.TeamType.A).UnitChange(unit, unit));




        //battleManager.GetUnitController(Team.TeamType.A).SetLock(false);
        //battleManager.GetUnitController(Team.TeamType.B).SetLock(false);

    }




    public override void RetireUnit(Unit unit)
    {
        if (battleManager.GetTeam(unit.teamType).IsValid)
        {
            //StartCoroutine(battleManager.GetTeam(Team.TeamType.A).UnitChange(unit, ));


        }
        else
        {
            print("Enemy Team Win!!");
        }

    }



}
