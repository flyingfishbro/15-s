using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartButton : ProgressInputSystem
{


    public Test test;


    public override void SetInput(bool set, Vector2 pos)
    {
        if (set)
            test.started = true;
    }


    public void CALLBACK_OnStartButtonClicked()
    {

        test.started = true;
    }
}
