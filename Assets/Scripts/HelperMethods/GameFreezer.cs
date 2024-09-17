using System.Collections;
using System.Threading.Tasks;
using UnityEngine;

public static class GameFreezer
{
    /// <summary>
    /// Freezes the time scale for the amount of seconds specified. I advise you to set a value lower than 0.06 seconds
    /// </summary>
    /// <param name="seconds"></param>
    public static async void FreezeGame(float seconds)
    {
        int miliseconds = (int)(seconds * 1000f);
        TimeScaleManager.SetTimeScale(0f);
        #if !UNITY_WEBGL
        await Task.Delay(miliseconds);
        #endif
        TimeScaleManager.SetTimeScale(1f);
    }


    /*public static void Freeze()
    {
        // await the freeze game async task and check if it is ongoing or not completed then cancel it and execute again?
    }*/
}
