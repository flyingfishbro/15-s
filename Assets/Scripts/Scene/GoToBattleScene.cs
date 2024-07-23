using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GoToBattleScene : MonoBehaviour
{

    public Text gravityText;

    void Start()
    {
        StartCoroutine(FirstBattleStart());
    }

    private IEnumerator FirstBattleStart()
    {
        yield return null;


        float gravityY = -Physics.gravity.y;

        string str = ((int)(gravityY * 10)).ToString();

        gravityText.text = "";
        for (int i = 0; i < str.Length - 1; ++i)
        {
            gravityText.text += $"{str[i]}";
        }
        gravityText.text += $".{str[str.Length - 1]}G";

        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene("BattleMap_Default");
    }

}
