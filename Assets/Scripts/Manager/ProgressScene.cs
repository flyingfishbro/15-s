using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressScene : MonoBehaviour
{

    public ProgressSceneUI instance;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(ProgressRoutine());
    }


    private IEnumerator ProgressRoutine()
    {
        yield return instance.OnProgressUI();

        yield return new WaitForSeconds(3f);

        SceneLoader.instance.LoadRecruitmentScene();
    }

}
