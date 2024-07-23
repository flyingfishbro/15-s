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
    /// ��ݰ��� ����մϴ�.
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


        //������ ó���� ���� �����մϴ�. �������� ü�¿� ����� �� ���� ������ ����ִ����� �Ǻ��� �ǰ�/������ ���� �����մϴ�.
        // ���� ������ ���
        float finalDamage = CalDamage(attackIntend.damage, (UnitHitInterface.HitBoxType)hitBoxType);

        // ������ ���� �� ����Ʈ ȣ��
        unit.status.SetHp(unit.status.hp - finalDamage);

        // �ǰ� ���� ����_mary
        unit.InvokeUnitStatusEvent();
        int rand = Random.Range(0, 6);
        SoundManager.Instance.Play($"Hit{rand}");


        //print($"is Stagger: {CalStagger(attackIntend.staggerPercent, (UnitHitInterface.HitBoxType)hitBoxType, finalDamage)}");

        // ��� �� ��� �� ����
        Vector3 shockDir =
            ((transform.position - attackIntend.hitPos).normalized) * CalShockFigure(attackIntend.shockFigure);

        // ���� ������ ���¿��� ���� ��� ���������� ���ư��鼭 �ױ⶧����(���������϶� ��ݰ� ����) ���ƿ��� �������� ��ݰ��� ������ ���ϰ� �ױ⿡ ����ġ ���� ��ġ���� �װԵ�.
        if (unit.status.IsAlive)
        {
            shockDir *= unit.isGrounded ? 1 : 0;
        }


        unit.rb.AddForce(shockDir);

        //������ ó�� �� ������ ���������
        if (unit.status.IsAlive)
        {

            unit.state.isHit.SetState(true);
            unit.state.isHit.SetState(false);


            // ���Կ� ���� ���� ����
            //unit.partial.aimManager.SetAimRot(unit.partial.aimManager.currentAimRot + attackIntend.weight);
            
            BattleManager.instance.GetBattleSceneUI().OnDamagedEffect(transform.position, finalDamage, hitBoxType);

        }

        //������ ó�� �� ������ �׾�����
        else
        {
            unit.state.isDeath.SetState(true);

            BattleManager.instance.RetireUnit(unit);

        }




        return true;
    }

    #endregion





}
