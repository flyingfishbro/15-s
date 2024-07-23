using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitChangeUIManager : MonoBehaviour
{

    public List<UnitChangingChooseInput> chooseInputList;

    private BattleMode.SetChoosedNextUnitDelegate choosedListener;

    
    
    private Unit selectedUnit;

    public delegate void SetSelectedListener(Unit unit);

    private void SetSelectedUnit(Unit unit)
    {
        SoundManager.Instance.Play("ButtonClick");

        //이미 먼저 입력을 받았을 경우 더이상 입력을 받지 않음.
        if (selectedUnit != null) return;

        selectedUnit = unit;
    }



    public IEnumerator ChoosingUnitInputCoroutine(List<Unit> waitList, BattleMode.SetChoosedNextUnitDelegate listener)
    {

        selectedUnit = null;

        while (selectedUnit == null)
        {

            int index = 0;
            for (; index < waitList.Count; ++index)
            {
                chooseInputList[index].Initialized(waitList[index], SetSelectedUnit);
            }
            for (; index < chooseInputList.Count; ++index)
            {
                chooseInputList[index].SetOff();
            }

            yield return null;
        }


        for (int i = 0; i < chooseInputList.Count; ++i)
        {
            chooseInputList[i].SetOff();
        }

        listener.Invoke(selectedUnit);

    }

}
