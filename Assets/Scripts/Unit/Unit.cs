using CartoonFX;
using System;
using System.Collections;
using UnityEngine;


public class Unit : Hitable
{
    #region Static 
    private static UnitDefaultStatus _UnitDefaultStatus;
    public static UnitDefaultStatus unitDefaultStatus => _UnitDefaultStatus ?? (_UnitDefaultStatus = Resources.Load<UnitDefaultStatus>("Scriptable/UnitDefaultStatus"));

    #endregion



    /// <summary>
    /// 이 유닛의 팀 타입을 나타냅니다.
    /// </summary>
    public Team.TeamType teamType;
    
    /// <summary>
    /// 자신이 속한 팀을 반환합니다.
    /// </summary>
    public Team team => BattleManager.instance.GetTeam(teamType);

    public UnitLoader.UnitModelType modelType;

    /// <summary>
    /// 이 유닛의 이미지파일입니다.
    /// </summary>
    public Sprite modelImg;

    /// <summary>
    /// 모델의 중심이 되는 오브젝트입니다.
    /// </summary>
    public Transform modelPos;


    /// <summary>
    /// 쉴드 오브젝트입니다.
    /// </summary>
    public Transform shieldPos;


    public bool isPlayerUnit => BattleManager.instance.playerTeamType == teamType;

    /// <summary>
    /// 현재 필드 유닛 중 이 유닛을 제외한 유닛(적군 유닛)을 반환합니다.
    /// </summary>
    public Unit enemyUnit => BattleManager.instance.GetOhterFieldUnit(teamType);

    /// <summary>
    /// 회전값입니다.
    /// </summary>
    private float turnSmoothVelocity;

    #region Physics
    private CapsuleCollider _SphereCollider;
    public CapsuleCollider sphereCollider => _SphereCollider ?? (GetComponent<CapsuleCollider>());

    private Rigidbody _Rb;
    public Rigidbody rb => _Rb ?? (_Rb = GetComponent<Rigidbody>());


    public bool isGrounded
        => Physics.SphereCast(transform.position, sphereCollider.radius, Vector3.down, out RaycastHit hitInfo, sphereCollider.height * .25f * 1.05f, 1 << 0);


    public bool isTooFar 
        => enemyUnit != null ? Vector3.Distance(enemyUnit.transform.position, transform.position) > BattleManager.unitMaxDistance : false;

    public bool isTooClose
        => enemyUnit != null ? Vector3.Distance(enemyUnit.transform.position, transform.position) < BattleManager.unitMinDistance : false;



    private bool isJumpCool;
    private Coroutine jumpCo;

    #endregion



    public UnitStatus status { get; private set; }


    public UnitState state { get; private set; }



    #region Status Listener

    public delegate void UnitStatusEvent(UnitStatus unitStatus);

    private UnitStatusEvent unitstatusEvent;

    public void InvokeUnitStatusEvent() => unitstatusEvent?.Invoke(status);

    public void AddUnitStatusEventListener(UnitStatusEvent eventer)
    {
        unitstatusEvent += eventer;
    }


    #endregion



    private UnitPartialManager _Partial;

    public UnitPartialManager partial => _Partial ?? (_Partial = GetComponentInChildren<UnitPartialManager>());




    public enum HandleDirection
    {
        LEFT,
        RIGHT
    }

    public HandleDirection handleDirection;

    public Transform[] leftHandlePos;
    public Transform[] rightHandlePos;

    public Transform[] GetHandlePos() 
        => handleDirection == HandleDirection.LEFT ? leftHandlePos : rightHandlePos;



    public Gun currentGun;




    public override void Initialized()
    {
        base.Initialized();

        if (status == null)
        {
            status = new UnitStatus(unitDefaultStatus);
        }


        state = new UnitState();

        SetGun(currentGun);

    }

    public void SetUnitStatus(UnitStatus unitStatus)
    {
        status = unitStatus;
    }


    private void Update()
    {
        if (!status.IsAlive) return;

        //SetRot은 현재 배틀이 진행중이 아니여도 정상적으로 동작해야함.
        SetUnitRot();

        if (!BattleManager.instance.isBattle) return;
        DistanceJump();



    }

    public void SetGun(Gun gun)
    {
        if (gun == null) return;
        if (!gun.SetUser(this)) return;

        currentGun = gun;
        partial.aimManager.SetGun(gun);

    }
    private void SetUnitRot()
    {
        if (enemyUnit == null) return;

        Vector3 dir = (enemyUnit.transform.position - transform.position).normalized;
        float targetAngle = MathF.Atan2(dir.x, dir.z) * Mathf.Rad2Deg;
        float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, 0.1f);

        transform.rotation = Quaternion.Euler(0, angle, 0);

    }



    public override bool OnDamaged(AttackIntend attackIntend, int hitBoxType) => partial.hitProcess.OnDamaged(attackIntend, hitBoxType);



    #region Controller Input


    public void SetAttackInput(bool set)
    {
        partial.attackProcess.SetAttack(set);
    }
    

    public void SetAimRot(float value)
    {
        partial.aimManager.SetAimRot(value);
    }

    public void AddAimRot(float value) => SetAimRot(partial.aimManager.currentAimRot + value);


    public void OnShield()
    {
        if (status.hasShield)
        {
            status.usedShield();
            state.isShield.SetState(true);
        }
    }

    #endregion


    #region Jump

    /// <summary>
    /// 현재 캐릭터간에 거리를 조절해줍니다. 너무 멀면 다가가고, 너무 가까우면 떨어집니다.
    /// </summary>
    private void DistanceJump()
    {
        if (isJumpCool) return; 
        if (enemyUnit == null) return;


        bool CanJump()
        {
            if (isTooFar)
            {
                return BattleManager.instance.jumpTargetTeam() == teamType && !state.isJump.state && isGrounded;
            }
            else
            {
                return !state.isJump.state;
            }

        }



        if (state.isJump.state)
        {
            if (rb.useGravity && rb.velocity.y == 0)
                state.isJump.SetState(false);
        }
        else
        {
            float jumpVelocity = 0;
            int dir = 0;

            if (isTooFar)
            {
                jumpVelocity = status.toCloseJumpPower;
                dir = 1;
            }          
            else if (isTooClose)
            {
                jumpVelocity = status.toFarJumpPower;
                dir = -1;
            }



            if (dir != 0 && CanJump())
            {
                rb.velocity = Vector3.zero;

                state.isJump.SetState(true);
                isJumpCool = true;


                Vector3 velocityDir = (enemyUnit.transform.position - transform.position).normalized * dir;

                float moveForce = 1.2f * GameManager.gravityCorrectionValue;

                Vector3 ret = ((velocityDir * moveForce + Vector3.up * 1.4f) * jumpVelocity);

                rb.AddForce(ret);
                SoundManager.Instance.Play("JetStart");


                if (jumpCo != null)
                {
                    StopCoroutine(jumpCo);
                    jumpCo = null;
                }
                jumpCo = StartCoroutine(JumpCoroutine());

            }
        }


    }
    private IEnumerator JumpCoroutine()
    {
        yield return new WaitForSeconds(0.1f);
        isJumpCool = false;
    }
    #endregion


    public void SetWait(bool set) => state.isWait.SetState(set);
}
