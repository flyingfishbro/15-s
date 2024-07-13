using UnityEngine;
using UnityEngine.UIElements;

public class UnitState
{


    /// <summary>
    /// 현재 에임중인지를 나타내는 상태값입니다.
    /// </summary>
    public State isAim      { get; private set; } = new State("IsAim", true);



    /// <summary>
    /// 현재 공격중인지를 나타내는 상태값입니다.
    /// </summary>
    public State isShot     { get; private set; } = new State("IsShot", false);


    public State isShotMotion { get; private set; } = new State("IsShotMotion", false);


    /// <summary>
    /// 현재 피격을 당하는 중인지를 나타내는 상태값입니다.
    /// </summary>
    public State isHit      { get; private set; } = new State("IsHit", false);



    /// <summary>
    /// 현재 강한 피격을 당하는 중인지를 나타내는 상태값입니다.
    /// </summary>
    public State isStagger  { get; private set; } = new State("IsStagger", false);



    /// <summary>
    /// 현재 점프중인지 나타내는 상태값입니다.
    /// </summary>
    public State isJump     { get; private set; } = new State("IsJump", false);


    /// <summary>
    /// 유닛이 죽었는지를 나타내는 상태값입니다.
    /// </summary>
    public State isDeath    { get; private set; } = new State("isDeath", false);


    /// <summary>
    /// 상대가 죽었거나 하여 잠시 승리 모션을 취하는 등 대기하는 상태를 나타내는 상태값입니다.
    /// </summary>
    public State isWait     { get; private set; } = new State("IsWait", false);


    public State isShield   { get; private set; } = new State("IsShield", false);



}
