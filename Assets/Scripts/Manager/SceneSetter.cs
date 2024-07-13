using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SceneSetter : MonoBehaviour
{

    public enum SceneType
    {
        BATTLE_SCENE,
        UNBATTLE_SCENE
    }

    public SceneType _SceneType = SceneType.BATTLE_SCENE;


    #region Battle Setting

    private Canvas _BattleCanvas => Resources.Load<Canvas>("SceneSetter/BattleCanvas");

    private GameObject _UnitControllers => Resources.Load<GameObject>("SceneSetter/UnitControllers");



    public Vector3 _CamStartPos;

    public Vector3 ATeamPos;
    public Vector3 BTeamPos;


    #endregion




    private void Start()
    {
        if (_SceneType == SceneType.BATTLE_SCENE)
        {
            BattleSceneAwake();
        }

        else
        {

        }

    }



    private void BattleSceneAwake()
    {

        Instantiate(_BattleCanvas);
        Instantiate(_UnitControllers);



        GameData.BattleSceneData data = GameData.instance.battleSceneData;

        Team ATeamInstance = TeamLoader.CreateTeam(data.ATeamLoadInfo, true);
        ATeamInstance.transform.position = ATeamPos;


        Team BTeamInstance = TeamLoader.CreateTeam(data.BTeamLoadInfo, true);
        BTeamInstance.transform.position = BTeamPos;
        
    }


}
