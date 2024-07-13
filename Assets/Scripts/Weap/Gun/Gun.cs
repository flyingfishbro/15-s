using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Gun : MonoBehaviour
{
    #region Static Default Status
    /// <summary>
    /// ���� ���� �⺻���� ����� ���Ȱ��� ��Ÿ���ϴ�. �ܺο��� ���� ������ �� �ֵ��� �Ͽ����ϴ�.
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
    /// �� �ڵ带 ��Ÿ���ϴ�. �ܺο��� ������ �� �ְ� �ڽĿ��� �������� ���� �ֽ��ϴ�.
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
    /// ���� ���� �⺻���� �������Դϴ�. ���� �����Ǵ� ������ �ʱ�ȭ�˴ϴ�.
    /// </summary>
    public GunInfo gunInfo {  get; private set; }

    /// <summary>
    /// ���� ������ �� ���ȿ� �����մϴ�.
    /// </summary>
    public GunStatus status => gunInfo.gunStatus;


    /// <summary>
    /// ���� ���� ����ϴ� ������ ��Ÿ���ϴ�.
    /// </summary>
    public Unit user { get; private set; }


    /// <summary>
    /// �Ÿ� �ݺ�� ���ط� �����ۼ�Ʈ�� ����մϴ�.
    /// </summary>
    /// <param name="distance">�ݹ� ������ Ÿ�ݵ� ����Ʈ ������ �Ÿ�</param>
    /// <returns></returns>
    protected virtual float CalReductionRate(float distance) => Mathf.Clamp(defaultStatus.damageReductionRate * distance, 0, defaultStatus.maxDamageReductionRate) * 0.01f;


    /// <summary>
    /// �ݹ� ������ Ÿ�� ����Ʈ�� �޾� ���ط��� ����մϴ�.
    /// </summary>
    /// <param name="shotPos">�ݹ� ����</param>
    /// <param name="collisionPos">Ÿ�� ����Ʈ</param>
    /// <returns></returns>
    public virtual float CalDamage(Vector3 shotPos, Vector3 collisionPos) => status.damage * (1 - CalReductionRate(Vector3.Distance(shotPos, collisionPos)));


    public virtual float CalShockFigure(Vector3 shotPos, Vector3 collisionPos) => status.shockWeight * (1 - Vector3.Distance(shotPos, collisionPos) * .025f);




    /// <summary>
    /// �� �������Դϴ�. ���� ���� �⺻ ���������� �޾� �ʱ�ȭ��ŵ�ϴ�.
    /// </summary>
    /// <param name="gunInfo">�� ����</param>
    protected virtual void GunInitialized(GunInfo gunInfo)
    {
        this.gunInfo = gunInfo;
    }


    /// <summary>
    /// ������ �����ϰ� ���� �������θ� ��ȯ�մϴ�.
    /// </summary>
    /// <param name="newUser">���� ������ ����</param>
    /// <returns></returns>
    public bool SetUser(Unit newUser)
    {
        if (newUser == null) return false;

        if (user != null) return false;

        user = newUser; 

        return true;
    }


    /// <summary>
    /// ���� ���� �������� �Ǵ��մϴ�. �ڽĿ� ���� �����ǵ˴ϴ�.
    /// </summary>
    protected virtual bool CanShot => true;


    protected virtual bool hasShotMotion => true;

    /// <summary>
    /// On/Off�� ���� �������θ� ��ȯ�ϸ� ����������� ���� ������(���ݼӵ�, ���� ��) �ð��� out�Ű������� �Ҵ��մϴ�.
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
