using UnityEngine;
using UnityEngine.UIElements;

public class UnitState
{


    /// <summary>
    /// ���� ������������ ��Ÿ���� ���°��Դϴ�.
    /// </summary>
    public State isAim      { get; private set; } = new State("IsAim", true);



    /// <summary>
    /// ���� ������������ ��Ÿ���� ���°��Դϴ�.
    /// </summary>
    public State isShot     { get; private set; } = new State("IsShot", false);


    public State isShotMotion { get; private set; } = new State("IsShotMotion", false);


    /// <summary>
    /// ���� �ǰ��� ���ϴ� �������� ��Ÿ���� ���°��Դϴ�.
    /// </summary>
    public State isHit      { get; private set; } = new State("IsHit", false);



    /// <summary>
    /// ���� ���� �ǰ��� ���ϴ� �������� ��Ÿ���� ���°��Դϴ�.
    /// </summary>
    public State isStagger  { get; private set; } = new State("IsStagger", false);



    /// <summary>
    /// ���� ���������� ��Ÿ���� ���°��Դϴ�.
    /// </summary>
    public State isJump     { get; private set; } = new State("IsJump", false);


    /// <summary>
    /// ������ �׾������� ��Ÿ���� ���°��Դϴ�.
    /// </summary>
    public State isDeath    { get; private set; } = new State("isDeath", false);


    /// <summary>
    /// ��밡 �׾��ų� �Ͽ� ��� �¸� ����� ���ϴ� �� ����ϴ� ���¸� ��Ÿ���� ���°��Դϴ�.
    /// </summary>
    public State isWait     { get; private set; } = new State("IsWait", false);


    public State isShield   { get; private set; } = new State("IsShield", false);



}
