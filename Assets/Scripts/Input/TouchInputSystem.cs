using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TouchInputSystem : MonoBehaviour
{
    public enum TouchInputType
    {
        NONE,
        BATTLE,
        PROGRESS
    }

    public virtual TouchInputType inputType => TouchInputType.NONE;


    private TouchInputManager _TouchInputManager;
    private TouchInputManager inputManager => _TouchInputManager ?? (_TouchInputManager = TouchInputManager.instance);


    protected virtual void Start() => RegisterInputSystem(this);


    protected virtual void RegisterInputSystem(TouchInputSystem inputSystem)
    {
        inputManager.RegisterInputSystem(inputSystem);
    }



    public abstract void SetInput(bool set, Vector2 pos);
    public virtual void SetInput(bool set) => SetInput(set, Vector2.zero);

}
