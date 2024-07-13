using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class SceneLoader : ManagerBase<SceneLoader>
{
    protected override bool IsDonDestroy => true;

    private const string SCENENAME_MAINMENU = "MainMenu";

    private const string SCENENAME_BATTLEMAP_DEFAULT = "BattleMap_Default";

    private const string SCENENAME_INTRO = "0. intro";

    private const string SCENENAME_MAIN_MENU = "1. MainMenu";

    private const string SCENENAME_LOADING = "2. Loading";

    private const string SCENENAME_ROUNDPROGRESS = "RoundProgress";

    private const string SCENENAME_DEFEAT = "4. Defeat";

    private const string SCENENAME_RECRUITMENT = "5. Recruitment";

    private const string SCENENAME_MANAGEMENT = "6. Management";

    private const string SCENENAME_RECOVERY = "7. Recovery";

    private const string SCENENAME_ROUNDEND = "10. RoundEnd";


    public void LoadBattleScene() => SceneManager.LoadScene(SCENENAME_BATTLEMAP_DEFAULT);

    public void LoadDefeatScene() => SceneManager.LoadScene(SCENENAME_DEFEAT);

    public void LoadMainMenuScene() => SceneManager.LoadScene(SCENENAME_MAIN_MENU);

    public void LoadRoundProgressScene(int round) => SceneManager.LoadScene($"{SCENENAME_ROUNDPROGRESS} {round}");


    public void LoadRecruitmentScene() => SceneManager.LoadScene(SCENENAME_RECRUITMENT);

    public void LoadRoundEndScene() => SceneManager.LoadScene(SCENENAME_ROUNDEND);

}
