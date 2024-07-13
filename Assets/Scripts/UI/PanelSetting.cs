using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelSetting : MonoBehaviour
{
    private bool _isOn = false;

    public void Switch()
    {
        if (_isOn)
        {
            Off();
        }
        else
        {
            On();
        }
    }

    public void On()
    {
        gameObject.SetActive(true);
        _isOn = true;
    }

    public void Off()
    {
        gameObject.SetActive(false);
        _isOn = false;
    }

    private void Awake()
    {
        Off();
    }
}
