using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PauseInput : BattleInputSystem
{
    [SerializeField]
    private Sprite _spritePause;
    [SerializeField]
    private Sprite _spritePlay;

    private Image _OnPauseInputImg;
    private Image inputImg => _OnPauseInputImg ?? (GetComponent<Image>());

    private float normalScale = 1.0f;

    private bool IsTimeStopped => Time.timeScale < float.Epsilon;

    private bool _isClicking = false;

    private void SwitchTimeScale()
    {
        if (IsTimeStopped)
        {
            IsGamePaused = false;
            Time.timeScale = normalScale;
            inputImg.sprite = _spritePause;
        }
        else
        {
            IsGamePaused = true;
            Time.timeScale = 0.0f;
            inputImg.sprite = _spritePlay;
        }
    }

    public override void SetInput(bool set, Vector2 pos)
    {
        if (set)
        {
            if (!_isClicking)
            {
                SwitchTimeScale();
            }

            _isClicking = true;
            SoundManager.Instance.Play("ButtonClick");
        }
        else
        {
            _isClicking = false;
        }
    }
}
