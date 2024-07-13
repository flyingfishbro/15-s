
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Subsystems;

public class BattleManager : MonoBehaviour //ManagerBase<BattleManager>
{
    #region Static
    private static BattleManager _CachedInstance;

    public static BattleManager instance => _CachedInstance ?? (_CachedInstance = GetBattleManagerInstance());




    private static BattleManagerSetting _BattleManagerSetting;
    private static BattleManagerSetting battleManagerSetting => _BattleManagerSetting ?? (_BattleManagerSetting = Resources.Load<BattleManagerSetting>("Scriptable/BattleManagerSetting"));
    #endregion

    private BattleSceneUI _CachedBattleSceneUI;


    private BattleCam _CachedBattleCam;


    public bool isBattle {  get; private set; }
    public void SetBattle(bool set)
    {
        isBattle = set;
        SetUnitControllerLock(!set);
    }

    public static float unitBaseDisstance => battleManagerSetting.DefaultUnitBaseDistance;

    public static float unitMaxDistance
        => Mathf.Clamp(
            battleManagerSetting.DefaultUnitMaxDistance / GameManager.gravityCorrectionValue,
            battleManagerSetting.DefaultUnitBaseDistance + 1,
            battleManagerSetting.TotalUnitMaxDistance);


    public static float unitMinDistance => battleManagerSetting.DefaultUnitMinDistance;


    public static BattleManager GetBattleManagerInstance()
    {
        BattleManager findedInstance = FindObjectOfType<BattleManager>();
        if (!findedInstance)
        {
            findedInstance = new GameObject("BattleManager").AddComponent<BattleManager>();
        }

        return findedInstance;
    }



    public static Vector3 GetUnitBasePosition(Unit caller)
    {
        if (caller == null) return Vector3.zero;

        Unit otherUnit = instance.GetOhterFieldUnit(caller.teamType);

        if (otherUnit == null) return Vector3.zero;

        Vector3 dir = (caller.transform.position - otherUnit.transform.position);
        dir.y = 0;
        dir.Normalize();

        Vector3 ret = otherUnit.transform.position + dir * unitBaseDisstance;

        return ret;
    }




    private BattleMode _CurrentBattleMode;

    public BattleMode currentBattleMode => _CurrentBattleMode ?? (_CurrentBattleMode = BattleModeManager.GetBattleModeInstance(currentBattleModeType));


    public BattleMode.ModeType currentBattleModeType = BattleMode.ModeType.THREE_TO_THREE;
    


    public Team.TeamType playerTeamType;
    public Team.TeamType botTeamType => playerTeamType == Team.TeamType.A ? Team.TeamType.B : Team.TeamType.A;

    public Team.TeamType enemyTeamType 
        => playerTeamType == Team.TeamType.A ? Team.TeamType.B : Team.TeamType.A;


    #region Controller
    public UnitController ATeamController { get; private set; }

    public UnitController BTeamController { get; private set; }
    public void SetUnitController(UnitController controller)
    {
        if (controller.targetTeamType == Team.TeamType.A) ATeamController = controller;
        else BTeamController = controller;
    }


    public UnitController GetUnitController(Team.TeamType teamType)
    {
        return teamType == Team.TeamType.A ? ATeamController : BTeamController;
    }

    private void SetUnitControllerLock(bool set)
    {
        if (ATeamController != null) ATeamController.SetLock(set);
        if (BTeamController != null) BTeamController.SetLock(set);
    }


    public UnitController GetPlayerTeamUnitController() => GetUnitController(playerTeamType);

    #endregion



    #region Team Manage

    private Team ATeam;

    private Team BTeam;
    

    public void TeamRegister(Team team)
    {
        if (team.teamType == Team.TeamType.A)
        {
            ATeam = team;
        }
        else
        {
            BTeam = team;
        }

        
        BattleManager.instance.GetBattleSceneUI().TeamRegister(team);

    }

    public Team GetTeam(Team.TeamType teamType)
        => teamType == Team.TeamType.A ? ATeam : BTeam;

    public Team GetTeam(Unit unit)
    {
        Team targetTeam = GetTeam(unit.teamType);

        return targetTeam == null ? null : targetTeam;
    }

    #endregion



    #region FieldUnit Manage

    public Unit GetFieldUnit(Team.TeamType teamType)
    {
        Team targetTeam = GetTeam(teamType);

        return targetTeam != null ? targetTeam.currentFieldUnit : null;
    }

    public Unit GetOhterFieldUnit(Team.TeamType teamType)
        => GetFieldUnit(teamType == Team.TeamType.A ? Team.TeamType.B : Team.TeamType.A);


    public bool fieldIsValid => GetFieldUnit(Team.TeamType.A) != null && GetFieldUnit(Team.TeamType.B);




    public void SetFieldUnit(Unit unit)
    {
        if (unit == null) return;

        //GetTeam(unit).SetFieldUnit(unit);

        currentBattleMode.SetFieldUnit(unit);
    }

    /// <summary>
    /// 마지막으로 필드의 있었던 해당 팀의 유닛을 반환합니다. 현재 필드에 해당 팀의 유닛이 있으면 반환하고, 없다면 마지막으로 리타이어되었던 유닛을 반환합니다.
    /// </summary>
    /// <param name="teamType"></param>
    /// <returns></returns>
    public Unit GetLastFieldUnit(Team.TeamType teamType)
    {
        Unit ret = GetFieldUnit(teamType);
        if (ret != null) return ret;

        Team team = GetTeam(teamType);
        if (team != null)
        {
            if (team.retireUnits == null || team.retireUnits.Count == 0) return null;

            return team.retireUnits[team.retireUnits.Count - 1];
        }
        return null;
    }


