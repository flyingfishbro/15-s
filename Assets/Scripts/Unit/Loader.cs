using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Loader<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T _Instance;

    public static T instance => _Instance ?? (_Instance = CreateInstance());

    private static T CreateInstance()
    {
        if (_Instance == null)
        {
            _Instance = FindObjectOfType<T>();
            if (_Instance == null)
            {
                _Instance = new GameObject($"{typeof(T).ToString()}Loader").AddComponent<T>();
            }
        }

        return _Instance;
    }
}
