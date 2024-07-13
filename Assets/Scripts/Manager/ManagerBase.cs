using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagerBase<T> : MonoBehaviour where T : MonoBehaviour
{
    public static T _Instance;
    public static T instance
    {
        get
        {
            if (_Instance == null)
            {
                _Instance = FindObjectOfType<T>();

                if (_Instance == null)
                {
                    _Instance = new GameObject(typeof(T).ToString()).AddComponent<T>(); 
                }
            }

            return _Instance;
        }
    }

    protected GameManager _GameManager;
    protected GameManager gameManager => _GameManager ?? (_GameManager = GameManager.instance);



    protected virtual bool IsDonDestroy => false;

    protected virtual void Awake()
    {
        if (IsDonDestroy)
            DontDestroyOnLoad(gameObject);

        gameManager.GameInitialized();
    }




}
