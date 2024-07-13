using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UISoundSlider : MonoBehaviour
{
    [SerializeField]
    private ESoundType soundType;

    [SerializeField]
    private string _text;

    [SerializeField]
    private Sprite _spriteSound;
    [SerializeField]
    private Sprite _spriteMute;

    [SerializeField]
    private TMP_Text _textSound;
    [SerializeField]
    private Text _textType;
    [SerializeField]
    private Button _btnSound;
    [SerializeField]
    private Image _imgSound;

    [SerializeField]
    private Slider _slider;

    private const float MIN_VALUE = 0.001f;
    public float InitialValue = 1.0f;

    private float _prevValue;
    private bool _bMute = false;

    private SoundManager soundManager => SoundManager.Instance;

    private void Switch()
    {
        if (_bMute)
        {
            _imgSound.sprite = _spriteSound;
            _slider.value = _prevValue;
            _bMute = false;
        }
        else
        {
            _imgSound.sprite = _spriteMute;
            _slider.value = MIN_VALUE;
            _bMute = true;
        }
    }

    private void Awake()
    {
        _textSound.text = _text;
        _textType.text = _text;
        _btnSound.onClick.AddListener(Switch);
    }

    private void Start()
    {
        _slider.onValueChanged.AddListener((value) =>
        {
            soundManager.SetVolume(soundType, value);
        });

        _slider.value = InitialValue;
    }
}
