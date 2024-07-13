using System.Collections.Generic;
using UnityEngine;

public class State
{
    /// <summary>
    /// �� ������ �̸��� �ǹ��մϴ�.
    /// </summary>
    public string stateName { get; private set; }
    public State(string name, bool initValue)
    {
        stateName = name;
        state = initValue;
    }


    /// <summary>
    /// �� ���¿� bool���� ��Ÿ���ϴ�.
    /// </summary>
    public bool state { get; private set; }



    public delegate void SetStateDelegate();

    /// <summary>
    /// ���°� True������ ����ɶ����� �Ź� ȣ��Ǵ� �븮���Դϴ�.
    /// </summary>
    private SetStateDelegate SetOnDelegate;

    /// <summary>
    /// ���°� False������ ����ɶ����� �Ź� ȣ��Ǵ� �븮���Դϴ�.
    /// </summary>
    private SetStateDelegate SetOffDelegate;


    /// <summary>
    /// ���¸� �����մϴ�. ���°� ���������� ����Ǹ� ��ϵ� �븮�ڰ� �ִٸ� ȣ���մϴ�.
    /// </summary>
    /// <param name="set">��ǥ�ϴ� ���°�</param>
    public void SetState(bool set)
    {
        if (state == set) { return; }

        if (set)    SetOnDelegate?.Invoke();
        else        SetOffDelegate?.Invoke();

        this.state = set;
    }


    /// <summary>
    /// State�� Ư�������� ����ɶ�, �Ź� ȣ��Ǵ� �븮�ڸ� ����մϴ�.
    /// </summary>
    /// <param name="set">������ �Ǵ� ���°�</param>
    /// <param name="action">ȣ������ �Ǵ� �븮��</param>
    public void AddSetStateStaticListener(bool set, SetStateDelegate action)
    {
        if (set)    SetOnDelegate += action;
        else        SetOffDelegate += action;
    }





}
