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
/// 1. MainMenu Scene �� Start ��ư�� ��Ÿ���� ���� ������Ʈ�Դϴ�.
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
    /// �÷��̾� �� ������ �����Ͽ� ��ȯ�մϴ�.
    /// </summary>
    /// <returns></returns>
    private TeamLoader.TeamLoadInfo GeneratePlayerTeamInfo()
    {
        // �߷� �ɼ� ��������
        GameManager.GravityType gravityType = m_GravityOption.GetGravityOptionValue();

        // ���� �ڵ� ��������
        List<string> playerWeaponCodes = new List<string>() { m_WeaponOption.GetWeaponOptionValue() };

        // Player Unit Model Type
        List<UnitLoader.UnitModelType> unitModelTypes = new List<UnitLoader.UnitModelType>() { UnitLoader.UnitModelType.V1 };

        // Player HandleType
        List<Unit.HandleDirection> handleDirections = new List<Unit.HandleDirection>() { Unit.HandleDirection.LEFT };

        return new TeamLoader.TeamLoadInfo(
            Team.TeamType.A, unitModelTypes, handleDirections, null, playerWeaponCodes);
    }


    /// <summary>
    /// �� �� ������ �����Ͽ� ��ȯ�մϴ�.
    /// </summary>
    /// <returns></returns>
    private TeamLoader.TeamLoadInfo GenerateEnemyTeamInfo()
    {
        // ������ ���� ��ȯ ���� �Լ�
        UnitLoader.UnitModelType GetRandomUnitModel(params UnitLoader.UnitModelType[] excludeUnitType)
        {
            // ��� UnitModelType ���� ���� ��Ҹ� ����Ʈ�� ����ϴ�.
            List<UnitLoader.UnitModelType> unitModelTypeList = new(
                Enum.GetValues(typeof(UnitLoader.UnitModelType)) as UnitLoader.UnitModelType[]);

            // ��� ����
            foreach(UnitLoader.UnitModelType exclude in excludeUnitType)
                unitModelTypeList.Remove(exclude);

            // ������ �� ���� Ÿ�� ��ȯ
            int randomindex = UnityEngine.Random.Range(0, unitModelTypeList.Count - 1);
            return unitModelTypeList[randomindex];
        }

        // ������ ���� �ڵ� ��ȯ ���� �Լ�
        string GetRandomWeaponCode()
        {
            // ���� �ڵ� ����Ʈ�� �߰�
            List<string> weaponCode = new List<string>();
            weaponCode.Add("w00001");
            weaponCode.Add("w00002");
            weaponCode.Add("w00003");
            weaponCode.Add("w00004");

            // ���� �ε���
            int randomIndex = UnityEngine.Random.Range(0, weaponCode.Count - 1);

            // ������ ���� �ڵ� ��ȯ
            return weaponCode[randomIndex];
        }

        // �� Ÿ��
        List<UnitLoader.UnitModelType> modelTypes = new() { GetRandomUnitModel(UnitModelType.V1) };

        // �ڵ� ����
        List<Unit.HandleDirection> handleDirections = new() { HandleDirection.RIGHT };

        // ���� �ڵ�
        List<string> gunCodes = new List<string>() { GetRandomWeaponCode() };

        return new TeamLoader.TeamLoadInfo(
            Team.TeamType.B, modelTypes, handleDirections, null, gunCodes);
    }


    /// <summary>
    /// StartButton Ŭ�� �� ȣ��Ǵ� �ݹ�
    /// </summary>
    /// <param name="evtData"></param>
    private void CALLBACK_OnStartButtonClicked(BaseEventData evtData)
    {
        // �÷��̾� �� ����
        TeamLoader.TeamLoadInfo playerTeamInfo = GeneratePlayerTeamInfo();

        // �� �� ����
        TeamLoader.TeamLoadInfo enemyTeamInfo = GenerateEnemyTeamInfo();

        // BattleScene ������ ����
        GameData.instance.SetBattleSceneData(new GameData.BattleSceneData(playerTeamInfo, enemyTeamInfo));

        Debug.Log("playerTeamInfo = " + playerTeamInfo);
        Debug.Log("enemyTeamInfo = " + enemyTeamInfo);

        // �� �̵�
        SceneManager.LoadScene("RoundProgress 0");
    }
}
