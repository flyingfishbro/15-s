using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitAttackProcess : UnitPartial
{

    private void Start()
    {
        unit.state.isShot.AddSetStateStaticListener(true, OnShotState);
        unit.state.isShot.AddSetStateStaticListener(false, OffShotState);

        unit.state.isShotMotion.AddSetStateStaticListener(true, OnShotMotionState);
        unit.state.isShotMotion.AddSetStateStaticListener(false, OffShotMotionState);
            
    }


    private void SetShotState(bool set)
    {
        if (set)
        {
            if (unit.isPlayerUnit || true)
                BattleManager.instance.GetBattleCam().CamShakeEffect(unit.currentGun.status.maxRecoil * .02f, .1f);


        }
        else
        {


        }
    }

    private void OnShotState() => SetShotState(true);
    private void OffShotState() => SetShotState(false); 




    private void SetShotMotionState(bool set)
    {
        if (set)
        {
            unit.state.isShot.SetState(true);

            unit.state.isAim.SetState(false);
        }
        else
        {
            unit.state.isShot.SetState(false);

            if (attackMotionCo != null)
            {
                StopCoroutine(attackMotionCo);
                attackMotionCo = null;
            }

            unit.state.isAim.SetState(true);
        }
    }

    private void OnShotMotionState() => SetShotMotionState(true);
    private void OffShotMotionState() => SetShotMotionState(false);






    private bool CanShot()
    {
        return unit.state.isAim.state && unit.currentGun != null;
    }

    public void SetAttack(bool set)
    {
        if (!CanShot()) return;

        bool shot = unit.currentGun.OnShot(set, out float delay, out bool shotMotion);

        if (shot)
        {
            unit.state.isShot.SetState(true);
            unit.state.isShot.SetState(false);

            unit.state.isShotMotion.SetState(shotMotion);

            

            float addRecoil = unit.partial.aimManager.CalGunRecoil();

            unit.partial.aimManager.SetAimRot(unit.partial.aimManager.currentAimRot + addRecoil);



            if (unit.state.isShotMotion.state)
            {
                if (attackMotionCo != null)
                {
                    StopCoroutine(attackMotionCo);
                    attackMotionCo = null;
                }
                attackMotionCo = StartCoroutine(AttackMotionCoroutine(delay));
            }
            

        }
    }




    private Coroutine attackMotionCo;

    private IEnumerator AttackMotionCoroutine(float sec)
    {
        sec = Mathf.Clamp(sec, 0.01f, .2f);

        yield return new WaitForSeconds(sec);

        unit.state.isShotMotion.SetState(false);
    }

}
