using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Team : MonoBehaviour
{
    /// <summary>
    /// ���� Ÿ���� ��Ÿ���ϴ�.
    /// </summary>
    public enum TeamType
    {
        A,
        B
    }

    /// <summary>
    /// ���� �� ���� ��Ÿ���� ��Ÿ���ϴ�.
    /// </summary>
    public TeamType teamType {  get; private set; }


    public Unit currentFieldUnit;// { get; private set; }


    public List<Unit> members {  get; private set; }


    /// <summary>
    /// ��� ���� ���ֵ��Դϴ�.
    /// </summary>
    public List<Unit> waitingUnits;// { get; private set; }    

    /// <summary>
    /// Ż���� ���ֵ��Դϴ�.
    /// </summary>
    public List<Unit> retireUnits;// { get; private set; }




    public delegate void TeamStatusEvent(Team team);

    private TeamStatusEvent teamStatusEvent;

    public void InvokeTeamStatusEvent() => teamStatusEvent?.Invoke(this);

    public void AddTeamStatusEventListener(TeamStatusEvent eventer)
    {
        teamStatusEvent += eventer;
    }





    /// <summary>
    /// ���� �� ���� ��ȿ���� �˻��մϴ�. ���� ��� �������� Ż���Ǹ�(��� ���� ������ �������) false�� ��ȯ�մϴ�.
    /// </summary>
    public bool IsValid => waitingUnits.Count > 0;



    private UnitChangeManager _ChangeManager;
    private UnitChangeManager changeManager => _ChangeManager;



    public bool IsBelong(Unit unit) => currentFieldUnit == unit || waitingUnits.Contains(unit) || retireUnits.Contains(unit);




    public Unit GetRandomUnitInWaitList() => waitingUnits.Count > 0 ? waitingUnits[Random.Range(0, waitingUnits.Count)] : null;


    public void SetMembers(List<Unit> unitList)
    {
        this.members = new List<Unit>();
        foreach (Unit unit in unitList)
        {
            this.members.Add(unit);
        }
    }


    /// <summary>
    /// ���� �ʱ�ȭ�մϴ�.
    /// </summary>
    /// <param name="unitList">��ϵ� ���� ����Ʈ</param>
    /// <param name="teamType">���� ����� �� Ÿ��</param>
    public void Initialized(List<Unit> unitList, TeamType teamType)
    {
        if (members == null) SetMembers(unitList);

        this.waitingUnits = unitList;
        this.retireUnits = new List<Unit>();


        foreach(Unit unit in unitList)
        {
            unit.gameObject.SetActive(false);
        }
        this.teamType = teamType;


        _ChangeManager = new GameObject($"{teamType} Team's UnitChangeManager").AddComponent<UnitChangeManager>();
        _ChangeManager.transform.SetParent(transform);


        BattleManager.instance.TeamRegister(this);


        //ù��° ���� ������ �ʵ��������� �����մϴ�.
        BattleManager.instance.SetFieldUnit(waitingUnits[0]);
    }



    /// <summary>
    /// �ʵ� ������ �����մϴ�. ������ �ʵ������� ��⸮��Ʈ���� ���ܵ˴ϴ�.
    /// </summary>
    /// <param name="targetUnit">Ÿ�� ����</param>
    /// <returns></returns>
    public void SetFieldUnit(Unit targetUnit)
    {
        targetUnit.gameObject.SetActive(true);

        waitingUnits.Remove(targetUnit);

        currentFieldUnit = targetUnit;

        InvokeTeamStatusEvent();
    }

    /// <summary>
    /// �ʵ������� ����, ��Ÿ�̾�� ������ ��Ÿ�̾� ����Ʈ�� �߰��մϴ�.
    /// </summary>
    /// <param name="targetUnit"></param>
    public void RetireUnit(Unit targetUnit)
    {
        if (targetUnit != currentFieldUnit) return;

        currentFieldUnit = null;

        retireUnits.Add(targetUnit);

        InvokeTeamStatusEvent();
    }



    public IEnumerator UnitChange(Unit retire, Unit next)
    {
        yield return changeManager.GetUnitChangeCoroutine()?.Invoke(retire, next);
    }

}
