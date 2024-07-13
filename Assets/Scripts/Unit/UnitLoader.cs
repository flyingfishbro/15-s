using Unity.VisualScripting;
using UnityEngine;

public class UnitLoader : Loader<UnitLoader>
{
    private GameObject retUnit;

    #region private
    private Transform model;
    private Transform partial;
    private Transform aimPos;


    private UnitModel modelComponent;

    private Transform shield;


    private int Layer(LayerMask layerMask) => (int)Mathf.Log(layerMask.value, 2);


    private string GetModelPath()
    {
        string ret = "Unit/Model/Model_";
        switch (_LoadInfo.unitModelType)
        {
            case UnitModelType.V1: ret += "V1"; 
                break;

            case UnitModelType.V2: ret += "V2";
                break;

            case UnitModelType.V3: ret += "V3";
                break;

            case UnitModelType.V4: ret += "V4";
                break;

            case UnitModelType.V5: ret += "V5";
                break;

            default: ret += "V6";
                break;
        }
        return ret;
    }


    public static string GetImgPath(UnitLoadInfo _LoadInfo)
    {
        string ret = "UI/Model/Image/Model_";
        switch (_LoadInfo.unitModelType)
        {
            case UnitModelType.V1:
                ret += "V1";
                break;

            case UnitModelType.V2:
                ret += "V2";
                break;

            case UnitModelType.V3:
                ret += "V3";
                break;

            case UnitModelType.V4:
                ret += "V4";
                break;

            case UnitModelType.V5:
                ret += "V5";
                break;

            default:
                ret += "V6";
                break;
        }
        ret += "_Img";
        return ret;
    }
    #endregion


    public enum UnitModelType
    {
        V1,
        V2,
        V3,
        V4,
        V5,
        V6
    }

    public struct UnitLoadInfo
    {
        public Team.TeamType unitTeamType { get; private set; }

        public UnitModelType unitModelType {  get; private set; }
        
        public Unit.HandleDirection handleDirection { get; private set; }

        public UnitStatus unitStatus { get; private set; }

        public string gunCode { get; private set; }    


        public UnitLoadInfo(Team.TeamType teamType, UnitModelType modelType, Unit.HandleDirection handleDir, UnitStatus unitStatus, string gunCode)
        {
            this.unitTeamType = teamType;
            this.unitModelType = modelType;
            this.handleDirection = handleDir;   
            this.unitStatus = unitStatus;
            this.gunCode = gunCode;
        }
    }


    #region Setting


    public UnitLoadInfo _LoadInfo {  get; private set; }

    /*
    public Team.TeamType teamType { get; private set; }

    public UnitModelType ModelType { get; private set; }

    public Unit.HandleDirection handleDirection {  get; private set; }

    public string gunCode { get; private set; }
    */

    #endregion

    public Unit GetUnitInstance(UnitLoadInfo loadInfo)
    {
        _LoadInfo = loadInfo;

        /*
        this.teamType = loadInfo.unitTeamType;
        this.ModelType = loadInfo.unitModelType;
        this.handleDirection = loadInfo.handleDirection;
        this.gunCode = loadInfo.gunCode;
        */

        SetModel();
        SetComponent();
        SetCollision();

        Unit ret = retUnit.GetComponent<Unit>();
        ret.Initialized();

        retUnit = null;
        return ret;
    }


