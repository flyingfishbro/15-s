using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class RecruitmentUnitElement : ProgressInputSystem
{
    public RecruitmentSceneUI ui;

    private int index;

    public Unit _Unit;

    [Header("Elements")]
    public Image modelImg;

    public Slider hpBar;

    public Image gunImg;

    public Image shieldImg;




    public void SetUnit(int index, Unit unit)
    {
        this.index = index;

        _Unit = unit;

        modelImg.sprite = unit.modelImg;

        hpBar.value = unit.status.hp / unit.status.maxHp;

        gunImg.sprite = unit.currentGun.gunInfo.sprite;

        shieldImg.color = Color.white * (unit.status.hasShield ? 1 : .6f);

    }


    bool lastInput = false;
    public override void SetInput(bool set, Vector2 pos)
    {
        if (lastInput != set)
        {
            lastInput = set;

            if (set)
            {
                ui.SetSelectedUnit(index, _Unit);
            }
            SoundManager.Instance.Play("ButtonClick");

        }

    }
}
