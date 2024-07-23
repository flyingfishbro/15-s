using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestBot : MonoBehaviour
{
    private UnitController _Controller;
    private UnitController controller => _Controller ?? (_Controller = BattleManager.instance.GetUnitController(BattleManager.instance.enemyTeamType)); 

    private bool canAttack = false;

    public float attackTermWeight = 3f;

    public Gun GetCurrentGun()
    {
        Unit unit = BattleManager.instance.GetFieldUnit(controller.targetTeamType);
        if (!unit) return null;


        return unit.currentGun;
    }



    private Coroutine attackTermCo;


    private bool botTeam;

    private void Start()
    {
        attackTermCo = StartCoroutine(AttackTermCoroutine());
        SoundManager.Instance.Play("Ready");
    }

    // Update is called once per frame
    private void Update()
    {
        Unit targetUnit = BattleManager.instance.GetFieldUnit(BattleManager.instance.enemyTeamType);
        Unit enemyUnit = BattleManager.instance.GetFieldUnit(BattleManager.instance.playerTeamType);
        
        if (controller == null ||
            targetUnit == null ||
            enemyUnit == null) return;


        Vector3 xzPos1 = targetUnit.transform.position;
        Vector3 xzPos2 = enemyUnit.transform.position;

        float height = xzPos1.y - xzPos2.y;



        xzPos1.y = 0;

        xzPos2.y = 0;

        float width = Vector3.Distance(xzPos1, xzPos2);


        float targetAngle = -Mathf.Atan2(height, width) * 180 / Mathf.PI;

        controller.SetAimRot(targetAngle);

        //targetUnit.partial.aimManager.SetAimRot(-targetAngle);


        Attack();


    }


    private void Attack()
    {
        if (controller == null) return;

        if (canAttack)
        {
            canAttack = false;

            controller.SetAttackInput(true);

            if (attackTermCo != null)
            {
                StopCoroutine(attackTermCo);
                attackTermCo = null;
            }
            attackTermCo = StartCoroutine(AttackTermCoroutine());

            if (GetCurrentGun().gunInfo.type != Gun.ShotType.HOLD)
                controller.SetAttackInput(false);
        }

    }


    private IEnumerator AttackTermCoroutine()
    {
        yield return new WaitForSeconds(GetCurrentGun().status.attackSpeed * attackTermWeight);
        canAttack = true;
    }



}