    #region Set Func
    /// <summary>
    /// ���ҽ��κ��� ���� �ҷ����� �ʿ��� ������Ʈ�� �����Ͽ� ��ȿ�� ������ ��ġ�� ������ �����մϴ�.
    /// </summary>
    private void SetModel()
    {
        // ��ȯ�� ���ο� ������Ʈ�� �����մϴ�.
        retUnit = new GameObject("NewUnitInstance");

        //���� �ҷ��ɴϴ�.
        GameObject model = Instantiate(Resources.Load<GameObject>(GetModelPath()));

        //�ڵ� ���⿡ ���� ���� �������� �����մϴ�.
        model.transform.SetParent(retUnit.transform);
        model.transform.localPosition = Vector3.down;
        model.transform.localRotation = Quaternion.Euler(new Vector3(0, 30, 0) * (_LoadInfo.handleDirection == Unit.HandleDirection.LEFT ? -1 : 1));

        //�ڵ� ���⿡ ���� ���� �ִ� ��Ʈ�ѷ��� �ҷ��� �����մϴ�.
        string animControllerPath = "Unit/Animation/UnitAnimController_";
        animControllerPath += _LoadInfo.handleDirection == Unit.HandleDirection.LEFT ? "Left" : "Right";
        model.GetComponent<Animator>().runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>(animControllerPath);

        this.model = model.transform;



        //������ �ۼ��� �������� �������� �����մϴ�.
        GameObject partial = new GameObject("UnitPartial");
        partial.transform.SetParent(retUnit.transform);
        partial.transform.localPosition = Vector3.zero;

        this.partial = partial.transform;


        //������ �������� �������� �����մϴ�.
        GameObject aim = new GameObject("AimPos");
        aim.transform.SetParent(retUnit.transform);
        aim.transform.localPosition = Vector3.zero;

        //�ڵ��� ��/�� �κ��� ����, �����մϴ�.
        GameObject aimHandle_Hand = new GameObject("HandlePos");
        aimHandle_Hand.transform.SetParent(aim.transform);

        GameObject aimHandle_Arm = new GameObject("ArmPos");
        aimHandle_Arm.transform.SetParent(aim.transform);


        //�ڵ� ������ ���� �� �ڵ��� ������ �����ǰ����� �����մϴ�.
        if (_LoadInfo.handleDirection == Unit.HandleDirection.LEFT)
        {
            aimHandle_Hand.transform.localPosition = new Vector3(-.3f, -.25f, .6f);
            aimHandle_Arm.transform.localPosition = new Vector3(-.3f, -.3f, .45f);

            aimHandle_Hand.transform.localRotation = Quaternion.Euler(new Vector3(0, -90, 0));
            aimHandle_Arm.transform.localRotation = Quaternion.Euler(new Vector3(0, -90, 0));
        }
        else
        {
            aimHandle_Hand.transform.localPosition = new Vector3(.13f, -.15f, .6f);
            aimHandle_Arm.transform.localPosition = new Vector3(.3f, -.15f, .45f);

            aimHandle_Hand.transform.localRotation = Quaternion.Euler(new Vector3(0, 90, 0));
            aimHandle_Arm.transform.localRotation = Quaternion.Euler(new Vector3(0, 90, 0));
        }

        this.aimPos = aim.transform;





        shield = Instantiate(Resources.Load<Transform>("Unit/Shield/UnitShield"));
        shield.SetParent(model.GetComponent<UnitModel>().head.transform);
        shield.localPosition = new Vector3(0, .1f, 0);
        shield.localScale = Vector3.one;
        shield.gameObject.SetActive(false);

    }


