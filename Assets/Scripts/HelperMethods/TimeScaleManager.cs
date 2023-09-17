using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TimeScaleManager
{
    static bool isProcessActive;
    static bool isForced = false;

    public static float TimeScale => Time.timeScale;

    public static void SetTimeScale(float scale)
    {
        if(isForced)return;
        Time.timeScale = scale;
        
    }

    public static void ForceTimeScale(float scale)
    {
        isForced = true;
        if(scale >= 1)isForced = false;
        Time.timeScale = scale;
    }
}
