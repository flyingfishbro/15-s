using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_tournament : MonoBehaviour
{
    [SerializeField]
    private Button _btnSetting;
    [SerializeField]
    private Button _btnInfo;
    [SerializeField]
    private Button _btnDown1;
    [SerializeField]
    private Button _btnDown2;
    [SerializeField]
    private Button _btnDown3;


    [SerializeField]
    private PanelSetting _canvasInfo;
    [SerializeField]
    private PanelSetting _panelSetting;

    private void Awake()
    {
        _btnSetting.onClick.AddListener(() =>
        {
            _panelSetting.Switch();
            SoundManager.Instance.Play("ButtonClick");

        });
        _btnInfo.onClick.AddListener(() =>
        {
            _canvasInfo.Switch();
            SoundManager.Instance.Play("ButtonClick");
        });
        _btnDown1.onClick.AddListener(() =>
        {
            SoundManager.Instance.Play("DownButtonClick");
        });
        _btnDown2.onClick.AddListener(() =>
        {
            SoundManager.Instance.Play("DownButtonClick");
        });
        _btnDown3.onClick.AddListener(() =>
        {
            SoundManager.Instance.Play("DownButtonClick");
        });
    }
}
