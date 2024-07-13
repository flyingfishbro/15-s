using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class RoundEndScene : MonoBehaviour
{

    private void LoadWinScene()
    {
        SceneManager.LoadScene("11.Win");
    }

    void Start()
    {
        Invoke("LoadWinScene", 3.0f);
    }
}
