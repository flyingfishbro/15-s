using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitChanger_UFO : MonoBehaviour
{
    public Animator _TractorBeamAnimator;
    public Animator anim => _TractorBeamAnimator;

    public void SetTractorBeam(bool set) => anim.SetTrigger(set ? "_LightOn" : "_LightOff");





}
