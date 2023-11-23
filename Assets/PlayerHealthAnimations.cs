using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(TweenAnimator))]
public class PlayerHealthAnimations : MonoBehaviour
{
    TweenAnimator _animator;
    [SerializeField]RectTransform _heartIcon;
    [SerializeField]RectTransform _healthBar;
    Vector3 _initialSize;
    int _blinkLoops = 0;
    int _maxBlinkTimes = 2;

    private void Awake() {
        GameObject thisGO = gameObject;
        thisGO.CheckComponent<TweenAnimator>(ref _animator);
        _initialSize = _heartIcon.localScale;
    }

    public void ShakeAnimation()
    {
        _animator.Scale(_heartIcon, _initialSize * 2, 0.2f,
        onComplete: () => 
        {
            _animator.Scale(_heartIcon, _initialSize, 0.2f);
        });
    }

    public void BlinkBarAnim()
    {
        gameObject.CheckComponent<TweenAnimator>(ref _animator);
        _animator.TweenImageOpacity(_healthBar, 0, 0.05f, CurveTypes.Linear, true, CheckBlinkTime);
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
        _animator.TweenImageOpacity(_healthBar, 255, 0.05f, CurveTypes.EaseInOut);
    }
}
