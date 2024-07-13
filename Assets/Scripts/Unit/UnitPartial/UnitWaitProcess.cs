using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitWaitProcess : UnitPartial
{

    private void Start()
    {
        unit.state.isWait.AddSetStateStaticListener(true, OnWaitState);
        unit.state.isWait.AddSetStateStaticListener(false, OffWaitState);
    }

    private void SetWaitState(bool set)
    {
        if (set)
        {

            if (unit.state.isShot.state) unit.state.isShot.SetState(false);

            if (unit.state.isShotMotion.state) unit.state.isShotMotion.SetState(false);


            if (unit.state.isStagger.state) unit.state.isStagger.SetState(false);

            if (unit.state.isJump.state) unit.state.isJump.SetState(false);

            unit.partial.animManager.SetWaitMotion(true);


            unit.state.isAim.SetState(false);
        }
        else
        {


            unit.partial.animManager.SetWaitMotion(false);

            unit.state.isAim.SetState(true);

        }
    } 

    private void OnWaitState() => SetWaitState(true);
    private void OffWaitState() => SetWaitState(false);

}
