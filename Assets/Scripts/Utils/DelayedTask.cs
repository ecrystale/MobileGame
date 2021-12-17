using System;
using System.Collections;
using UnityEngine;

public static class DelayedTask
{
    public static IEnumerator Wrapper(Action callback, float delay, bool waitRealtime = false)
    {
        if (waitRealtime) yield return new WaitForSecondsRealtime(delay);
        else yield return new WaitForSeconds(delay);

        callback();
    }
}
