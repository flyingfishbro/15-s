using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EffectObject : MonoBehaviour
{
    public enum PositionType
    {
        LOCAL,
        WORLD,
        RECT
    }

    public PositionType positionType;

    public float lifeTime = 5;
    private Coroutine lifeTimeCo;


    public EffectPoolObject effectPoolObject { get; private set; }
    


    public virtual void EffectObjectInitialized(PositionType positionType, EffectPoolObject poolObject)
    {
        this.positionType = positionType;
        this.effectPoolObject = poolObject;

    }



    public void SetEffect(bool set) => SetEffect(set, Vector3.zero);
    public virtual void SetEffect(bool set, Vector3 pos)
    {
        if (set)
        {
            gameObject.SetActive(true);


            if (positionType == PositionType.WORLD)
            {
                transform.parent = null;
                transform.position = pos;
            }
            else if (positionType == PositionType.LOCAL)
            {
                transform.localPosition = pos;
            }
            else //if (positionType == PositionType.RECT)
            {
                transform.position = pos;
            }



            if (lifeTimeCo != null)
            {
                StopCoroutine(lifeTimeCo);  
                lifeTimeCo = null;
            }

            lifeTimeCo = StartCoroutine(LifeTimeCoroutine());

        }
        else
        {
            gameObject.SetActive(false);

            transform.SetParent(effectPoolObject.transform);
            transform.localPosition = Vector3.zero;
        }

    }



    public abstract void Play(Vector3 pos);

    public abstract void Play(Vector3 pos, int option, float optionValue);

    public abstract void Stop();


    private IEnumerator LifeTimeCoroutine()
    {
        yield return new WaitForSeconds(lifeTime);
        SetEffect(false);
    }

}
