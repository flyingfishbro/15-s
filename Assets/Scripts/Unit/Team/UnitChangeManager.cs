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

        // �Ѱܹ��� ������ ��������, ȸ���� ufo���� �� ��ġ�� �����մϴ�.
        Vector3 unitTractorBeamPos(Unit unit) => (unit.modelPos.position + new Vector3(0, 4, 0));
        float moveSpeed = 12;

        ufo.transform.position = originUfoPos;



        //UFO �� �����ɴϴ�.
        //�״� ��� �ִϸ��̼ǿ��� ���� �̵��� �����Ƿ�
        //Ufo��ġ ���� �������θ� Unit������ �������� �ƴ� Unit�� ���� �������� �������� ��� �̵��մϴ�.
        SoundManager.Instance.Play("UfoCome");
        while (true)
        {
            yield return null;
            
            //ufo�� ��ġ�� ��Ÿ�̾�� ������ �Ÿ����� Ʈ���ͺ� ��� ��ġ�� ��Ÿ�̾� ���ְ��� �Ÿ������� ��������� �������� �����ϰ� �ݺ����� �����ϴ�.
            if (Vector3.Distance(ufo.transform.position, retire.modelPos.position) < Vector3.Distance(unitTractorBeamPos(retire), retire.modelPos.position))
            {
                ufo.transform.position = unitTractorBeamPos(retire);
                break;
            }

            ufo.transform.position += moveDir(unitTractorBeamPos(retire), ufo.transform.position) * moveSpeed * Time.deltaTime;
        }
        #endregion


        //UFO�� ���� ��� �ִϸ��̼��� �����ϸ� �����ð��� �������� ��ٸ��ϴ�.
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

            //ufo�� ��ġ�� ��Ÿ�̾� ���ְ��� �Ÿ����� ufo�������� ��ġ�� ��Ÿ�̾� ������ �Ÿ������� �ָ� �������� �����ϰ� �ݺ����� �����ϴ�.
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
        

        //��ü�Ǿ� ���� ������ ������ �������� �÷��̾������� �������� �����Ͽ� ķ�� �����ǰ��� �����մϴ�.
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
