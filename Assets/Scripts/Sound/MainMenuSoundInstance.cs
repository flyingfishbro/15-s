using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuSoundInstance : SoundInstance 
{
    protected override void InitializeSounds()
    {
        base.InitializeSounds();
        AddSound("StartButton", "Sound/SFX_StartButton");
        AddSound("ButtonClick", "Sound/SFX_ButtonSound");
        AddSound("DownButtonClick", "Sound/SFX_DownButton");
        AddSound("UfoCome", "Sound/SFX_UfoCome");
        AddSound("UfoBeamdown", "Sound/SFX_UfoBeamDown");
        AddSound("UfoGone", "Sound/SFX_UfoGone");
        AddSound("JetStart", "Sound/SFX_JetStart");
        AddSound("Hit0", "Sound/SFX_Hit");
        AddSound("Hit1", "Sound/SFX_Hit 1");
        AddSound("Hit2", "Sound/SFX_Hit 2");
        AddSound("Hit3", "Sound/SFX_Hit 3");
        AddSound("Hit4", "Sound/SFX_Hit 4");
        AddSound("Hit5", "Sound/SFX_Hit 5");
        AddSound("WinVoice", "Sound/SFX_WinVoice");
        AddSound("Ready", "Sound/SFX_Ready");
    }
}
