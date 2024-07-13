using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using static GameManager;

public class GameManager : ManagerBase<GameManager>
{
    protected override bool IsDonDestroy => true;

    public static readonly float WIDTH = 1920;
    public static readonly float HEIGHT = 1080;


    public static readonly Vector3 GRAVITY_LV1 = new Vector3(0f, -9.81f, 0f);

    public static readonly Vector3 GRAVITY_LV2 = new Vector3(0f, -12.2f, 0f);

    public static readonly Vector3 GRAVITY_LV3 = new Vector3(0f, -6.5f, 0f);




    public static float gravityCorrectionValue => (Physics.gravity.y / GRAVITY_LV1.y);



    public Vector3 GetCurrentGravity() => Physics.gravity;

    public enum GravityType
    {
        LV1 = 0,
        LV2 = 1,
        LV3 = 2,




        _COUNT
    }

    public GravityType currentGravityType {  get; private set; }

    public void SetGravity(GravityType gravityType)
    {
        currentGravityType = gravityType;
        Physics.gravity = ConvertGravityType(gravityType);

    }


    public Vector3 ConvertGravityType(GravityType gravityType)
    {
        Vector3 ret = Vector3.zero;

        switch (gravityType)
        {
            case GravityType.LV2:
                ret = GRAVITY_LV2;
                break;

            case GravityType.LV3:
                ret = GRAVITY_LV3;
                break;

            default:
                ret = GRAVITY_LV1;
                break;
        }

        return ret;
    }






    private bool setInitialized = false;

    public void GameInitialized()
    {
        if (setInitialized) return;
        setInitialized = true;

        
#if UNIT_IOS || UNITY_ANDROID
        Application.targetFrameRate = 120;
#else
         
        QualitySettings.vSyncCount = 1;
#endif

        float randomSpeed = Random.Range(0f, 100f);
        Random.InitState((int)randomSpeed);


    }

}
