using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BattleInputSystem : TouchInputSystem
{
    private static InputOption _InputOptionInstance;
    public static InputOption inputOptionInstance => _InputOptionInstance ?? (_InputOptionInstance = Resources.Load<InputOption>("Scriptable/InputOptionScriptable"));

    public static bool IsGamePaused = false;

    public override TouchInputType inputType => TouchInputType.BATTLE;


    private UnitController _PlayerController;
    protected UnitController playerController => _PlayerController ?? (_PlayerController = BattleManager.instance.GetPlayerTeamUnitController());

}
