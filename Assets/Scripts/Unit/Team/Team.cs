using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Team : MonoBehaviour
{
    /// <summary>
    /// 팀의 타입을 나타냅니다.
    /// </summary>
    public enum TeamType
    {
        A,
        B
    }

    /// <summary>
    /// 현재 이 팀의 팀타입을 나타냅니다.
    /// </summary>
    public TeamType teamType {  get; private set; }


    public Unit currentFieldUnit;// { get; private set; }


    public List<Unit> members {  get; private set; }


    /// <summary>
    /// 대기 중인 유닛들입니다.
    /// </summary>
    public List<Unit> waitingUnits;// { get; private set; }    

    /// <summary>
    /// 탈락된 유닛들입니다.
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
    /// 현재 이 팀이 유효한지 검사합니다. 만일 모든 팀원들이 탈락되면(대기 중인 팀원이 없을경우) false를 반환합니다.
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
    /// 팀을 초기화합니다.
    /// </summary>
    /// <param name="unitList">등록될 유닛 리스트</param>
    /// <param name="teamType">팀의 적용될 팀 타입</param>
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


        //첫번째 메인 유닛을 필드유닛으로 설정합니다.
        BattleManager.instance.SetFieldUnit(waitingUnits[0]);
    }



    /// <summary>
    /// 필드 유닛을 설정합니다. 설정된 필드유닛은 대기리스트에서 제외됩니다.
    /// </summary>
    /// <param name="targetUnit">타겟 유닛</param>
    /// <returns></returns>
    public void SetFieldUnit(Unit targetUnit)
    {
        targetUnit.gameObject.SetActive(true);

        waitingUnits.Remove(targetUnit);

        currentFieldUnit = targetUnit;

        InvokeTeamStatusEvent();
    }

    /// <summary>
    /// 필드유닛을 비우고, 리타이어된 유닛을 리타이어 리스트에 추가합니다.
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
