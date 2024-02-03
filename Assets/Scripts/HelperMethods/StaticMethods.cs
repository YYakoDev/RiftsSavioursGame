using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public static class HelperMethods
{
    /*private static Camera _camera;
    public static Camera MainCamera
    {
        get{
            if(_camera == null) _camera = Camera.main;
            return _camera;
        }
    }

    private static readonly Dictionary<float, WaitForSeconds> WaitDictionary = new();
    public static WaitForSeconds GetWait(float time)
    {
        if(WaitDictionary.TryGetValue(time, out var waitForSeconds)) return waitForSeconds;

        WaitDictionary[time] = new WaitForSeconds(time);
        return WaitDictionary[time];
    }*/

    private static PointerEventData _eventDataCurrentPosition;
    private static List<RaycastResult> _results;

    public static bool IsOverUI()
    {
        EventSystem currentEventSys = EventSystem.current;
        _eventDataCurrentPosition = new PointerEventData(currentEventSys) {position = YYInputManager.MousePosition};
        _results = new();
        currentEventSys.RaycastAll(_eventDataCurrentPosition, _results);
        return _results.Count > 0;
    }

}
