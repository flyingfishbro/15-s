using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ShotInput : BattleInputSystem
{
    public override void SetInput(bool set, Vector2 pos)
    {
        if (!IsGamePaused)
        {
            playerController.SetAttackInput(set);
        }
    }

    

}
