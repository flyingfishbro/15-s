using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static BattleMode;
using static TeamLoader;
using static Unit;
using static UnitLoader;


/// <summary>
/// 1. MainMenu Scene 의 Start 버튼을 나타내기 위한 컴포넌트입니다.
/// </summary>
public sealed class MainMenuStartButton : MonoBehaviour
{
    [Header("# Gravity Option")]
    public MainMenuTournamentGravityOption m_GravityOption;

    [Header("# Weapon Option")]
    public MainMenuTournamentWeaponOption m_WeaponOption;

    private Image _StartButtonImage;
    private EventTrigger _EventTrigger;


    private void Awake()
    {


        _StartButtonImage = GetComponent<Image>();
        _EventTrigger = GetComponent<EventTrigger>();

        // Bind StartButton Event...
        EventTrigger.Entry onStartButtonClickEvent = new EventTrigger.Entry();
        onStartButtonClickEvent.callback.AddListener(CALLBACK_OnStartButtonClicked);

        _EventTrigger.triggers.Add(onStartButtonClickEvent);
    }


    /// <summary>
    /// 플레이어 팀 정보를 생성하여 반환합니다.
    /// </summary>
    /// <returns></returns>
    private TeamLoader.TeamLoadInfo GeneratePlayerTeamInfo()
    {
        // 중력 옵션 가져오기
        GameManager.GravityType gravityType = m_GravityOption.GetGravityOptionValue();

        // 무기 코드 가져오기
        List<string> playerWeaponCodes = new List<string>() { m_WeaponOption.GetWeaponOptionValue() };

        // Player Unit Model Type
        List<UnitLoader.UnitModelType> unitModelTypes = new List<UnitLoader.UnitModelType>() { UnitLoader.UnitModelType.V1 };

        // Player HandleType
        List<Unit.HandleDirection> handleDirections = new List<Unit.HandleDirection>() { Unit.HandleDirection.LEFT };

        return new TeamLoader.TeamLoadInfo(
            Team.TeamType.A, unitModelTypes, handleDirections, null, playerWeaponCodes);
    }


    /// <summary>
    /// 적 팀 정보를 생성하여 반환합니다.
    /// </summary>
    /// <returns></returns>
    private TeamLoader.TeamLoadInfo GenerateEnemyTeamInfo()
    {
        // 랜덤한 유닛 반환 내부 함수
        UnitLoader.UnitModelType GetRandomUnitModel(params UnitLoader.UnitModelType[] excludeUnitType)
        {
            // 모든 UnitModelType 열거 형식 요소를 리스트에 담습니다.
            List<UnitLoader.UnitModelType> unitModelTypeList = new(
                Enum.GetValues(typeof(UnitLoader.UnitModelType)) as UnitLoader.UnitModelType[]);

            // 요소 제외
            foreach(UnitLoader.UnitModelType exclude in excludeUnitType)
                unitModelTypeList.Remove(exclude);

            // 랜덤한 적 유닛 타입 반환
            int randomindex = UnityEngine.Random.Range(0, unitModelTypeList.Count - 1);
            return unitModelTypeList[randomindex];
        }

        // 랜덤한 무기 코드 반환 내부 함수
        string GetRandomWeaponCode()
        {
            // 무기 코드 리스트에 추가
            List<string> weaponCode = new List<string>();
            weaponCode.Add("w00001");
            weaponCode.Add("w00002");
            weaponCode.Add("w00003");
            weaponCode.Add("w00004");

            // 랜덤 인덱스
            int randomIndex = UnityEngine.Random.Range(0, weaponCode.Count - 1);

            // 랜덤한 무기 코드 반환
            return weaponCode[randomIndex];
        }

        // 적 타입
        List<UnitLoader.UnitModelType> modelTypes = new() { GetRandomUnitModel(UnitModelType.V1) };

        // 핸들 방향
        List<Unit.HandleDirection> handleDirections = new() { HandleDirection.RIGHT };

        // 무기 코드
        List<string> gunCodes = new List<string>() { GetRandomWeaponCode() };

        return new TeamLoader.TeamLoadInfo(
            Team.TeamType.B, modelTypes, handleDirections, null, gunCodes);
    }


    /// <summary>
    /// StartButton 클릭 시 호출되는 콜백
    /// </summary>
    /// <param name="evtData"></param>
    private void CALLBACK_OnStartButtonClicked(BaseEventData evtData)
    {
        // 플레이어 팀 정보
        TeamLoader.TeamLoadInfo playerTeamInfo = GeneratePlayerTeamInfo();

        // 적 팀 정보
        TeamLoader.TeamLoadInfo enemyTeamInfo = GenerateEnemyTeamInfo();

        // BattleScene 데이터 설정
        GameData.instance.SetBattleSceneData(new GameData.BattleSceneData(playerTeamInfo, enemyTeamInfo));

        Debug.Log("playerTeamInfo = " + playerTeamInfo);
        Debug.Log("enemyTeamInfo = " + enemyTeamInfo);

        // 씬 이동
        SceneManager.LoadScene("RoundProgress 0");
    }
}
