using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class UnitAimManager : UnitPartial
{

    public float aimWeightSpeed = 0.01f;

    public float currentAimRot { get; private set; }


    public virtual float CalGunRecoil() => UnityEngine.Random.Range(unit.currentGun.status.minRecoil, unit.currentGun.status.maxRecoil);


    public Transform aimPos;


    public Transform[] gunHandlePos;

    public Transform[] playerHandlePos;


    public Vector3[] originHandlePos;



    private void Awake()
    {

        playerHandlePos = unit.GetHandlePos();


        originHandlePos = new Vector3[playerHandlePos.Length];
        for (int i = 0; i < playerHandlePos.Length; ++i)
        {
            originHandlePos[i] = playerHandlePos[i].localPosition;
        }

    }



    private void Update()
    {
        SetHandlePos();


        if (!unit.state.isAim.state) return;


        // 현재 장착된 건의 각도를 에임과 정확하게 일치시킵니다.
        if (unit.currentGun != null)
        {
            SetGunRot();
        }



        //aimPos.transform.position = unit.shoulderModel.position;
        


        Vector3 rot = aimPos.localEulerAngles;

        rot.z = -currentAimRot * (unit.handleDirection == Unit.HandleDirection.LEFT ? 1 : -1);


        aimPos.localRotation = Quaternion.Euler(rot);


        unit.partial.animManager.SetAimRot(currentAimRot / (unit.status.maxAimRot - unit.status.minAimRot) * 1.2f);

    }


    public void SetAimRot(float rot)
    {
        currentAimRot = Mathf.Clamp(rot, unit.status.minAimRot, unit.status.maxAimRot);
    }



    /// <summary>
    /// 현재 장착된 건을 손의 자식오브젝트로 둔후 정상적인 위치를 조정합니다.
    /// </summary>
    /// <param name="gun"> 장착된 건</param>
    public void SetGun(Gun gun)
    {
        gun.transform.parent = playerHandlePos[0];
        gun.transform.localPosition = Vector3.zero;

    }

    /// <summary>
    /// 손의 위치를 조정합니다. Lerp를 사용하여 적절한 속도로 조정합니다.
    /// </summary>
    private void SetHandlePos()
    {

        if (!unit.state.isAim.state)
        {
            for (int i = 0; i < playerHandlePos.Length; ++i) playerHandlePos[i].localPosition = Vector3.Lerp(playerHandlePos[i].localPosition, originHandlePos[i], aimWeightSpeed);
        }
        else
        {
            for (int i = 0; i < playerHandlePos.Length; ++i) playerHandlePos[i].position = Vector3.Lerp(playerHandlePos[i].position, gunHandlePos[i].position, aimWeightSpeed);
        }
    }



    /// <summary>
    /// 건의 각도를 조정합니다. x축은 AimPos기준을, y축은 적군을 정확히 바라보도록 세팅하되, Lerp를 이용하여 IsAim상태를 기준으로 자연스럽게 조정합니다.
    /// </summary>
    private void SetGunRot()
    {
        Gun gun = unit.currentGun;

        if (gun == null) return;
        if (unit.enemyUnit == null) return;

        //적군을 바라보는 방향의 y값 계산.
        Vector3 dir = (unit.enemyUnit.transform.position - gun.transform.position).normalized;
        float yAngle = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg;


        //타겟이 되는 각도
        Quaternion targetRot = Quaternion.Euler(
            aimPos.eulerAngles.z * (unit.handleDirection == Unit.HandleDirection.LEFT ? 1 : -1),
            yAngle,
            0);

        //타겟 각도로 자연스럽게 설정.
        gun.transform.rotation = Quaternion.Lerp(gun.transform.rotation, targetRot, .5f);

    }



}
