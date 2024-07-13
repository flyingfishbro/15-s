using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData : ManagerBase<GameData> 
{

    protected override bool IsDonDestroy => true;

    public int currentRound = 0;

    public class BattleSceneData
    {

        public Team.TeamType winnerTeamType;

        public TeamLoader.TeamLoadInfo ATeamLoadInfo { get; private set; }

        public TeamLoader.TeamLoadInfo BTeamLoadInfo { get; private set; }



        public Team.TeamType winnerTeam {  get; private set; }
        public void SetWinnerTeamType(Team.TeamType teamType) => winnerTeam = teamType;


        public BattleSceneData(
            TeamLoader.TeamLoadInfo ATeamLoadInfo,
            TeamLoader.TeamLoadInfo BTeamLoadInfo)
        {
            this.ATeamLoadInfo = ATeamLoadInfo;
            this.BTeamLoadInfo = BTeamLoadInfo;
        }



    }


    public BattleSceneData battleSceneData {  get; private set; }   

    public void SetBattleSceneData(BattleSceneData battleSceneData) => this.battleSceneData = battleSceneData;






}
