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


        // ���� ������ ���� ������ ���Ӱ� ��Ȯ�ϰ� ��ġ��ŵ�ϴ�.
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
    /// ���� ������ ���� ���� �ڽĿ�����Ʈ�� ���� �������� ��ġ�� �����մϴ�.
    /// </summary>
    /// <param name="gun"> ������ ��</param>
    public void SetGun(Gun gun)
    {
        gun.transform.parent = playerHandlePos[0];
        gun.transform.localPosition = Vector3.zero;

    }

    /// <summary>
    /// ���� ��ġ�� �����մϴ�. Lerp�� ����Ͽ� ������ �ӵ��� �����մϴ�.
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
    /// ���� ������ �����մϴ�. x���� AimPos������, y���� ������ ��Ȯ�� �ٶ󺸵��� �����ϵ�, Lerp�� �̿��Ͽ� IsAim���¸� �������� �ڿ������� �����մϴ�.
    /// </summary>
    private void SetGunRot()
    {
        Gun gun = unit.currentGun;

        if (gun == null) return;
        if (unit.enemyUnit == null) return;

        //������ �ٶ󺸴� ������ y�� ���.
        Vector3 dir = (unit.enemyUnit.transform.position - gun.transform.position).normalized;
        float yAngle = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg;


        //Ÿ���� �Ǵ� ����
        Quaternion targetRot = Quaternion.Euler(
            aimPos.eulerAngles.z * (unit.handleDirection == Unit.HandleDirection.LEFT ? 1 : -1),
            yAngle,
            0);

        //Ÿ�� ������ �ڿ������� ����.
        gun.transform.rotation = Quaternion.Lerp(gun.transform.rotation, targetRot, .5f);

    }



}
