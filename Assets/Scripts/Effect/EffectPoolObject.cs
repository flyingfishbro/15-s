using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class EffectPoolObject : MonoBehaviour
{
    public Transform user {  get; private set; }

    private List<EffectObject> effectPool;

    private const int DEFAULT_POOLSIZE = 10;


    public static EffectPoolObject GetPoolObjectInstance(
        string effectPath,
        string effectName,
        Transform user,
        int poolSize,
        EffectObject.PositionType positionType)
    {
        return GetPoolObjectInstance(Resources.Load<EffectObject>(effectPath), effectName, user, poolSize, positionType);
    }


    public static EffectPoolObject GetPoolObjectInstance(
        EffectObject effectPrefab,
        string effectName,
        Transform user,
        int poolSize,
        EffectObject.PositionType positionType)
    {

        if (poolSize < DEFAULT_POOLSIZE) poolSize = DEFAULT_POOLSIZE;


        EffectPoolObject instance = new GameObject($"{effectName}_EffectPoolObject").
            AddComponent<EffectPoolObject>();

        instance.user = user;
        instance.transform.parent = instance.user.transform;
        instance.transform.localPosition = Vector3.zero;
        instance.transform.localRotation = Quaternion.identity;


        instance.effectPool = new List<EffectObject>();
        
        for (int i = 0; i < poolSize; ++i)
        {
            EffectObject effect = Instantiate(effectPrefab);
            instance.effectPool.Add(effect);
            effect.EffectObjectInitialized(positionType, instance);


            effect.SetEffect(false);
        }



        return instance;
    }



    private int indexOfEffect;
    private void IncreaseIndexOfEffect()
    {
        ++indexOfEffect;
        indexOfEffect %= effectPool.Count;
    }




    public EffectObject OnEffect(Vector3 pos, Vector3 rot) => OnEffect(pos, rot, 0, 0f);


    public EffectObject OnEffect(Vector3 pos, Vector3 rot, int option, float optionValue)
    {
        EffectObject effectObject = effectPool[indexOfEffect];
        IncreaseIndexOfEffect();

        effectObject.Play(pos, option, optionValue);

        return effectObject;
    }




}
