using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthAnimations : MonoBehaviour
{
    [SerializeField]TweenAnimator _heartAnimator, _barAnimator;
    [SerializeField]RectTransform _heartIcon, _healthBar;
    [SerializeField] Image _fillImg;
    RectTransform _fillRect;
    Color _startingColor;
    [SerializeField]Color _blinkColor;
    Vector3 _initialSize;
    int _blinkLoops = 0;
    int _maxBlinkTimes = 2;

    private void Awake() {
        _heartAnimator = _heartIcon.GetComponent<TweenAnimator>();
        _barAnimator = _healthBar.GetComponent<TweenAnimator>();
        _initialSize = _heartIcon.localScale;
        if(_fillImg != null)
        {
            _fillRect = _fillImg.rectTransform;
            _startingColor = _fillImg.color;
        }
    }

    public void ShakeAnimation()
    {
        _heartAnimator.Scale(_heartIcon, _initialSize * 0.5f, 0.1f,
        onComplete: () => 
        {
            _heartAnimator.Scale(_heartIcon, _initialSize, 0.1f);
        });
    }

    public void BlinkBarAnim()
    {
        _barAnimator.TweenImageColor(_fillRect, _startingColor, _blinkColor, 0.05f, CurveTypes.Linear, true, CheckBlinkTime);
        //_barAnimator.TweenImageOpacity(_healthBar, 0, 0.05f, CurveTypes.Linear, true, CheckBlinkTime);
    }

    void CheckBlinkTime()
    {
        _blinkLoops++;
        if(_blinkLoops > _maxBlinkTimes)
        {
            StopBlink();
            _blinkLoops = 0;
        }
    }

    void StopBlink()
    {
        _barAnimator.AnimationComplete();
        //_barAnimator.TweenImageOpacity(_healthBar, 255, 0.05f, CurveTypes.EaseInOut);
        _barAnimator.TweenImageColor(_fillRect, _blinkColor, _startingColor, 0.025f);
    }

    public void Stop()
    {
        _heartAnimator.Clear();
        _barAnimator.Clear();
    }
}
