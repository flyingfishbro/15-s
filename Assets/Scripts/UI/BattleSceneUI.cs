using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleSceneUI : UI_Instance
{
    private const string DAMAGE_UI_EFFECT_PATH = "UI/Effect/DamageFloatingTextEffect";
    private EffectPoolObject damageEffectPoolObject;


    public UnitChangeUIManager unitChangeUIManager;

    private void Awake()
    {
        damageEffectPoolObject = EffectPoolObject.GetPoolObjectInstance(DAMAGE_UI_EFFECT_PATH, "DamageEffect", transform, 20, EffectObject.PositionType.RECT);
    }



    public TeamStatusUI ATeamStatusUI;

    public TeamStatusUI BTeamStatusUI;


    public void TeamRegister(Team team)
    {

        if (team.teamType == Team.TeamType.A)
        {
            ATeamStatusUI.gameObject.SetActive(true);
            ATeamStatusUI.SetTeam(team);
        }
        else
        {
            BTeamStatusUI.gameObject.SetActive(true);
            BTeamStatusUI.SetTeam(team);
        }

    }




    

    public void OnDamagedEffect(Vector3 worldPos, float damage, int option)
    {
        damageEffectPoolObject.OnEffect(
            Camera.main.WorldToScreenPoint(worldPos),
            Vector3.zero,
            option,
            damage);
    }


    public IEnumerator UnitChoosingCoroutine(List<Unit> waitList, BattleMode.SetChoosedNextUnitDelegate choosedListener) => unitChangeUIManager.ChoosingUnitInputCoroutine(waitList, choosedListener);

    public class ChoosedUnitInfo
    {
        public Unit _Unit { get; private set; }

        public void SetUnit(Unit unit) => _Unit = unit;

    }


}
