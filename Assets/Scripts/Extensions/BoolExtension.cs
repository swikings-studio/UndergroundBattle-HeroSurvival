using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class BoolExtension
{
    public static void SwitchBoolWithDelay(this bool boolean, MonoBehaviour monoBehaviour ,  float delay)
    {
        monoBehaviour.StartCoroutine(SwitchingBoolWithDelay(boolean, delay));
    }
    private static IEnumerator SwitchingBoolWithDelay(this bool boolean, float delay)
    {
        boolean = !boolean;
        yield return new WaitForSeconds(delay);
        boolean = !boolean;
        yield break;
    }
}
