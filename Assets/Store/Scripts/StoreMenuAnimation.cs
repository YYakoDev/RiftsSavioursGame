using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(TweenAnimatorMultiple))]
public class StoreMenuAnimation : MonoBehaviour
{
    [SerializeField] Canvas _mainCanvas;
    TweenAnimatorMultiple _animator;
    [SerializeField] Image _bg;
    [SerializeField] RectTransform _storeBanner, _merchant, _closeButton, _upgradesContainer, _balanceText, _rerollButton;
    [SerializeField] float _bgFadeInDuration = 0.6f, _animDuration = 0.4f;
    [SerializeField] Vector3 _storeBannerStartPos, _storeBannerEndPos, _merchantStartPos, _merchantEndPos,
     _closeBtnEndScale, _itemsContainerStartPos,_itemsContainerEndPos, _balanceTextEndScale, _rerollBtnEndScale;
    Timer _bgMidAnimationTimer;
    bool _animationFinished = false;
    //audio stuff
    [SerializeField] AudioSource _audio;
    [SerializeField] AudioClip _openingSFX;
    //properties
    public bool IsFinished => _animationFinished;
    private void Awake() {
        _animator = GetComponent<TweenAnimatorMultiple>();
        _animator.ChangeTimeScalingUsage(TweenAnimator.TimeUsage.UnscaledTime);
        _bgMidAnimationTimer = new(_bgFadeInDuration / 1.65f, useUnscaledTime: true);
        _bgMidAnimationTimer.Stop();
        _bgMidAnimationTimer.onEnd += PlayRestOfAnimations;
    }

    private void OnEnable() {
        _animator.Clear();
        StartAnimation();
    }

    private void Update() {
        _bgMidAnimationTimer.UpdateTime();
    }
    void StartAnimation()
    {
        var newColor = _bg.color;
        newColor.a = 0;
        _bg.color = newColor;
        _animationFinished = false;
        _audio.PlayWithVaryingPitch(_openingSFX);
        BGFadeIn();
    }

    void BGFadeIn()
    {
        _animator.TweenImageOpacity(_bg.rectTransform, 255, _bgFadeInDuration);
        _bgMidAnimationTimer.Start();
        _storeBanner.localPosition = _storeBannerStartPos;
        _merchant.localPosition = _merchantStartPos;
        _upgradesContainer.localPosition = _itemsContainerStartPos;
        _closeButton.localScale = Vector3.zero;
        _rerollButton.localScale = Vector3.zero;
        _balanceText.localScale = Vector3.zero;
    }

    void PlayRestOfAnimations()
    {
        TweenMove(_storeBanner, _storeBannerEndPos);
        TweenMove(_merchant, _merchantEndPos);
        TweenMove(_upgradesContainer, _itemsContainerEndPos);
        TweenScale(_closeButton, _closeBtnEndScale);
        TweenScale(_rerollButton, _rerollBtnEndScale);
        TweenScale(_balanceText, _balanceTextEndScale);
        _animator.TweenImageOpacity(_bg.rectTransform, 255, _bgFadeInDuration, onComplete:() => _animationFinished = true);
    }

    void TweenMove(RectTransform rect, Vector3 end)
    {
        _animator.TweenMoveToBouncy(rect, end, end * 0.1f, _animDuration, 0, 5);
    }

    void TweenScale(RectTransform rect, Vector3 end)
    {
        _animator.TweenScaleBouncy(rect, end, end * 0.1f ,_animDuration, 0, 5);
    }

    private void OnDestroy() {
        _bgMidAnimationTimer.onEnd -= PlayRestOfAnimations;
    }

    private void OnDrawGizmosSelected() {
        if(Application.isPlaying) return;
        if(_mainCanvas == null) return;
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(_mainCanvas.TranslateUiToWorldPoint(_storeBannerStartPos), Vector3.one * 20f);
        Gizmos.DrawWireCube(_mainCanvas.TranslateUiToWorldPoint(_storeBannerEndPos), Vector3.one * 30f);
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireCube(_mainCanvas.TranslateUiToWorldPoint(_merchantStartPos), Vector3.one * 20f);
        Gizmos.DrawWireCube(_mainCanvas.TranslateUiToWorldPoint(_merchantEndPos), Vector3.one * 30f);
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(_mainCanvas.TranslateUiToWorldPoint(_itemsContainerStartPos), Vector3.one * 35f);
        Gizmos.DrawWireCube(_mainCanvas.TranslateUiToWorldPoint(_itemsContainerEndPos), Vector3.one * 50f);
    }
}

