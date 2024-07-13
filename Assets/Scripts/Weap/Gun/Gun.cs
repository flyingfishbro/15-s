using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Gun : MonoBehaviour
{
    #region Static Default Status
    /// <summary>
    /// 건의 대한 기본적인 공통된 스탯값을 나타냅니다. 외부에서 쉽게 접근할 수 있도록 하였습니다.
    /// </summary>
    private static GunDefaultStatus _DefaultStatus;
    public static GunDefaultStatus defaultStatus => _DefaultStatus ?? (_DefaultStatus = Resources.Load<GunDefaultStatus>("Scriptable/GunDefaultStatus"));

    #endregion

    
    public static Gun CreateGunInstance(string gunCode)
    {
        GunInfo infoInstance = Resources.Load<GunInfoScriptableObject>("Scriptable/GunInfo").FindGunInfo(gunCode);

        string path = $"Weapon/Gun/{infoInstance.gunName}/{infoInstance.gunName}";

        Gun ret = Instantiate(Resources.Load<Gun>(path));
        ret.GunInitialized(infoInstance);
        return ret;
    }




    /// <summary>
    /// 건 코드를 나타냅니다. 외부에서 설정할 수 있고 자식에서 재정의할 수도 있습니다.
    /// </summary>
    [SerializeField] string _GunCode = "000000";
    public virtual string gunCode => _GunCode;

    protected virtual string gunPathBase => $"Weapon/Gun/{gunInfo.gunName}";
    



    public enum ShotType
    {
        INSTANT,
        HOLD,
        CHARGING
    }

    /// <summary>
    /// 건의 대한 기본적인 인포값입니다. 건이 생성되는 시점에 초기화됩니다.
    /// </summary>
    public GunInfo gunInfo {  get; private set; }

    /// <summary>
    /// 건의 인포값 중 스탯에 접근합니다.
    /// </summary>
    public GunStatus status => gunInfo.gunStatus;


    /// <summary>
    /// 현재 건을 사용하는 유저를 나타냅니다.
    /// </summary>
    public Unit user { get; private set; }


    /// <summary>
    /// 거리 반비례 피해량 차감퍼센트를 계산합니다.
    /// </summary>
    /// <param name="distance">격발 시점과 타격된 포인트 사이의 거리</param>
    /// <returns></returns>
    protected virtual float CalReductionRate(float distance) => Mathf.Clamp(defaultStatus.damageReductionRate * distance, 0, defaultStatus.maxDamageReductionRate) * 0.01f;


    /// <summary>
    /// 격발 시점과 타격 포인트를 받아 피해량을 계산합니다.
    /// </summary>
    /// <param name="shotPos">격발 시점</param>
    /// <param name="collisionPos">타격 포인트</param>
    /// <returns></returns>
    public virtual float CalDamage(Vector3 shotPos, Vector3 collisionPos) => status.damage * (1 - CalReductionRate(Vector3.Distance(shotPos, collisionPos)));


    public virtual float CalShockFigure(Vector3 shotPos, Vector3 collisionPos) => status.shockWeight * (1 - Vector3.Distance(shotPos, collisionPos) * .025f);




    /// <summary>
    /// 건 생성자입니다. 건의 대한 기본 정보값들을 받아 초기화시킵니다.
    /// </summary>
    /// <param name="gunInfo">건 인포</param>
    protected virtual void GunInitialized(GunInfo gunInfo)
    {
        this.gunInfo = gunInfo;
    }


    /// <summary>
    /// 유저를 설정하고 그의 성공여부를 반환합니다.
    /// </summary>
    /// <param name="newUser">새로 설정될 유저</param>
    /// <returns></returns>
    public bool SetUser(Unit newUser)
    {
        if (newUser == null) return false;

        if (user != null) return false;

        user = newUser; 

        return true;
    }


    /// <summary>
    /// 현재 샷이 가능한지 판단합니다. 자식에 따라 재정의됩니다.
    /// </summary>
    protected virtual bool CanShot => true;


    protected virtual bool hasShotMotion => true;

    /// <summary>
    /// On/Off의 따라 성공여부를 반환하며 성공했을경우 남은 딜레이(공격속도, 어택 텀) 시간을 out매개변수에 할당합니다.
    /// </summary>
    /// <param name="set"></param>
    /// <param name="delay"></param>
    /// <returns></returns>
    public virtual bool OnShot(bool set, out float delay, out bool shotMotion)
    {
        delay = 0f;
        shotMotion = hasShotMotion;
        return true;
    }




    public virtual void CollisionEvent(RaycastHit hitInfo) { }

}