    /// <summary>
    /// �� �ʿ��� ������Ʈ�� ��ȿ�� ��ġ�� ����մϴ�.
    /// </summary>
    private void SetComponent()
    {
        //������ �浹������ ����ϴ� ĸ���� ������ �ٵ� �����ϰ� �����մϴ�.
        CapsuleCollider capsule = retUnit.transform.AddComponent<CapsuleCollider>();
        capsule.radius = 0.5f;
        capsule.height = 2;

        Rigidbody rb = retUnit.transform.AddComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeAll ^ (RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionY);



        //���� ������Ʈ�� ����մϴ�.
        Unit unit = retUnit.transform.AddComponent<Unit>();

        unit.teamType = _LoadInfo.unitTeamType;

        unit.modelType = _LoadInfo.unitModelType;

        unit.modelImg = Resources.Load<Sprite>(GetImgPath(_LoadInfo));


        unit.handleDirection = _LoadInfo.handleDirection;

        unit.SetUnitStatus(_LoadInfo.unitStatus);


        //�� ������Ʈ�� �ҷ��ɴϴ�.
        modelComponent = model.GetComponent<UnitModel>();


        unit.modelPos = modelComponent.modelPos;


        //�ҷ��� �� ������Ʈ�κ��� �� �ڵ��� �ڵ������ǵ��� �޽��ϴ�.
        unit.leftHandlePos = modelComponent.HandlePos_Left;
        unit.rightHandlePos = modelComponent.HandlePos_Right;




        //�ִ��� ����մϴ�.
        UnitAnimManager animManager = this.model.AddComponent<UnitAnimManager>();
        animManager._Unit = unit;


        //�ۼ� �Ŵ����� ����մϴ�.
        this.partial.AddComponent<UnitPartialManager>();


        //������ ����ϰ� �� ������ ������ �ʱ�ȭ��ŵ�ϴ�.
        UnitAimManager aimManager = this.partial.AddComponent<UnitAimManager>();
        aimManager.aimPos = this.aimPos;

        aimManager.gunHandlePos = new Transform[2];

        aimManager.gunHandlePos[0] = this.aimPos.GetChild(0);
        aimManager.gunHandlePos[1] = this.aimPos.GetChild(1);


        //���� ���μ����� ����մϴ�.
        UnitAttackProcess attackProcess = this.partial.AddComponent<UnitAttackProcess>();

        //�ǰ� ���μ����� ����մϴ�.
        UnitHitProcess hitProcess = this.partial.AddComponent<UnitHitProcess>();


        //���� ���μ����� ����մϴ�.
        UnitWaitProcess waitProcess = this.partial.AddComponent<UnitWaitProcess>();


        //����Ʈ �Ŵ����� ����մϴ�.
        UnitEffectManager effectManager = this.partial.AddComponent<UnitEffectManager>();


        ParticleSystem jetpackEffect = Instantiate(Resources.Load<ParticleSystem>("Unit/Jetpack/JetpackEffect"));
        jetpackEffect.transform.SetParent(modelComponent.backPack);
        jetpackEffect.transform.localPosition = new Vector3(-.05f, .11f, .25f);
        jetpackEffect.transform.localRotation = Quaternion.Euler(200, 0, 0);
        jetpackEffect.transform.localScale = new Vector3(.4f, .4f, .5f);

        effectManager.SetJetpackEffect(jetpackEffect);


        unit.shieldPos = shield;




        //������ ���� ����մϴ�.
        unit.SetGun(Gun.CreateGunInstance(_LoadInfo.gunCode));

    }


    /// <summary>
    /// ���̾�� �浹ü ������ �����մϴ�. ������ �ǰ������� ���� ������ ����˴ϴ�.
    /// </summary>
    private void SetCollision()
    {
        //������ ���̾�� �����մϴ�.
        retUnit.layer = Layer(Unit.unitDefaultStatus.unitLayer);

        //������ ��������� �ݶ��̴��� ������ٵ� ����ϰ� �����մϴ�.
        SphereCollider sphere = modelComponent.head.AddComponent<SphereCollider>();
        sphere.center = new Vector3(-.13f, 0.02f, 0);
        sphere.radius = .18f;

        Rigidbody rb = modelComponent.head.AddComponent<Rigidbody>();
        rb.useGravity = false;
        rb.constraints = RigidbodyConstraints.FreezeAll;

        //������ ��������� ���̾ �����մϴ�.
        modelComponent.head.gameObject.layer = Layer(Unit.unitDefaultStatus.unitHitLayer);

        //������ ����� HitInterface�� ����մϴ�.
        UnitHitInterface hitInterface = modelComponent.head.AddComponent<UnitHitInterface>();
        hitInterface.hitBoxType = UnitHitInterface.HitBoxType.HEAD;




        //������ �ٵ������� �ݶ��̴��� ������ٵ� ����ϰ� �����մϴ�.
        BoxCollider box = modelComponent.body.AddComponent<BoxCollider>();
        box.center = new Vector3(.08f, 0f, 0f);
        box.size = new Vector3(.35f, .3f, .3f);

        rb = modelComponent.body.AddComponent<Rigidbody>();
        rb.useGravity = false;
        rb.constraints = RigidbodyConstraints.FreezeAll;

        //������ �ٵ������� ���̾ �����մϴ�.
        modelComponent.body.gameObject.layer = Layer(Unit.unitDefaultStatus.unitHitLayer);

        //������ �ٵ��� HitInterface�� ����մϴ�.
        hitInterface = modelComponent.body.AddComponent<UnitHitInterface>();
        hitInterface.hitBoxType = UnitHitInterface.HitBoxType.BODY;
    }

    #endregion

}
