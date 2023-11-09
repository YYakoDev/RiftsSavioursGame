using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public static class HelperMethods
{
    private static Camera _camera;
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
    }

    private static PointerEventData _eventDataCurrentPosition;
    private static List<RaycastResult> _results;

    public static bool IsOverUI()
    {
        EventSystem currentEventSys = EventSystem.current;
        _eventDataCurrentPosition = new PointerEventData(currentEventSys) {position = Input.mousePosition};
        _results = new();
        currentEventSys.RaycastAll(_eventDataCurrentPosition, _results);
        return _results.Count > 0;
    }

    public static Vector2 TranslateUiToWorldPoint(Vector2 canvasReferenceResolution, Vector2 position)
    {
        float screenXCenter = Display.main.renderingWidth / 2f;
        float screenYCenter = Display.main.renderingHeight / 2f;
        Debug.Log($"{screenXCenter*2}x{screenYCenter*2}");

        float canvasWidthCenter = canvasReferenceResolution.x / 2f;
        float canvasHeightCenter = canvasReferenceResolution.y / 2f;


        Vector2 worldPoint = new Vector2
        (
            (position.x / canvasWidthCenter) * screenXCenter + screenXCenter,
            (position.y / canvasHeightCenter) * screenYCenter + screenYCenter
        );

        return worldPoint;
    }

}
