using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UI_Instance : MonoBehaviour
{
    [SerializeField] private CanvasGroup _LoadingImgCanvasGroup;

    
    [SerializeField] private CanvasGroup _LoadingTextImg;






    public IEnumerator OnLoadingImgBackground()
    {
        yield return null;

        float targetAlpha = 1.0f;
        while (Mathf.Abs(targetAlpha - _LoadingImgCanvasGroup.alpha) > .01f)
        {
            yield return null;

            _LoadingImgCanvasGroup.alpha = Mathf.Lerp(_LoadingImgCanvasGroup.alpha, targetAlpha, .1f);
        }
        _LoadingImgCanvasGroup.alpha = targetAlpha;
        

        yield return new WaitForSeconds(2f);


        targetAlpha = 0.0f;
        while (Mathf.Abs(targetAlpha - _LoadingTextImg.alpha) > .01f)
        {
            yield return null;

            _LoadingTextImg.alpha = Mathf.Lerp(_LoadingTextImg.alpha, targetAlpha, .2f);
        }
        _LoadingTextImg.alpha = targetAlpha;

        yield return new WaitForSeconds(1f);
    }

    


}
