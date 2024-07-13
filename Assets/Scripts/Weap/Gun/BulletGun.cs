using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.VisualScripting;
using UnityEngine;

public class BulletGun : Gun
{
    /// <summary>
    /// 불렛이나 이펙트 풀링에 사용될 사이즈입니다.
    /// </summary>
    private const int DEFAULT_BULLET_COUNT = 10;

    #region Bullet
    /// <summary>
    /// 불렛 프리펩 입니다.
    /// </summary>
    private Bullet bulletPrefab => Resources.Load<Bullet>($"{gunPathBase}/Bullet/{gunInfo.gunName}_Bullet");

    protected BulletPoolObject bulletPoolObject;
    
    #endregion



    #region Effect

    /// <summary>
    /// 샷 이펙트 프리펩입니다.
    /// </summary>
    private EffectObject shotEffectPrefab => Resources.Load<EffectObject>($"{gunPathBase}/Effect/{gunInfo.gunName}_ShotEffect");
    protected EffectPoolObject shotEffectPoolObject;

    /// <summary>
    /// 콜리션 이펙트 프리펩입니다.
    /// </summary>
    private EffectObject collisionEffectPrefab => Resources.Load<EffectObject>($"{gunPathBase}/Effect/{gunInfo.gunName}_CollisionEffect");
    protected EffectPoolObject collisionEffectPoolObject;

    #endregion



    /// <summary>
    /// 현재 공격속도의 따른 공격 쿨타임인지 판단 기준이 되는 값입니다.
    /// </summary>
    public bool isAttackTerm { get; protected set; }


    #region AttackTerm Coroutine
    /// <summary>
    /// 공격속도 쿨타임에 사용될 코루틴입니다.
    /// </summary>
    protected Coroutine attackTermCo;
    private IEnumerator AttackTermCoroutine(float sec)
    {
        yield return new WaitForSeconds(sec);
        isAttackTerm = false;
    }

    #endregion





    /// <summary>
    /// 재정의된 샷 가능 판단 여부값입니다. 불렛 건의 특성에 따라 현재 공격속도 쿨타임이면 샷을 진행하지 못합니다.
    /// </summary>
    protected override bool CanShot => !isAttackTerm;

    protected virtual void BulletShot(Bullet bullet, Vector3 shotDirection) => bullet.Shot(shotDirection);


    public Transform shotPos;
    public Transform front, back;

    public Vector3 shotDir => (front.position - back.position).normalized;






    protected override void GunInitialized(GunInfo gunInfo)
    {
        base.GunInitialized(gunInfo);

        bulletPoolObject = BulletPoolObject.GetPoolObjectInstance(bulletPrefab, this, DEFAULT_BULLET_COUNT);

        shotEffectPoolObject = EffectPoolObject.GetPoolObjectInstance(shotEffectPrefab, "ShotEffect", transform, DEFAULT_BULLET_COUNT, EffectObject.PositionType.LOCAL);

        collisionEffectPoolObject = EffectPoolObject.GetPoolObjectInstance(collisionEffectPrefab, "CollisionEffect", transform, DEFAULT_BULLET_COUNT, EffectObject.PositionType.WORLD);
    }





    public override bool OnShot(bool set, out float delay, out bool shotMotion)
    {
        base.OnShot(set, out delay, out shotMotion);

        if (!set)
            return false;

        if (CanShot)
        {
            Bullet bullet = bulletPoolObject.GetBullet();
            bullet.transform.position = shotPos.position;
            bullet.transform.rotation = Quaternion.LookRotation(shotDir);

            BulletShot(bullet, shotDir);
            //bullet.Shot(shotDir);


            isAttackTerm = true;

            if (attackTermCo != null)
            {
                StopCoroutine(attackTermCo);
                attackTermCo = null;
            }
            attackTermCo = StartCoroutine(AttackTermCoroutine(status.attackSpeed));
            delay = status.attackSpeed;

            shotEffectPoolObject.OnEffect(shotPos.localPosition, Vector3.zero);


            return true;

        }


        return false;
    }




    public override void CollisionEvent(RaycastHit hitInfo)
    {
        base.CollisionEvent(hitInfo);
        collisionEffectPoolObject.OnEffect(hitInfo.point, hitInfo.normal);
    }


}
