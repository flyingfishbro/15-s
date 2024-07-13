using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressSceneUI : UI_Instance
{
    [SerializeField] CanvasGroup _ProgressUI;

    [SerializeField] Text gravityText;
    
    
    public IEnumerator OnProgressUI()
    {
        float gravityY = -Physics.gravity.y;


        string str = ((int)(gravityY * 10)).ToString();

        gravityText.text = "";
        for (int i = 0; i < str.Length - 1; ++i)
        {
            gravityText.text += $"{str[i]}";
        }
        gravityText.text += $".{str[str.Length - 1]}G";


        float targetAlpha = 1;

        while (Mathf.Abs(targetAlpha - _ProgressUI.alpha) > 0.01f)
        {
            yield return null;
            _ProgressUI.alpha = Mathf.Lerp(_ProgressUI.alpha, targetAlpha, 0.1f);
        }
        _ProgressUI.alpha = targetAlpha;


        yield return new WaitForSeconds(3);


        targetAlpha = 0;
        while (Mathf.Abs(targetAlpha - _ProgressUI.alpha) > 0.01f)
        {
            yield return null;
            _ProgressUI.alpha = Mathf.Lerp(_ProgressUI.alpha, targetAlpha, 0.1f);
        }
        _ProgressUI.alpha = targetAlpha;

    }


}
