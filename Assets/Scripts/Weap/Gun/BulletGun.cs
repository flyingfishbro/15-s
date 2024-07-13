using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.VisualScripting;
using UnityEngine;

public class BulletGun : Gun
{
    /// <summary>
    /// �ҷ��̳� ����Ʈ Ǯ���� ���� �������Դϴ�.
    /// </summary>
    private const int DEFAULT_BULLET_COUNT = 10;

    #region Bullet
    /// <summary>
    /// �ҷ� ������ �Դϴ�.
    /// </summary>
    private Bullet bulletPrefab => Resources.Load<Bullet>($"{gunPathBase}/Bullet/{gunInfo.gunName}_Bullet");

    protected BulletPoolObject bulletPoolObject;
    
    #endregion



    #region Effect

    /// <summary>
    /// �� ����Ʈ �������Դϴ�.
    /// </summary>
    private EffectObject shotEffectPrefab => Resources.Load<EffectObject>($"{gunPathBase}/Effect/{gunInfo.gunName}_ShotEffect");
    protected EffectPoolObject shotEffectPoolObject;

    /// <summary>
    /// �ݸ��� ����Ʈ �������Դϴ�.
    /// </summary>
    private EffectObject collisionEffectPrefab => Resources.Load<EffectObject>($"{gunPathBase}/Effect/{gunInfo.gunName}_CollisionEffect");
    protected EffectPoolObject collisionEffectPoolObject;

    #endregion



    /// <summary>
    /// ���� ���ݼӵ��� ���� ���� ��Ÿ������ �Ǵ� ������ �Ǵ� ���Դϴ�.
    /// </summary>
    public bool isAttackTerm { get; protected set; }


    #region AttackTerm Coroutine
    /// <summary>
    /// ���ݼӵ� ��Ÿ�ӿ� ���� �ڷ�ƾ�Դϴ�.
    /// </summary>
    protected Coroutine attackTermCo;
    private IEnumerator AttackTermCoroutine(float sec)
    {
        yield return new WaitForSeconds(sec);
        isAttackTerm = false;
    }

    #endregion





    /// <summary>
    /// �����ǵ� �� ���� �Ǵ� ���ΰ��Դϴ�. �ҷ� ���� Ư���� ���� ���� ���ݼӵ� ��Ÿ���̸� ���� �������� ���մϴ�.
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
