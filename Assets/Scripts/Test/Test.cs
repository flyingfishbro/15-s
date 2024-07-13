using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Test : MonoBehaviour
{
    
    public List<UnitLoader.UnitModelType> ATeamModelTypes;
    public List<Unit.HandleDirection> ATeamHandleTypes;
    public List<string> ATeamGunCodes;


    public List<UnitLoader.UnitModelType> BTeamModelTypes;
    public List<Unit.HandleDirection> BTeamHandleTypes;
    public List<string> BTeamGunCodes;




    public bool started { get; set; } = false;
    

    
    private void Start()
    {
        StartCoroutine(TestCoroutine());
    }
    private IEnumerator TestCoroutine()
    {
        while (!started) yield return null;

        TeamLoader.TeamLoadInfo ATeamLoadInfo = new TeamLoader.TeamLoadInfo(Team.TeamType.A, ATeamModelTypes, ATeamHandleTypes, null, ATeamGunCodes);
        TeamLoader.TeamLoadInfo BTeamLoadInfo = new TeamLoader.TeamLoadInfo(Team.TeamType.B, BTeamModelTypes, BTeamHandleTypes, null, BTeamGunCodes);


        GameData.instance.SetBattleSceneData(new GameData.BattleSceneData(ATeamLoadInfo, BTeamLoadInfo));

        SceneManager.LoadScene("RoundProgress 0");


    }
    

}
