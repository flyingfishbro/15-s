using UnityEngine;

public class AimInput : BattleInputSystem
{
    private bool isAimInput;
    private float startTouchYPos;
    private float currentTouchYPos;


    public override void SetInput(bool set, Vector2 pos)
    {
        if (set && !IsGamePaused)
        {
            if (!isAimInput)
            {
                startTouchYPos = pos.y;
            }

            currentTouchYPos = pos.y;

            isAimInput = true;
        }
        else
        {
            isAimInput = false;


        }
    }


    private void Update()
    {
        if (!isAimInput) return;

        if (startTouchYPos == currentTouchYPos) return;

        float addValue = (currentTouchYPos - startTouchYPos) * inputOptionInstance.aimInputWeight;


        playerController.AddAimRot(addValue);

        startTouchYPos = currentTouchYPos;

    }

}