    #endregion


    /// <summary>
    /// 만일 피격받은 대상의 적군이 없거나(적팀의 필드유닛이 Null이거나), 적군이 현재 살아있지 않다면 받은 피해는 유효하지 못함을 판단하는 함수입니다.
    /// </summary>
    /// <param name="caller">피격받은 대상</param>
    /// <returns></returns>
    public bool damageIsValid(Unit caller)
    {
        Unit otherUnit = GetOhterFieldUnit(caller.teamType);

        return (otherUnit == null || !otherUnit.status.IsAlive) ? false : true;

    }


    public Team.TeamType jumpTargetTeam()
    {
        float disA = Vector3.Distance(GetFieldUnit(Team.TeamType.A).transform.position, Vector3.zero);
        float disB = Vector3.Distance(GetFieldUnit(Team.TeamType.B).transform.position, Vector3.zero);

        return disA >= disB ? Team.TeamType.A : Team.TeamType.B;
    }



    public void RetireUnit(Unit unit)
    {
        if (unit == null) return;

        currentBattleMode.RetireUnit(unit);

    }


    public void FinishBattle(Team.TeamType winnerTeamType)
    {
        GameData.BattleSceneData existingData = GameData.instance.battleSceneData;

        List<UnitStatus> ATeamUnitStatus = new List<UnitStatus>();
        
        Team ATeam = GetTeam(Team.TeamType.A);
        foreach (Unit unit in ATeam.members)
        {
            float half = unit.status.maxHp;
            float set = unit.status.hp > half ? unit.status.hp : half;

            unit.status.SetHp(set);

            unit.status.chargeShield();

            ATeamUnitStatus.Add(unit.status);
        }


        TeamLoader.TeamLoadInfo ATeamLoadInfo = 
            new TeamLoader.TeamLoadInfo(
                existingData.ATeamLoadInfo.teamType, 
                existingData.ATeamLoadInfo.modelTypes,
                existingData.ATeamLoadInfo.handleDirections,
                ATeamUnitStatus,
                existingData.ATeamLoadInfo.gunCodes);



        List<UnitStatus> BTeamUnitStatus = new List<UnitStatus>();

        Team BTeam = GetTeam(Team.TeamType.B);
        foreach (Unit unit in BTeam.members)
        {
            unit.status.SetHp(unit.status.maxHp);

            BTeamUnitStatus.Add(unit.status);   
        }


        TeamLoader.TeamLoadInfo BTeamLoadInfo =
            new TeamLoader.TeamLoadInfo(
                existingData.BTeamLoadInfo.teamType,
                existingData.BTeamLoadInfo.modelTypes,
                existingData.BTeamLoadInfo.handleDirections,
                BTeamUnitStatus,
                existingData.BTeamLoadInfo.gunCodes);


        GameData.instance.SetBattleSceneData(new GameData.BattleSceneData(ATeamLoadInfo, BTeamLoadInfo));
        GameData.instance.battleSceneData.SetWinnerTeamType(winnerTeamType);

        GameData.instance.currentRound++;


        StartCoroutine(FinishRoutine(winnerTeamType));

        
    }


    private IEnumerator FinishRoutine(Team.TeamType winnerTeamType)
    {
        yield return new WaitForSeconds(3);
        yield return GetBattleSceneUI().OnLoadingImgBackground();

        if (winnerTeamType == playerTeamType)
        {
            if (GameData.instance.currentRound < 3)
                SceneLoader.instance.LoadRoundProgressScene(GameData.instance.currentRound);
            else
            {
                // 게임 종료시 현재 라운드 0으로 초기화
                SceneLoader.instance.LoadRoundEndScene();
                GameData.instance.currentRound = 0;
            }
        }
        else
        {
            SceneLoader.instance.LoadDefeatScene();
        }

    }






    public BattleSceneUI GetBattleSceneUI()
    { 
        if (!_CachedBattleSceneUI)
        {
            _CachedBattleSceneUI = FindAnyObjectByType<BattleSceneUI>();
        }

        return _CachedBattleSceneUI;
    }


    public BattleCam GetBattleCam()
    {
        if (!_CachedBattleCam)
        {
            _CachedBattleCam = FindAnyObjectByType<BattleCam>();

            if (!_CachedBattleCam)
                _CachedBattleCam = BattleCam.CreateBattleCamInstance();
        }

        return _CachedBattleCam;    
    }



    private void OnDestroy()
    {
        _CachedInstance = null;
    }


    #region Check Frame
    private float deltaTime = 0f;

    [SerializeField] private int size = 25;
    [SerializeField] private Color color = Color.red;

    void Update()
    {
        deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;
    }

    private void OnGUI()
    {
        GUIStyle style = new GUIStyle();

        Rect rect = new Rect(30, 30, Screen.width, Screen.height);
        style.alignment = TextAnchor.UpperLeft;
        style.fontSize = size;
        style.normal.textColor = color;

        float ms = deltaTime * 1000f;
        float fps = 1.0f / deltaTime;
        string text = string.Format("{0:0.} FPS ({1:0.0} ms)", fps, ms);

        GUI.Label(rect, text, style);
    }
    #endregion
    
}
