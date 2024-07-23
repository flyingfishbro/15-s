using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static Unit;

public class UnitHitProcess : UnitPartial
{
    private void Start()
    {
        unit.state.isHit.AddSetStateStaticListener(true, OnHitState);
        unit.state.isHit.AddSetStateStaticListener(false, OffHitState);

        unit.state.isStagger.AddSetStateStaticListener(true, OnStaggerState);
        unit.state.isStagger.AddSetStateStaticListener(false, OffStaggerState);

        unit.state.isDeath.AddSetStateStaticListener(true, OnDeathState);
        unit.state.isDeath.AddSetStateStaticListener(false, OffDeathState);


    }

    #region Set HitState
    private void SetHitState(bool set)
    {
        if (set)
        {
            if (unit.isPlayerUnit || true)
                BattleManager.instance.GetBattleCam().CamShakeEffect(.05f, .1f);

        }
        else
        {

        }
    }

    private void OnHitState() => SetHitState(true);
    private void OffHitState() => SetHitState(false);


    #endregion


    #region Set Stagger
    
    private void SetStaggerState(bool set)
    {
        if (set)
        {
            unit.state.isHit.SetState(true);


            if (unit.state.isShot.state) unit.state.isShot.SetState(false);
            if (unit.state.isShotMotion.state) unit.state.isShotMotion.SetState(false);


            unit.state.isAim.SetState(false);
        }
        else
        {
            unit.state.isHit.SetState(false);



            unit.state.isAim.SetState(true);
        }
    }

    private void OnStaggerState() => SetStaggerState(true);
    private void OffStaggerState() => SetStaggerState(false);

    #endregion




    private void SetDeathState(bool set)
    {
        if (set)
        {
            if (unit.isTooClose && unit.rb.velocity.magnitude < .1f) 
                unit.rb.AddForce((unit.transform.position - unit.enemyUnit.transform.position).normalized * unit.status.toFarJumpPower * GameManager.gravityCorrectionValue);

            if (unit.state.isShot.state) unit.state.isShot.SetState(false);
            if (unit.state.isShotMotion.state) unit.state.isShotMotion.SetState(false);

            if (unit.state.isStagger.state) unit.state.isStagger.SetState(false);

            if (unit.state.isJump.state) unit.state.isJump.SetState(false);



            unit.partial.animManager.OnDeathMotion();

            unit.state.isAim.SetState(false);
        }
        else 
        {

        }
    }

    private void OnDeathState() => SetDeathState(true);
    private void OffDeathState() => SetDeathState(false);



    #region Damaged


    /// <summary>
    /// 충격값을 계산합니다.
    /// </summary>
    /// <param name="finalDamage"></param>
    /// <param name="shockFigure"></param>
    /// <returns></returns>
    private float CalShockFigure(float shockFigure)
    {
        return Mathf.Clamp(shockFigure, Gun.defaultStatus.minShockFigure, shockFigure);
    }


    public virtual float CalDamage(float damage, UnitHitInterface.HitBoxType hitBoxType)
    {
        float ret = hitBoxType == UnitHitInterface.HitBoxType.HEAD ? damage * Unit.unitDefaultStatus.headShotAddDamage : damage;

        return Mathf.Round(ret);
    }

    public virtual bool CalStagger(float staggerPercent, UnitHitInterface.HitBoxType hitBoxType, float finalDamage)
    {
        int randNum = Random.Range(1, 101);

        float partPer = hitBoxType == UnitHitInterface.HitBoxType.HEAD ? Unit.unitDefaultStatus.staggerPer_Head : Unit.unitDefaultStatus.staggerPer_Body;

        float dmgPer = finalDamage / 2;

        float percent = (partPer + dmgPer) * staggerPercent;

        return randNum <= percent;
    }



    public bool OnDamaged(AttackIntend attackIntend, int hitBoxType)
    {
        if (!unit.status.IsAlive) return false;

        if (!BattleManager.instance.damageIsValid(unit)) return false;

        if (unit.state.isShield.state) return false;


        //데미지 처리를 먼저 진행합니다. 데미지가 체력에 적용된 후 현재 유닛이 살아있는지를 판별후 피격/죽음을 따로 진행합니다.
        // 최종 데미지 계산
        float finalDamage = CalDamage(attackIntend.damage, (UnitHitInterface.HitBoxType)hitBoxType);

        // 데미지 적용 및 이펙트 호출
        unit.status.SetHp(unit.status.hp - finalDamage);

        // 피격 사운드 적용_mary
        unit.InvokeUnitStatusEvent();
        int rand = Random.Range(0, 6);
        SoundManager.Instance.Play($"Hit{rand}");


        //print($"is Stagger: {CalStagger(attackIntend.staggerPercent, (UnitHitInterface.HitBoxType)hitBoxType, finalDamage)}");

        // 충격 값 계산 및 적용
        Vector3 shockDir =
            ((transform.position - attackIntend.hitPos).normalized) * CalShockFigure(attackIntend.shockFigure);

        // 점프 상태인 상태에서 죽을 경우 상대방향으로 날아가면서 죽기때문에(점프상태일때 충격값 무시) 날아오는 과정에서 충격값이 상쇄되지 못하고 죽기에 적절치 못한 위치에서 죽게됨.
        if (unit.status.IsAlive)
        {
            shockDir *= unit.isGrounded ? 1 : 0;
        }


        unit.rb.AddForce(shockDir);

        //데미지 처리 후 유닛이 살아있을때
        if (unit.status.IsAlive)
        {

            unit.state.isHit.SetState(true);
            unit.state.isHit.SetState(false);


            // 무게에 따른 에임 방해
            //unit.partial.aimManager.SetAimRot(unit.partial.aimManager.currentAimRot + attackIntend.weight);
            
            BattleManager.instance.GetBattleSceneUI().OnDamagedEffect(transform.position, finalDamage, hitBoxType);

        }

        //데미지 처리 후 유닛이 죽었을때
        else
        {
            unit.state.isDeath.SetState(true);

            BattleManager.instance.RetireUnit(unit);

        }




        return true;
    }

    #endregion





}
