using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitChangingChooseInput : ProgressInputSystem
{
    private UnitChangeUIManager _UnitChangeUIManager;
    private UnitChangeUIManager unitChangeUIManager => _UnitChangeUIManager ?? (_UnitChangeUIManager = GetComponentInParent<UnitChangeUIManager>());


    public Unit _Unit;

    [Header("Elements")]
    public Image modelImg;
    public Image modelImgFrame;


    public Slider hpBar;


    public Image gunImg;
    public Image gunImgFrame;


    public Image shieldImg;
    public Image shieldImgFrame;


    private UnitChangeUIManager.SetSelectedListener selectedListener;

    public void Initialized(Unit unit, UnitChangeUIManager.SetSelectedListener listener)
    {
        if (unit == null) return;


        gameObject.SetActive(true);

        _Unit = unit;

        modelImg.sprite = unit.modelImg;

        hpBar.value = unit.status.hp / unit.status.maxHp;


        selectedListener = listener;


        gunImg.sprite = unit.currentGun.gunInfo.sprite;

        shieldImg.color = Color.white * (unit.status.hasShield ? 1 : .6f);
        
    }


    public void SetOff()
    {
        _Unit = null;
        selectedListener = null;

        gameObject.SetActive(false);

    }




    public override void SetInput(bool set, Vector2 pos)
    {
        if (set)
            selectedListener.Invoke(_Unit);
    }



}
