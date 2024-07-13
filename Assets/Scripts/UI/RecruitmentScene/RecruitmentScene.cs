using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecruitmentScene : MonoBehaviour
{
    public RecruitmentSceneUI ui;


    private void Start()
    {
        StartCoroutine(RecruimentCoroutine());
    }

    private Unit recruitmentUnit;

    public void SetRecruitmentUnit(Unit unit) => recruitmentUnit = unit; 

    private IEnumerator RecruimentCoroutine()
    {
        yield return ui.OnRecruitmentUI();

        yield return new WaitForSeconds(.5f);

        yield return ui.OffRecruitmentUI();



        TeamLoader.TeamLoadInfo teamLoadInfo = 
            GameData.instance.battleSceneData.winnerTeamType == Team.TeamType.A ? 
            GameData.instance.battleSceneData.ATeamLoadInfo : GameData.instance.battleSceneData.BTeamLoadInfo;

        teamLoadInfo.modelTypes.Add(recruitmentUnit.modelType);
        teamLoadInfo.handleDirections.Add(Unit.HandleDirection.LEFT);
        teamLoadInfo.unitStatus.Add(recruitmentUnit.status);
        teamLoadInfo.gunCodes.Add(recruitmentUnit.currentGun.gunCode);


        TeamLoader.TeamLoadInfo enemyTeamLoadInfo =
            GameData.instance.battleSceneData.winnerTeamType == Team.TeamType.A ?
            GameData.instance.battleSceneData.BTeamLoadInfo : GameData.instance.battleSceneData.ATeamLoadInfo;

        int round = GameData.instance.currentRound;

        LoadInfoList_Test info = Instantiate(Resources.Load<LoadInfoList_Test>("Test_DeleteMe/Test"));


        GameData.BattleSceneData newData = new GameData.BattleSceneData(GameData.instance.battleSceneData.ATeamLoadInfo, 
            new TeamLoader.TeamLoadInfo(
                Team.TeamType.B, 
                info.GetModelTypeList(round +1),
                info.GetHandleList(round +1),
                null,
                info.GetGunCodeList(round + 1)));

        
        GameData.instance.SetBattleSceneData(newData);

        SceneLoader.instance.LoadBattleScene();


    }


}
