using System;
using System.Collections;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    #region Static Status
    private static BulletDefaultStatus _BulletDefaultStatus;
    public static BulletDefaultStatus bulletDefaultStatus => _BulletDefaultStatus ?? (_BulletDefaultStatus = Resources.Load<BulletDefaultStatus>("Scriptable/BulletDefaultStatus"));
    
    #endregion

    public Gun gun {  get; private set; }

    public BulletPoolObject bulletPoolObject;


    public bool isShot;

    protected Vector3 shotStartPoint;

    protected virtual float damage => gun.CalDamage(shotStartPoint, transform.position);

    protected virtual float shockFigure => gun.CalShockFigure(shotStartPoint, transform.position);


    protected float chargingRatio;




    protected virtual AttackIntend GetAttackIntend(Vector3 collisionPoint) 
        => new AttackIntend(damage, collisionPoint, shockFigure, bulletStatus.weight, gun.status.staggerWeight);



    public BulletStatus bulletStatus => gun.gunInfo.bulletStatus;

    private Coroutine lifeTimeCo;


    public void BulletInitialized(
        Gun gun,
        BulletPoolObject poolObject)
    {
        this.gun = gun;

        bulletPoolObject = poolObject;

    }




    #region Gravity
    private Vector3 gravity;
    private void SetGravity()
        => gravity += Physics.gravity * bulletStatus.weight * Time.fixedDeltaTime;

    #endregion


    #region Move
    protected Vector3 shotDir;

    protected Vector3 moveDir
        => (shotDir * bulletStatus.speed + gravity);

    private Vector3 nextPos
        => transform.position + moveDir * Time.fixedDeltaTime;



    #endregion


    #region Collision

    private LayerMask targetLayer => bulletDefaultStatus.structLayer | bulletDefaultStatus.hitableLayer;//bulletStatus.structLayerMask | bulletStatus.hitableLayerMask;

    private enum CollisionObjectType
    {
        STRUCT,
        HITABLE,
        INVALID
    }
    
    private bool CheckCollision( out RaycastHit hitInfo, out CollisionObjectType objectType, out HitableInterface hitable)
    {
        Vector3 start = transform.position;
        Vector3 dir = moveDir;

        float posDis = Vector3.Distance(transform.position, nextPos);
        float rayDis = posDis < bulletStatus.bulletRadius ? bulletStatus.bulletRadius : posDis;


        objectType = CollisionObjectType.INVALID;
        hitable = null;

        if (Physics.SphereCast(start, bulletStatus.bulletRadius, dir, out hitInfo, rayDis, targetLayer))
        {
            objectType = ConvertCollisionObjectType(hitInfo, out hitable);

            return objectType != CollisionObjectType.INVALID;
        }

        return false;
    }



    /// <summary>
    /// HitInfo의 타입을 정의하여 반환하고, 반환된 타입이 Hitable타입이라면 해당 오브젝트를 out변수에 할당하여 반환합니다.
    /// </summary>
    /// <param name="hitInfo"></param>
    /// <param name="hitable"></param>
    /// <returns></returns>
    private CollisionObjectType ConvertCollisionObjectType(RaycastHit hitInfo, out HitableInterface hitable)
    {
        int hitLayer = 1 << hitInfo.transform.gameObject.layer;

        int strcutLayer = (int)bulletDefaultStatus.structLayer;//(int)bulletStatus.structLayerMask;

        int hitableLayer = (int)bulletDefaultStatus.hitableLayer;//(int)bulletStatus.hitableLayerMask;

        hitable = null;

        if ((strcutLayer & hitLayer) != 0)
        {
            return CollisionObjectType.STRUCT;

        }
        else if ((hitableLayer & hitLayer) != 0) 
        {
            hitable = hitInfo.transform.GetComponent<HitableInterface>();

            //HitableInterface가 존재하지만 해당 건(Gun)의 유저와 ID가 같을시 유효하지 않은 충돌체로 판단합니다.
            if (hitable != null && !hitable.hitable.ID.Equals(gun.user.ID))
                return CollisionObjectType.HITABLE;

        }


        return CollisionObjectType.INVALID;
    }

    #endregion

    private void FixedUpdate()
    {
        if (isShot)
        {
            SetGravity();

            if (CheckCollision(out RaycastHit hitInfo, out CollisionObjectType objectType, out HitableInterface hitable))
            {
                transform.position = hitInfo.point - moveDir.normalized * bulletStatus.bulletRadius;


                CollisionEvent(hitInfo);
            }
            else
            {
                transform.position = nextPos;
            }


        }

    }


    public void Shot(Vector3 shotDirection)
    {
        Shot(shotDirection, 0);
    }


    public virtual void Shot(Vector3 shotDirection, float chargingRatio)
    {
        isShot = true;
        shotDir = shotDirection;
        shotStartPoint = transform.position;

        this.chargingRatio = chargingRatio;
    }


    protected virtual void CollisionEvent(RaycastHit hitInfo)
    {
        UnitHitInterface unit = hitInfo.transform.GetComponent<UnitHitInterface>();
        if (unit != null)
        {
            unit.OnDamaged(GetAttackIntend(hitInfo.point));
        }

        isShot = false;

        chargingRatio = 0;

        SetBullet(false);
        gun.CollisionEvent(hitInfo);
    }


    public virtual void SetBullet(bool set)
    {
        gameObject.SetActive(set);
        if (set)
        {
            if (lifeTimeCo != null)
            {
                StopCoroutine(lifeTimeCo);

                lifeTimeCo = null;
            }

            transform.parent = null;
            lifeTimeCo = StartCoroutine(LifeTimeTimer());
        }
        else
        {
            transform.parent = bulletPoolObject.transform;
            transform.localPosition = Vector3.zero;
        }

        gravity = Vector3.zero;

    }


    private IEnumerator LifeTimeTimer()
    {
        yield return null;

        yield return new WaitForSeconds(bulletStatus.lifeTime);

        lifeTimeCo = null;
        SetBullet(false);
    }



}
