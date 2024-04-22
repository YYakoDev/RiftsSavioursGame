using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeImage : MonoBehaviour
{
    [SerializeField] float _fadeDuration = 0.5f;
    [SerializeField] bool _fadeIn = false;
    Image _img;
    AnimationCurve _curve;
    float _elapsedTime = 0f, _startValue, _endValue;
    event Action _onFadeComplete = null;
    private void Awake() {
        _img = GetComponent<Image>();
        SetValues();
    }

    void SetValues()
    {
        _startValue = (_fadeIn) ? -0.01f:1.01f;
        _endValue = (_fadeIn) ? 1.01f:-0.01f;
        var color = _img.color;
        color.a = _startValue;
        _img.color = color;
        _elapsedTime = 0f;
    }

    private void OnEnable() {
        SetValues();
    }

    private void Start() {
        _curve = TweenCurveLibrary.GetCurve(CurveTypes.EaseInOut);
    }

    // Update is called once per frame
    void Update()
    {
        if(_elapsedTime >= _fadeDuration) return;
        _elapsedTime += Time.deltaTime;
        float percent = _elapsedTime / _fadeDuration;
        var newColor = _img.color;
        newColor.a = Mathf.Lerp(_startValue, _endValue, _curve.Evaluate(percent));
        _img.color = newColor;
        if(_elapsedTime >= _fadeDuration) _onFadeComplete?.Invoke();
    }

    public void FadeIn(Action onComplete = null)
    {
        _fadeIn = true;
        SetValues();
        _onFadeComplete = onComplete;
    }
    public void FadeOut(Action onComplete = null)
    {
        _fadeIn = false;
        SetValues();
        _onFadeComplete = onComplete;
    }
}

