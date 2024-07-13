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
    private PanelSetting _canvasInfo;
    [SerializeField]
    private PanelSetting _panelSetting;

    private void Awake()
    {
        _btnSetting.onClick.AddListener(() =>
        {
            _panelSetting.Switch();
        });
        _btnInfo.onClick.AddListener(() =>
        {
            _canvasInfo.Switch();
        });
    }
}
