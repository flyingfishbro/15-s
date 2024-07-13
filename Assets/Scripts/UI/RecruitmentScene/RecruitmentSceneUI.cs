using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RecruitmentSceneUI : UI_Instance
{
    [SerializeField] private CanvasGroup recruitmentImg;

    [SerializeField] private List<RecruitmentUnitElement> recruitmentUnitList;

    [SerializeField] private RecruitmentScene manager;

    public IEnumerator OnRecruitmentUI()
    {
        Team.TeamType loserTeamType = GameData.instance.battleSceneData.winnerTeam == Team.TeamType.A ? Team.TeamType.B : Team.TeamType.A;
        TeamLoader.TeamLoadInfo enemyUnitInfo = 
            loserTeamType == Team.TeamType.A ?
            GameData.instance.battleSceneData.ATeamLoadInfo : GameData.instance.battleSceneData.BTeamLoadInfo;


        Team enemyTeam = TeamLoader.CreateTeam(enemyUnitInfo, false);
        enemyTeam.gameObject.SetActive(false);


        for (int i = 0; i < enemyTeam.members.Count; ++i)
        {
            recruitmentUnitList[i].SetUnit(i, enemyTeam.members[i]);
            recruitmentUnitList[i].gameObject.SetActive(true);
        }


        float targetAlpha = 1;

        while (Mathf.Abs(targetAlpha - recruitmentImg.alpha) > 0.01f)
        {
            yield return null;
            recruitmentImg.alpha = Mathf.Lerp(recruitmentImg.alpha, targetAlpha, 0.1f);
        }
        recruitmentImg.alpha = targetAlpha;


        while (recruitUnit == null) yield return null; 

        manager.SetRecruitmentUnit(recruitUnit);

    }


    public IEnumerator OffRecruitmentUI()
    {
        float targetAlpha = 0;
        while (Mathf.Abs(targetAlpha - recruitmentImg.alpha) > 0.01f)
        {
            yield return null;
            recruitmentImg.alpha = Mathf.Lerp(recruitmentImg.alpha, targetAlpha, 0.1f);
        }
        recruitmentImg.alpha = targetAlpha;
    }





    public Color defaultColor;
    public Color selectedColor;

    private Unit recruitUnit;

    private Unit selectedUnit;

    public void SetSelectedUnit(int index, Unit unit)
    {

        if (selectedUnit == unit)
        {
            recruitUnit = unit;
            return;
        }

        selectedUnit = unit;

        for (int i = 0; i < recruitmentUnitList.Count; ++i)
        {
            if (i == index) recruitmentUnitList[i].GetComponent<Image>().color = selectedColor;
            else recruitmentUnitList[i].GetComponent<Image>().color = defaultColor;
        }

    }




}
