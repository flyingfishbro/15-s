using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamManagerBase<T> : MonoBehaviour where T : MonoBehaviour
{
    public static T _Instance;

    public static T instance => _Instance ?? (_Instance = CreateInstance());


    private static T CreateInstance()
    {
        if (_Instance == null)
        {
            _Instance = FindObjectOfType<T>();
            
            if (_Instance == null)
            {
                _Instance = Camera.main.gameObject.AddComponent<T>();

            }
        }

        return _Instance;
    }




    private Camera _Cam;
    public Camera cam => _Cam ?? (_Cam = GetComponent<Camera>());



    protected virtual void Awake()
    {
        //GameManager.instance.SetScreenResolution();
    }

}
