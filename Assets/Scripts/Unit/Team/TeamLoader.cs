using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static BattleMode;
using static Team;

public class TeamLoader : Loader<TeamLoader>
{

    public struct TeamLoadInfo
    {
        public Team.TeamType teamType { get; private set; }

        public List<UnitLoader.UnitModelType> modelTypes { get; private set; }

        public List<Unit.HandleDirection> handleDirections { get; private set; }

        public List<UnitStatus> unitStatus { get; private set; }

        public List<string> gunCodes { get; private set; }

        public override string ToString()
        {
            return $"{teamType} | {modelTypes[0]} | {handleDirections[0]} | {gunCodes[0]}";
            
        }


        public TeamLoadInfo(
            Team.TeamType teamType, 
            List<UnitLoader.UnitModelType> modelTypes, 
            List<Unit.HandleDirection> handleDirections, 
            List<UnitStatus> unitStatus, List<string> gunCodes)
        {
            this.teamType = teamType;
            this.modelTypes = modelTypes;
            this.handleDirections = handleDirections;
            this.unitStatus = unitStatus;
            this.gunCodes = gunCodes;
        }
    }


    public static Team CreateTeam(TeamLoadInfo teamLoadInfo, bool setInitialized)
    {

        Team teamInstance = new GameObject($"{teamLoadInfo.teamType}'s TeamInstance").AddComponent<Team>();
        List<Unit> units = new List<Unit>();

        for (int i = 0; i < teamLoadInfo.modelTypes.Count; ++i)
        {

            Unit unitInstance = UnitLoader.instance.GetUnitInstance(
                new UnitLoader.UnitLoadInfo(
                    teamLoadInfo.teamType,
                    teamLoadInfo.modelTypes[i],
                    teamLoadInfo.handleDirections[i],
                    teamLoadInfo.unitStatus != null ? teamLoadInfo.unitStatus[i] : null,
                    teamLoadInfo.gunCodes[i]));

            unitInstance.transform.SetParent(teamInstance.transform);
            unitInstance.transform.localPosition = Vector3.zero;
            unitInstance.transform.localRotation = Quaternion.identity;
            units.Add(unitInstance);
        }

        teamInstance.SetMembers(units);
        
        if (setInitialized)
            teamInstance.Initialized(units, teamLoadInfo.teamType);
        return teamInstance;
    }


}
