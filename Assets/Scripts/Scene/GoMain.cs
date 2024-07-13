using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GoMain : MonoBehaviour
{
    [SerializeField] private float waitSec;
    void Start()
    {
        Invoke("GoToMain", waitSec);
    }

    void GoToMain()
    {
        SceneManager.LoadScene("1. MainMenu");
    }

 


}
