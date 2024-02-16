using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeInImage : MonoBehaviour
{
    [SerializeField] float _fadeDuration = 0.5f;
    Image _img;
    AnimationCurve _curve;
    float _elapsedTime = 0f;
    private void Awake() {
        _img = GetComponent<Image>();
    }

    private void OnEnable() {
        _elapsedTime = 0f;
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
        newColor.a = Mathf.Lerp(1, -0.05f, _curve.Evaluate(percent));
        _img.color = newColor;
    }
}
