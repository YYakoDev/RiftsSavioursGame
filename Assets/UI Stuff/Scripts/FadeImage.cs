using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeImage : MonoBehaviour
{
    float _fadeDuration = 1f;
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
        _elapsedTime += Time.unscaledDeltaTime;
        float percent = _elapsedTime / _fadeDuration;
        var newColor = _img.color;
        newColor.a = Mathf.Lerp(_startValue, _endValue, _curve.Evaluate(percent));
        _img.color = newColor;
        if(_elapsedTime >= _fadeDuration) _onFadeComplete?.Invoke();
    }

    void Fade(bool fadeType, Action onComplete, float duration)
    {
        _fadeDuration = duration;
        _fadeIn = fadeType;
        SetValues();
        _onFadeComplete = onComplete;
    }
    public void FadeIn(Action onComplete = null, float duration = 0.5f) => Fade(true, onComplete, duration);
    public void FadeOut(Action onComplete = null, float duration = 0.5f) => Fade(false, onComplete, duration);
}

