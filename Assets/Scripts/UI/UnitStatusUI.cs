using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitStatusUI : MonoBehaviour
{
    #region Color Setting
    private static readonly Color COLOR_UNIT_ALIVE = new Color(1, 1, 1, 1);
    private static readonly Color COLOR_UNIT_DEATH = new Color(.4f, .4f, .4f, 1);

    private Color hpBarFillDefaultColor;
    private Color hpBarBgDefaultColor;
    #endregion
    

    private Unit _Unit;


    public Image _UnitImg;

    public Image _UnitImgFrame;



    public Slider _UnitHp;

    public Image _UnitHpBarFill;
    public Image _UnitHpBarBackground;


    private void Awake()
    {
        hpBarFillDefaultColor = _UnitHpBarFill.color;
        hpBarBgDefaultColor = _UnitHpBarBackground.color;
    }


    public void SetUnit(Unit unit)
    {
        if (unit == null) return;

        _Unit = unit;

        _UnitImg.sprite = unit.modelImg;

        unit.AddUnitStatusEventListener(UnitStatusEventListener);

        //설정되고 한번 호출이 되고 시작
        UnitStatusEventListener(unit.status);
    }


    private void UnitStatusEventListener(UnitStatus unitStatus)
    {
        UpdateUnitHp(unitStatus.hp, unitStatus.maxHp);
    }

    private void UpdateUnitHp(float currentHp, float maxHp)
    {
        _UnitHp.value = currentHp / maxHp;


        #region Set Image/Slider Color
        if (_Unit.status.IsAlive)
        {
            _UnitImg.color = COLOR_UNIT_ALIVE;
            _UnitImgFrame.color = COLOR_UNIT_ALIVE;

            _UnitHpBarFill.color = hpBarFillDefaultColor;

            _UnitHpBarBackground.color = hpBarBgDefaultColor;
        }
        else
        {
            _UnitImg.color = COLOR_UNIT_DEATH;
            _UnitImgFrame.color = COLOR_UNIT_DEATH;

            _UnitHpBarFill.color = COLOR_UNIT_DEATH;


            _UnitHpBarBackground.color = COLOR_UNIT_DEATH;
        }
        #endregion
    }
}
