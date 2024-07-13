using UnityEngine;

public static class AudioExtensions
{
    public static float ToLogarithmicVolume(this float sliderValue)
    {
        return Mathf.Log10(Mathf.Max(sliderValue, 0.0001f)) * 20;
    }

    public static float ToLogarithmicFraction(this float fraction)
    {
        return Mathf.Log10(1 + 9 * fraction) / Mathf.Log10(10);
    }
}