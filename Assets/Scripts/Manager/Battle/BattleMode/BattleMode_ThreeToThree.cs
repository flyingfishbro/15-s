using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class BattleMode_ThreeToThree : BattleMode
{
    public override ModeType mode => ModeType.THREE_TO_THREE;

    public bool battleIsStarted { get; private set; } = false;

    public override void SetFieldUnit(Unit unit)
    {
        BattleManager.instance.GetTeam(unit).SetFieldUnit(unit);
        
        if (!battleManager.fieldIsValid) return;



        //Battle�� ó�� ���۵Ǿ�����
        if (!battleIsStarted)
        {
            battleIsStarted = true;


            BattleManager.instance.GetBattleCam().SetCamMode(BattleCam.BattleCamMode.BATTLE);

            
            battleManager.SetBattle(true);

        }
        else
        {

        }




    }




    public override void RetireUnit(Unit unit)
    {
        battleManager.SetBattle(false);

        Team unitTeam = battleManager.GetTeam(unit.teamType);

        battleManager.GetFieldUnit(unit.enemyUnit.teamType).SetWait(true);
        if (unitTeam.IsValid)
        {
            StartCoroutine(ChangeUnitCoroutine(unit));

        }
        else
        {
            //print("Enemy Team Win!!");

            StartCoroutine(FinishBattleCoroutine(unit));
        }

    }


    private Unit selectedNextUnit;
    
    private void SetSelectedUnit(Unit unit)
    {
        selectedNextUnit = unit;
    }


    
    private IEnumerator ChangeUnitCoroutine(Unit retire)
    {
        
        #region Death Scene

        BattleManager.instance.GetBattleCam().SetTargetingCamMode(retire.transform, BattleManager.unitMinDistance + 1, 4);
        Time.timeScale = .5f;

        yield return new WaitForSeconds(1f);

        yield return new WaitUntil(() => !(retire.rb.velocity.magnitude > 0.5f));

        #endregion

        #region Choosing NextUnit


        
        //�÷��̾� ���� ��쿡�� �������� �����ϴ�.
        if (retire.teamType == BattleManager.instance.playerTeamType)
            yield return BattleManager.instance.GetBattleSceneUI().UnitChoosingCoroutine(retire.team.waitingUnits, SetSelectedUnit);


        //�������� ���� ���� ����� �Ҵ��ϰ�, ���� ���������� null�� �ʱ�ȭ��ŵ�ϴ�.
        Unit nextUnit = selectedNextUnit;
        selectedNextUnit = null;

        //���� nextUnit�� ��ȿ���� ���� ���(�����̰ų�, �������� ������ ����) �ش� ���� waitingUnits����Ʈ������ �������� ���� �ϳ��� �޾ƿɴϴ�.
        if (nextUnit == null)
        {
            nextUnit = retire.team.GetRandomUnitInWaitList();
        }
        

        #endregion




        #region ChangeScene

        /*
        BattleCam.instance.SetTargetingCamMode(
            new Transform[]
            {
                BattleManager.instance.GetLastFieldUnit(BattleManager.instance.playerTeamType).transform,
                BattleManager.instance.GetLastFieldUnit(BattleManager.instance.enemyTeamType).transform
            });
        */

        BattleManager.instance.GetBattleCam().SetTargetingCamMode(retire.transform, 8, 0);



        Time.timeScale = 1f;


        Team retireUnitTeam = battleManager.GetTeam(retire);
        yield return retireUnitTeam.UnitChange(retire, nextUnit);
        #endregion


        #region Finally
        BattleManager.instance.GetOhterFieldUnit(retire.teamType).state.isWait.SetState(false);
        BattleManager.instance.GetBattleCam().SetCamMode(BattleCam.BattleCamMode.BATTLE);

        yield return new WaitForSeconds(1);

        BattleManager.instance.SetBattle(true);
        #endregion


    }


    private IEnumerator FinishBattleCoroutine(Unit retire)
    {
        BattleManager.instance.GetBattleCam().SetTargetingCamMode(retire.transform, BattleManager.unitMinDistance + 1, 1);
        Time.timeScale = .5f;

        yield return new WaitForSeconds(1.5f);
        yield return new WaitUntil(() => !(retire.rb.velocity.magnitude > 0.1f));
        Time.timeScale = 1f;


        BattleManager.instance.GetBattleCam().SetTargetingCamMode(retire.enemyUnit.transform, 4, 3);


        yield return new WaitForSeconds(2);

        BattleManager.instance.FinishBattle(retire.enemyUnit.teamType);
    }

}
