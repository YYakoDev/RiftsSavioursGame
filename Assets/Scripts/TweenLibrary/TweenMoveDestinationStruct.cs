using UnityEngine;
using UnityEngine.UI;

public struct TweenDestination
{
    Canvas _canvas;
    RectTransform _canvasRect;
    Vector3 _rawEndPosition, _endPositionPercentages, _endPos;
    public Vector3 RawEndPosition => _rawEndPosition;
    public Vector3 EndPositionPercentage => _endPositionPercentages;
    public TweenDestination(Vector3 endPosition, Canvas canvas = null)
    {
        _canvas = canvas;
        if(_canvas == null)
        {
            _canvas = GameObject.FindObjectOfType<Canvas>();
        }
        _canvasRect = _canvas.GetComponent<RectTransform>();
        var scaleFactor = _canvas.scaleFactor;

        _endPos = Vector3.zero;
        _rawEndPosition = endPosition;
        _endPositionPercentages = new Vector3();
        _endPositionPercentages.x = GetPercentage(1920f, (endPosition.x + 1920f / 2f));
        _endPositionPercentages.y = GetPercentage(1080f, (endPosition.y + 1080f / 2f));
        _endPositionPercentages.z = 0f;
    }

    float GetPercentage(float screenValue, float screenPosition)
    {
        Debug.Log(Screen.width + "  " + Screen.height);
        Debug.Log(screenValue);
        return (screenPosition * 100f) / screenValue;
    }

    public Vector3 GetEndPosition()
    {
        Debug.Log("X percentage:  " + EndPositionPercentage.x + "\n Y percentage:  " + EndPositionPercentage.y);
        //var inverseScale = 1f / _canvas.scaleFactor;
        return new Vector3(((_canvasRect.sizeDelta.x) * EndPositionPercentage.x) / 100f - (_canvasRect.sizeDelta.x / 2f), ((_canvasRect.sizeDelta.y) * EndPositionPercentage.y) / 100f - (_canvasRect.sizeDelta.y / 2f), 0f);
    }
}
