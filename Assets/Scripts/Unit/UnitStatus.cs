using UnityEngine;

public class UnitStatus
{
    public float maxHp {  get; private set; }
    public float hp { get; private set; }

    public void SetHp(float set) => hp = Mathf.Clamp(set, 0, maxHp);

    public bool IsAlive => hp > 0;


    public float toCloseJumpPower { get; private set; }
    public float toFarJumpPower { get; private set; }

    public float maxAimRot { get; private set; }
    public float minAimRot { get; private set; }
  

    public bool hasShield { get; private set; } 
    public void usedShield() => hasShield = false;
    public void chargeShield() => hasShield = true;


    public UnitStatus(UnitDefaultStatus defaultStatus)
    {
        maxHp = defaultStatus.maxHp;
        hp = maxHp;

        toCloseJumpPower = defaultStatus.toCloseJumpPower;
        toFarJumpPower = defaultStatus.toFarJumpPower;  

        minAimRot = defaultStatus.minAimRot;
        maxAimRot = defaultStatus.maxAimRot;

        hasShield = true;
    }


}
