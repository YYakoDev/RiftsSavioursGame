using System;
using UnityEngine;
using UnityEngine.UI;

public struct TweenDestination
{
    private Canvas _sceneCanvas;
    RectTransform _canvasRect;
    Vector3 _rawEndPosition, _endPositionPercentages, _endPos;
    public Vector3 RawEndPosition => _rawEndPosition;
    public Vector3 EndPositionPercentage => _endPositionPercentages;
    public Canvas SceneCanvas => _sceneCanvas;
    
    public TweenDestination(Vector3 endPosition, Canvas sceneCanvas = null)
    {
        var canvas = sceneCanvas;
        if(canvas == null)
        {
            try
            {
                canvas = GameObject.FindGameObjectWithTag("MainCanvas").GetComponent<Canvas>();
            }
            catch (Exception e)
            {
                canvas = GameObject.FindObjectOfType<Canvas>();
                Console.WriteLine(e);
            }
            if(canvas == null)
            {
                Debug.LogWarning("Could not get not a single canvas object for the tween library");
            }
        }

        _sceneCanvas = canvas;
        _canvasRect = _sceneCanvas?.GetComponent<RectTransform>();

        _endPos = Vector3.zero;
        _rawEndPosition = endPosition;
        _endPositionPercentages = new();
        _endPositionPercentages.x = GetPercentage(1920f, (endPosition.x));
        _endPositionPercentages.y = GetPercentage(1080f, (endPosition.y));
        _endPositionPercentages.z = 0f;
    }

    float GetPercentage(float screenValue, float screenPosition)
    {
        return ((screenPosition + screenValue / 2f) * 100f) / screenValue;
    }

    public Vector3 GetEndPosition()
    {
        return new Vector3((_canvasRect.sizeDelta.x * EndPositionPercentage.x) / 100f - (_canvasRect.sizeDelta.x / 2f), ((_canvasRect.sizeDelta.y) * EndPositionPercentage.y) / 100f - (_canvasRect.sizeDelta.y / 2f), 0f);
    }

    public Vector2 GetCanvasSize()
    {
        if (_sceneCanvas == null) return new Vector2(Screen.width, Screen.height); //the screen width and height are not the value that you desire but at least it returns something
        if (_canvasRect == null)
        {
            _canvasRect = _sceneCanvas.GetComponent<RectTransform>(); // mutable struct????
        }
        return _canvasRect.sizeDelta; 
    }
    
}
