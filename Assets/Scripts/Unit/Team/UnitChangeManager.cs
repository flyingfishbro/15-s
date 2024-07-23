using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UnitChangeManager : MonoBehaviour
{
    public delegate IEnumerator UnitChangeCoroutine(Unit retire, Unit next);


    public UnitChangeCoroutine GetUnitChangeCoroutine()
    {
        return UFOCoroutine;
    }


    private UnitChanger_UFO _Ufo;
    private UnitChanger_UFO ufo => _Ufo ?? (_Ufo = Instantiate(Resources.Load<UnitChanger_UFO>("Unit/Team/UFO/UFO")));


    private IEnumerator UFOCoroutine(Unit retire, Unit next)
    {
        Vector3 moveDir(Vector3 targetPos, Vector3 startPos) => (targetPos - startPos).normalized;


        #region Retire
        ufo.gameObject.SetActive(true);
        Vector3 originUfoPos = new Vector3(0, 20, 0);

        #region UFO ComeDown

        // 넘겨받은 유닛을 기준으로, 회수할 ufo빔을 쏠 위치를 선정합니다.
        Vector3 unitTractorBeamPos(Unit unit) => (unit.modelPos.position + new Vector3(0, 4, 0));
        float moveSpeed = 12;

        ufo.transform.position = originUfoPos;



        //UFO 가 내려옵니다.
        //죽는 모션 애니메이션에서 모델의 이동이 있으므로
        //Ufo위치 세팅 한정으로만 Unit기준의 포지션이 아닌 Unit의 모델을 기준으로 포지션을 잡고 이동합니다.
        SoundManager.Instance.Play("UfoCome");
        while (true)
        {
            yield return null;
            
            //ufo의 위치와 라타이어된 유닛의 거리차가 트랙터빔 쏘는 위치와 리타이어 유닛과의 거리차보다 가까워지면 포지션을 세팅하고 반복문을 나갑니다.
            if (Vector3.Distance(ufo.transform.position, retire.modelPos.position) < Vector3.Distance(unitTractorBeamPos(retire), retire.modelPos.position))
            {
                ufo.transform.position = unitTractorBeamPos(retire);
                break;
            }

            ufo.transform.position += moveDir(unitTractorBeamPos(retire), ufo.transform.position) * moveSpeed * Time.deltaTime;
        }
        #endregion


        //UFO가 빔을 쏘는 애니메이션을 시작하며 적정시간이 지나도록 기다립니다.
        ufo.SetTractorBeam(true);
        SoundManager.Instance.Play("UfoBeamdown");
        yield return new WaitForSeconds(1f);


        #region Retrieve Retrie Unit
        retire.rb.useGravity = false;

        while (retire.transform.position.y < ufo.transform.position.y) 
        {
            yield return null;
            retire.transform.position += Vector3.up * moveSpeed * Time.deltaTime;
        }
        retire.gameObject.SetActive(false);

        #endregion


        ufo.SetTractorBeam(false);

        yield return new WaitForSeconds(2f);


        #region UFO TurnBack
        SoundManager.Instance.Play("UfoGone");
        while (true)
        {
            yield return null;

            //ufo의 위치와 리타이어 유닛과의 거리차가 ufo시작점의 위치와 리타이어 유닛의 거리차보다 멀면 포지션을 세팅하고 반복문을 나갑니다.
            if (Vector3.Distance(ufo.transform.position, retire.transform.position) > Vector3.Distance(originUfoPos, retire.transform.position))
            {
                ufo.transform.position = originUfoPos;
                break;
            }

            ufo.transform.position += moveDir(originUfoPos, ufo.transform.position) * moveSpeed * Time.deltaTime;
        }
        #endregion


        ufo.gameObject.SetActive(false);


        #endregion


        BattleManager.instance.GetTeam(retire).RetireUnit(retire);
        BattleManager.instance.GetTeam(next).SetFieldUnit(next);



        #region SetFieldNextUnit

        Vector3 setNextPos = BattleManager.GetUnitBasePosition(retire);
        

        //교체되어 새로 생성될 유닛을 기준으로 플레이어팀인지 봇팀인지 구분하여 캠에 포지션값을 전달합니다.
        Vector3 otherNextPos = BattleManager.instance.GetOhterFieldUnit(next.teamType).transform.position;

        Vector3 playerTeamPos = Vector3.zero;
        Vector3 botTeampPos = Vector3.zero;
        if (retire.teamType == BattleManager.instance.playerTeamType)
        {
            playerTeamPos = setNextPos;
            botTeampPos = otherNextPos;
        }
        else
        {
            playerTeamPos = otherNextPos;
            botTeampPos = setNextPos;
        }

        BattleManager.instance.GetBattleCam().SetTargetingCamMode(
            new Vector3[]
            {
                playerTeamPos,
                botTeampPos
            });



        Vector3 startNextPos = setNextPos;
        startNextPos.y += 10;


        next.transform.position = startNextPos;
        next.rb.useGravity = false;


        next.partial.effectManager.OnJetpackEffect();

        while (true)
        {
            yield return null;

            if (Vector3.Distance(startNextPos, next.transform.position) > Vector3.Distance(startNextPos, setNextPos))
            {
                next.transform.position = setNextPos;
                break;
            }


            next.transform.position += moveDir(setNextPos, next.transform.position) * moveSpeed * .7f * GameManager.gravityCorrectionValue * Time.deltaTime;
        }

        next.rb.useGravity = true;
        next.partial.effectManager.OffJetpackEffect();


        yield return new WaitForSeconds(1);

        #endregion

    }



}
