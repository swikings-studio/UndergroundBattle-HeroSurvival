using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public static class GameTimeManager
{
    public static void SetTimeScaleSmoothly(float duration, float endValue)
    {
        DOTween.To(x => Time.timeScale = x, Time.timeScale, endValue, duration);
    }
}
