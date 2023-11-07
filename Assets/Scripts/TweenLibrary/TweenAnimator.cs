using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TweenAnimator : MonoBehaviour
{
    Camera _mainCamera;
    RectTransform _rectTransform;
    Vector3 _startPosition;
    Vector3 _destination;
    float _elapsedTime;
    float _totalDuration;
    AnimationCurve _curve;
    event Action _onComplete;
    bool _loop = false;
    bool _enabled = false;
    Action TweenAction;

    private void Awake() {
        if(_mainCamera == null) _mainCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if(!_enabled) return;

        TweenAction?.Invoke();

    }


    public void MoveTo(RectTransform rectTransform, Vector3 endPosition, float duration, Action onComplete = null, bool loop = false)
    {
        _rectTransform = rectTransform;
        _startPosition = rectTransform.localPosition;
        Debug.Log(_startPosition);
        _destination = endPosition;
        _totalDuration = duration;
        _onComplete = onComplete;
        _loop = loop;
        SwitchTweenAction(Move);
        _enabled = true;
    }

    void Move()
    {
        _elapsedTime += Time.deltaTime;
        float percent = _elapsedTime / _totalDuration;
        //_rectTransform.position = Vector3.Lerp(_startPosition, _destination, percent);
        _rectTransform.localPosition = _startPosition;

        if(_elapsedTime >= _totalDuration)
        {
            _onComplete?.Invoke();
            if(_loop)
            {
                _enabled = true;
                Vector3 oldStartPos = _startPosition;
                _startPosition = _destination;
                _destination = oldStartPos;
                _elapsedTime = 0;
            }else
            {
                _enabled = false;
            }
        }
    }

    void SwitchTweenAction(Action voidMethod)
    {
        TweenAction = voidMethod;
    }

    Vector3 TranslateRectPosition(Vector3 position)
    {
        return _mainCamera.WorldToScreenPoint(Vector3.one * 2);
    }
}
