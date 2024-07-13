using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TeamStatusUI : MonoBehaviour
{
    public UnitStatusUI _FieldUnitStatus;

    public List<Image> _UnFieldUnitList;


    public void SetTeam(Team team)
    {
        if (team == null) return;

        team.AddTeamStatusEventListener(TeamStatusEventListener);

        //등록됨과 동시에 한번 호출
        team.InvokeTeamStatusEvent();
    }

    private void TeamStatusEventListener(Team team)
    {
        if (team == null) return;

        if (team.currentFieldUnit != null)
        {
            _FieldUnitStatus.SetUnit(team.currentFieldUnit);
        }

        SetNextUnitList(team);

    }

    private static readonly Color COLOR_WAITUNIT = new Color(1, 1, 1, 1);
    private static readonly Color COLOR_RETIREUNIT = new Color(.4f, .4f, .4f, 1);


    private void SetNextUnitList(Team team)
    {
        int index = 0;

        
        foreach (Unit unit in team.waitingUnits)
        {
            _UnFieldUnitList[index].gameObject.SetActive(true);

            Image img = _UnFieldUnitList[index].transform.GetChild(0).GetComponent<Image>();
            img.sprite = unit.modelImg;
            img.color = COLOR_WAITUNIT;

            ++index;
        }


        foreach (Unit unit in team.retireUnits)
        {
            _UnFieldUnitList[index].gameObject.SetActive(true);

            Image img = _UnFieldUnitList[index].transform.GetChild(0).GetComponent<Image>();
            img.sprite = unit.modelImg;
            img.color = COLOR_RETIREUNIT;

            ++index;
        }


        for (; index < _UnFieldUnitList.Count; ++index)
        {
            _UnFieldUnitList[index].gameObject.SetActive(false);
        }

        
        
        /*
        foreach (Unit unit in team.waitingUnits)
        {
            _UnFieldUnitList[index].gameObject.SetActive(true);

            Image unitImg = _UnFieldUnitList[index].transform.GetChild(0).GetComponent<Image>();

            unitImg.sprite = unit.modelImg;
            unitImg.color = COLOR_WAITUNIT;

            ++index;
        }
        */
        /*
        foreach (Unit unit in team.retireUnits)
        {
            _UnFieldUnitList[index].gameObject.SetActive(true);

            Image unitImg = _UnFieldUnitList[index].transform.GetChild(0).GetComponent<Image>();

            unitImg.sprite = unit.modelImg;
            unitImg.color = COLOR_RETIREUNIT;

            ++index;
        }
        */


    }

}
