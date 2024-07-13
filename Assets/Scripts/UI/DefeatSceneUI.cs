using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefeatSceneUI : MonoBehaviour
{
    [SerializeField] CanvasGroup _ProgressUI;


    public IEnumerator OnDefeatUI()
    {
        float targetAlpha = 1;

        while (Mathf.Abs(targetAlpha - _ProgressUI.alpha) > 0.01f)
        {
            yield return null;
            _ProgressUI.alpha = Mathf.Lerp(_ProgressUI.alpha, targetAlpha, 0.1f);
        }
        _ProgressUI.alpha = targetAlpha;

    }
}
