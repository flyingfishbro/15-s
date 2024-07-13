using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BattleMode : MonoBehaviour
{
    protected BattleManager battleManager => BattleManager.instance;


    public enum ModeType
    {
        NONE,
        ONE_TO_ONE,
        THREE_TO_THREE
    }

    public virtual ModeType mode => ModeType.NONE;


    public delegate void SetChoosedNextUnitDelegate(Unit unit);



    public virtual void Initialized() { }




    public abstract void SetFieldUnit(Unit unit) ;

    public abstract void RetireUnit(Unit unit);


}
