using UnityEngine;

public class BattleModeManager : ManagerBase<BattleModeManager>
{

    public static BattleMode GetBattleModeInstance(BattleMode.ModeType modeType)
    {
        GameObject retObject = new GameObject("CurrentBattleMode");
        retObject.transform.SetParent(BattleModeManager.instance.transform);

        switch (modeType)
        {
            case BattleMode.ModeType.ONE_TO_ONE:        retObject.AddComponent<BattleMode_OneToOne>(); break;

            case BattleMode.ModeType.THREE_TO_THREE:    retObject.AddComponent<BattleMode_ThreeToThree>(); break;
        }

        BattleMode ret = retObject.GetComponent<BattleMode>();
        ret.Initialized();

        return ret;
    }


}
