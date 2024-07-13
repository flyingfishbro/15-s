using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class IntroScene : MonoBehaviour
{
    [SerializeField]
    private GameObject _logo;

    [SerializeField]
    private GameObject _main;

    private void ChangeLogo()
    {
        _main.SetActive(false);
        Invoke("ShowMain", 2.0f);
    }

    private void ShowMain()
    {
        _logo.SetActive(false);
        _main.SetActive(true);
        Invoke("LoadMainMenu", 3.0f);
    }

    private void LoadMainMenu()
    {
        SceneManager.LoadScene("1. MainMenu");
    }

    void Start()
    {
        ChangeLogo();
    }
}
