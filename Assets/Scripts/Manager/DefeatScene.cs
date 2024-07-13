using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefeatScene : MonoBehaviour
{
    public DefeatSceneUI instance;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(instance.OnDefeatUI());
    }
}
