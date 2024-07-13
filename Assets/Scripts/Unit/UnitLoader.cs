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
    /// 리소스로부터 모델을 불러오고 필요한 오브젝트를 생성하여 유효한 값으로 위치와 각도를 설정합니다.
    /// </summary>
    private void SetModel()
    {
        // 반환될 새로운 오브젝트를 생성합니다.
        retUnit = new GameObject("NewUnitInstance");

        //모델을 불러옵니다.
        GameObject model = Instantiate(Resources.Load<GameObject>(GetModelPath()));

        //핸들 방향에 따라 모델의 포지션을 설정합니다.
        model.transform.SetParent(retUnit.transform);
        model.transform.localPosition = Vector3.down;
        model.transform.localRotation = Quaternion.Euler(new Vector3(0, 30, 0) * (_LoadInfo.handleDirection == Unit.HandleDirection.LEFT ? -1 : 1));

        //핸들 방향에 따라 모델의 애님 컨트롤러를 불러와 설정합니다.
        string animControllerPath = "Unit/Animation/UnitAnimController_";
        animControllerPath += _LoadInfo.handleDirection == Unit.HandleDirection.LEFT ? "Left" : "Right";
        model.GetComponent<Animator>().runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>(animControllerPath);

        this.model = model.transform;



        //유닛의 퍼셜을 생성한후 포지션을 설정합니다.
        GameObject partial = new GameObject("UnitPartial");
        partial.transform.SetParent(retUnit.transform);
        partial.transform.localPosition = Vector3.zero;

        this.partial = partial.transform;


        //에임을 생성한후 포지션을 설정합니다.
        GameObject aim = new GameObject("AimPos");
        aim.transform.SetParent(retUnit.transform);
        aim.transform.localPosition = Vector3.zero;

        //핸들의 손/팔 부분을 생성, 설정합니다.
        GameObject aimHandle_Hand = new GameObject("HandlePos");
        aimHandle_Hand.transform.SetParent(aim.transform);

        GameObject aimHandle_Arm = new GameObject("ArmPos");
        aimHandle_Arm.transform.SetParent(aim.transform);


        //핸들 방향의 따라 각 핸들의 고유의 포지션값들을 세팅합니다.
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
    /// 각 필요한 컴포넌트를 유효한 위치에 등록합니다.
    /// </summary>
    private void SetComponent()
    {
        //유닛의 충돌판정을 담당하는 캡슐과 리지드 바디를 생성하고 설정합니다.
        CapsuleCollider capsule = retUnit.transform.AddComponent<CapsuleCollider>();
        capsule.radius = 0.5f;
        capsule.height = 2;

        Rigidbody rb = retUnit.transform.AddComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeAll ^ (RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionY);



        //유닛 컴포넌트를 등록합니다.
        Unit unit = retUnit.transform.AddComponent<Unit>();

        unit.teamType = _LoadInfo.unitTeamType;

        unit.modelType = _LoadInfo.unitModelType;

        unit.modelImg = Resources.Load<Sprite>(GetImgPath(_LoadInfo));


        unit.handleDirection = _LoadInfo.handleDirection;

        unit.SetUnitStatus(_LoadInfo.unitStatus);


        //모델 컴포넌트를 불러옵니다.
        modelComponent = model.GetComponent<UnitModel>();


        unit.modelPos = modelComponent.modelPos;


        //불러온 모델 컴포넌트로부터 각 핸들의 핸들포지션들을 받습니다.
        unit.leftHandlePos = modelComponent.HandlePos_Left;
        unit.rightHandlePos = modelComponent.HandlePos_Right;




        //애님을 등록합니다.
        UnitAnimManager animManager = this.model.AddComponent<UnitAnimManager>();
        animManager._Unit = unit;


        //퍼셜 매니저를 등록합니다.
        this.partial.AddComponent<UnitPartialManager>();


        //에임을 등록하고 각 에임의 값들을 초기화시킵니다.
        UnitAimManager aimManager = this.partial.AddComponent<UnitAimManager>();
        aimManager.aimPos = this.aimPos;

        aimManager.gunHandlePos = new Transform[2];

        aimManager.gunHandlePos[0] = this.aimPos.GetChild(0);
        aimManager.gunHandlePos[1] = this.aimPos.GetChild(1);


        //어택 프로세스를 등록합니다.
        UnitAttackProcess attackProcess = this.partial.AddComponent<UnitAttackProcess>();

        //피격 프로세스를 등록합니다.
        UnitHitProcess hitProcess = this.partial.AddComponent<UnitHitProcess>();


        //웨잇 프로세스를 등록합니다.
        UnitWaitProcess waitProcess = this.partial.AddComponent<UnitWaitProcess>();


        //이펙트 매니저를 등록합니다.
        UnitEffectManager effectManager = this.partial.AddComponent<UnitEffectManager>();


        ParticleSystem jetpackEffect = Instantiate(Resources.Load<ParticleSystem>("Unit/Jetpack/JetpackEffect"));
        jetpackEffect.transform.SetParent(modelComponent.backPack);
        jetpackEffect.transform.localPosition = new Vector3(-.05f, .11f, .25f);
        jetpackEffect.transform.localRotation = Quaternion.Euler(200, 0, 0);
        jetpackEffect.transform.localScale = new Vector3(.4f, .4f, .5f);

        effectManager.SetJetpackEffect(jetpackEffect);


        unit.shieldPos = shield;




        //유닛의 건을 등록합니다.
        unit.SetGun(Gun.CreateGunInstance(_LoadInfo.gunCode));

    }


    /// <summary>
    /// 레이어와 충돌체 판정을 설정합니다. 유닛의 피격판정의 대한 설정이 진행됩니다.
    /// </summary>
    private void SetCollision()
    {
        //유닛의 레이어값을 설정합니다.
        retUnit.layer = Layer(Unit.unitDefaultStatus.unitLayer);

        //유닛의 헤드판정에 콜라이더와 리지드바디를 등록하고 설정합니다.
        SphereCollider sphere = modelComponent.head.AddComponent<SphereCollider>();
        sphere.center = new Vector3(-.13f, 0.02f, 0);
        sphere.radius = .18f;

        Rigidbody rb = modelComponent.head.AddComponent<Rigidbody>();
        rb.useGravity = false;
        rb.constraints = RigidbodyConstraints.FreezeAll;

        //유닛의 헤드판정의 레이어를 설정합니다.
        modelComponent.head.gameObject.layer = Layer(Unit.unitDefaultStatus.unitHitLayer);

        //설정된 헤드의 HitInterface를 등록합니다.
        UnitHitInterface hitInterface = modelComponent.head.AddComponent<UnitHitInterface>();
        hitInterface.hitBoxType = UnitHitInterface.HitBoxType.HEAD;




        //유닛의 바디판정에 콜라이더와 리지드바디를 등록하고 설정합니다.
        BoxCollider box = modelComponent.body.AddComponent<BoxCollider>();
        box.center = new Vector3(.08f, 0f, 0f);
        box.size = new Vector3(.35f, .3f, .3f);

        rb = modelComponent.body.AddComponent<Rigidbody>();
        rb.useGravity = false;
        rb.constraints = RigidbodyConstraints.FreezeAll;

        //유닛의 바디판정의 레이어를 설정합니다.
        modelComponent.body.gameObject.layer = Layer(Unit.unitDefaultStatus.unitHitLayer);

        //설정된 바디의 HitInterface를 등록합니다.
        hitInterface = modelComponent.body.AddComponent<UnitHitInterface>();
        hitInterface.hitBoxType = UnitHitInterface.HitBoxType.BODY;
    }

    #endregion

}
