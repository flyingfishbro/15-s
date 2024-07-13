using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShieldInput : BattleInputSystem
{
    private Image _OnShieldInputImg;
    private Image inputImg => _OnShieldInputImg ?? (GetComponent<Image>());

    public override void SetInput(bool set, Vector2 pos)
    {
        if (set && !IsGamePaused)
        {
            playerController.OnShield();
        }

    }

    private void Update()
    {
        Unit playerUnit = BattleManager.instance.GetFieldUnit(BattleManager.instance.playerTeamType);
        if (playerUnit != null)
        {
            inputImg.enabled = playerUnit.status.hasShield;
        }
    }

}
