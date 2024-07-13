using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BattleCam : MonoBehaviour
{
    public static BattleCam CreateBattleCamInstance()
    {
        return Camera.main.AddComponent<BattleCam>();
    }





    public enum BattleCamMode
    {
        WAIT,
        BATTLE,
        TARGETING,
        
    }

    public BattleCamMode camMode { get; private set; } = BattleCamMode.WAIT;

    private Transform[] targetObject;


    /// <summary>
    /// 타겟이 아닌 백터값이 들어왔을때 타겟 대신 사용되는 임시 트랜스폼입니다.
    /// </summary>
    private Transform[] _TemporaryTargetObject;
    private Transform[] tempTargetObject
        => _TemporaryTargetObject ?? (_TemporaryTargetObject =
        new Transform[2]
        {
            new GameObject("BattleCam_TemporaryTransform").transform,
            new GameObject("BattleCam_TemporaryTransform").transform
        });


    private float singleTargetDistance;
    private float singleTargetingRotSpeed;


    public void SetSingleTargetSetting(float targetDistance, float targetRotSpeed)
    {
        singleTargetDistance = Mathf.Max(targetDistance, 1);
        singleTargetingRotSpeed = targetRotSpeed;
    }

    public void SetCamMode(BattleCamMode setMode)
    {
        camMode = setMode;
        targetObject = null;

        SetSingleTargetSetting(0, 0);
    }

    #region SetTargetingCamMode
    public void SetTargetingCamMode(Transform singleTarget, float targetDistance, float targetRotSpeed)
    {
        SetTargetingCamMode(new Transform[] { singleTarget });

        SetSingleTargetSetting(targetDistance, targetRotSpeed);
    }
    public void SetTargetingCamMode(Transform[] targets)
    {
        camMode = BattleCamMode.TARGETING;

        targetObject = targets;

        SetSingleTargetSetting(0, 0);

    }

    public void SetTargetingCamMode(Vector3 singleTargetPos, float targetDistance, float targetRotSpeed)
    {
        SetTargetingCamMode(new Vector3[] { singleTargetPos });

        SetSingleTargetSetting(targetDistance, targetRotSpeed);
    }
    public void SetTargetingCamMode(Vector3[] targetPos)
    {
        //targetObject = null;

        for (int i = 0; i < tempTargetObject.Length; ++i)
        {
            tempTargetObject[i].position = targetPos[i];
        }

        targetObject = tempTargetObject;


        SetSingleTargetSetting(0, 0);

    }

    #endregion

    private Vector3 _CamPos;

    private const float CAM_DEFAULT_SPEED = .1f;
    private float camSpeed => CAM_DEFAULT_SPEED * GameManager.gravityCorrectionValue;


    private bool isCamShake;

    private Vector3 shakeDir;
    private float camShakeVelocity;

    private float camShakeTime;


    private void Update()
    {
        CamMode();

        
        if (isCamShake)
        {
            camShakeTime -= Time.deltaTime;
            if (camShakeTime < 0)
            {
                isCamShake = false;
                camShakeTime = 0;
            }

            shakeDir = new Vector3(Random.Range(-camShakeVelocity, camShakeVelocity), Random.Range(-camShakeVelocity, camShakeVelocity), 0);
            shakeDir = transform.TransformDirection(shakeDir);

            transform.position = _CamPos + shakeDir;
        }
        else
        {
            transform.position = _CamPos;
        }

    }


    private void CamMode()
    {
        switch (camMode)
        {
            case BattleCamMode.TARGETING: CamMode_Targeting(); break;

            case BattleCamMode.BATTLE: CamMode_Battle(); break;

            default: CamMode_Wait(); break;
        }
    }


    private bool CamModeIsValid()
    {
        if (camMode == BattleCamMode.WAIT) return true;

        if (camMode == BattleCamMode.TARGETING) return targetObject != null && targetObject.Length > 0;

        if (camMode == BattleCamMode.BATTLE) return BattleManager.instance.fieldIsValid;

        return false;
    }


    private void CamMode_Wait() { }

    private void CamMode_Targeting()
    {
        if (!CamModeIsValid())
        {
            SetCamMode(BattleCamMode.WAIT);
            return;
        }

        CamTargeting(targetObject);
    }

    
    /// <summary>
    /// 배틀 도중에 적용되는 카메라의 위치와 각도를 설정합니다.
    /// </summary>
    private void CamMode_Battle()
    {
        if (!CamModeIsValid())
        {
            SetCamMode(BattleCamMode.WAIT);
            return;
        }

        CamTargeting(
            new Transform[]
            {
                BattleManager.instance.GetFieldUnit(BattleManager.instance.playerTeamType).transform,
                BattleManager.instance.GetOhterFieldUnit(BattleManager.instance.playerTeamType).transform
            });
    }



    private void CamTargeting(Transform[] targets)
    {
        if (targets == null || targets.Length < 1) return;

        Vector3 targetCamPos = transform.position;

        if (targets.Length < 2)
        {
            transform.LookAt(targets[0]);


            float dis = Vector3.Distance(targets[0].position, transform.position) - singleTargetDistance;

            Vector3 dir = (targets[0].position - transform.position).normalized;


            targetCamPos = _CamPos + 
                (dir * dis) + 
                transform.TransformDirection(Vector3.right) * singleTargetingRotSpeed * Time.deltaTime * 1 / Time.timeScale;


        }
        else
        {
            // 타겟이 되는 두 오브젝트의 위치를 받아옵니다.
            Vector3 target1 = targets[0].position;
            Vector3 target2 = targets[1].position;

            //미리 두 값의 높이 차이 값을 계산합니다.
            float heightDiff = Mathf.Abs(target1.y - target2.y);

            // 두 값이 거리를 계산합니다.
            float distance = Vector3.Distance(target1, target2);

            distance = Mathf.Max(distance, 6);

            
            // 두 값중 높이가 더 낮은 값을 선택하고 두 값의 높이의 적용합니다.
            float min = target1.y > target2.y ? target2.y : target1.y;

            target1.y = min;
            target2.y = min;


            // 카메라가 바라보는 포지션의 추가되는 값을 계산합니다. 두 포지션간의 높이차가 많이 날수록 위로 올려다봅니다.
            Vector3 lookPosAddValue = new Vector3(0, heightDiff * .7f, 0);


            // 두 사이값의 중앙을 계산합니다.
            Vector3 mid = target1 + (target2 - target1).normalized * distance * .5f;


            // 중앙과 추가되는 값을 더하여 해당 포지션을 바라봅니다.
            transform.LookAt(mid + lookPosAddValue);



            // 카메라가 있어야 하는 위치를 나타냅니다. 높이차가 날수록 더 멀리서 바라보며, 약간 밑에서 바라보도록 하였습니다.
            targetCamPos =
                mid + Vector3.Cross((target1 - target2).normalized, Vector3.up) * distance
                * (1 + heightDiff * .1f);

            
        }

        // 타겟이 되는 위치에 자연스럽게 다가갑니다.
        _CamPos = Vector3.Lerp(transform.position, targetCamPos, camSpeed);

    }



    public void CamShakeEffect(float velocity, float time)
    {
        isCamShake = true;  
        camShakeVelocity = velocity;
        camShakeTime = time;
    }





}
