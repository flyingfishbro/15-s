using System.Collections.Generic;
using UnityEngine;

public class State
{
    /// <summary>
    /// 현 상태의 이름을 의미합니다.
    /// </summary>
    public string stateName { get; private set; }
    public State(string name, bool initValue)
    {
        stateName = name;
        state = initValue;
    }


    /// <summary>
    /// 현 상태에 bool값을 나타냅니다.
    /// </summary>
    public bool state { get; private set; }



    public delegate void SetStateDelegate();

    /// <summary>
    /// 상태가 True값으로 변경될때마다 매번 호출되는 대리자입니다.
    /// </summary>
    private SetStateDelegate SetOnDelegate;

    /// <summary>
    /// 상태가 False값으로 변경될때마다 매번 호출되는 대리자입니다.
    /// </summary>
    private SetStateDelegate SetOffDelegate;


    /// <summary>
    /// 상태를 변경합니다. 상태가 성공적으로 변경되면 등록된 대리자가 있다면 호출합니다.
    /// </summary>
    /// <param name="set">목표하는 상태값</param>
    public void SetState(bool set)
    {
        if (state == set) { return; }

        if (set)    SetOnDelegate?.Invoke();
        else        SetOffDelegate?.Invoke();

        this.state = set;
    }


    /// <summary>
    /// State가 특정값으로 변경될때, 매번 호출되는 대리자를 등록합니다.
    /// </summary>
    /// <param name="set">기준이 되는 상태값</param>
    /// <param name="action">호출대상이 되는 대리자</param>
    public void AddSetStateStaticListener(bool set, SetStateDelegate action)
    {
        if (set)    SetOnDelegate += action;
        else        SetOffDelegate += action;
    }





}
