using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(TweenAnimatorMultiple))]
public class UpgradeMenuAnimations : MonoBehaviour
{
    [Header("References")]
    [SerializeField] RectTransform _menuParent;
    [SerializeField] Image _BG;
    [SerializeField] Image _anvilImg;
    [SerializeField] Image _choicesLeftImg;
    RectTransform[] _upgradeItemsInstance = new RectTransform[0];
    Vector3[] _itemStartingScales = new Vector3[0];
    TweenAnimatorMultiple _animator;
    Canvas _canvas;

    [Header("Animations")]
    [SerializeField] float _anvilAnimDuration = 0.7f;
    [SerializeField] Vector3 _anvilEndPosition;
    [SerializeField] Vector3 _anvilMovementOffset = Vector3.up;
    [SerializeField] int _anvilMovementBounces = 4;
    [SerializeField] float _bgFadeInDuration = 0.5f;
    [SerializeField, Range(0, 255)] int _bgEndOpacity = 188;
    [SerializeField] float _itemScaleAnimDuration = 1f;
    [SerializeField] int _scaleBounces = 5;
    [SerializeField] Vector3 _itemsScaleOffset;
    [SerializeField] float _choicesAmountAnimDuration = 0.35f;

    [Header("Audio Stuff")]
    [SerializeField] AudioSource _audio;
    [SerializeField] AudioClip _anvilSFX, _BGSFX, _upgradeItemSFX;

    private void Awake() {
        _animator = GetComponent<TweenAnimatorMultiple>();
    }

    private void Start() {
        _canvas = _BG.canvas;
    }

    public void SetElements(UpgradeItemPrefab[] items)
    {
        if(_upgradeItemsInstance.Length != items.Length)
        {
            Array.Resize<RectTransform>(ref _upgradeItemsInstance, items.Length);
            Array.Resize<Vector3>(ref _itemStartingScales, items.Length);
        }
        for (int i = 0; i < _upgradeItemsInstance.Length; i++)
        {
            _upgradeItemsInstance[i] = items[i].GetComponent<RectTransform>();
            _itemStartingScales[i] = _upgradeItemsInstance[i].localScale;
        }
    }

    void SetInitialStates()
    {
        _animator.Clear();
        var bgColor = _BG.color;
        bgColor.a = 0;
        _BG.color = bgColor;

        //_anvilImg.rectTransform.localPosition = Vector3.down * 2160;
        var anvilNewColor = _anvilImg.color;
        anvilNewColor.a = 0;
        _anvilImg.color = anvilNewColor;

        foreach(var item in _upgradeItemsInstance) item.localScale = new Vector3(0, 1, 1);

        _choicesLeftImg.rectTransform.localScale = Vector3.zero;
    }

    public void Play(Action onComplete)
    {
        SetInitialStates();
        PlayAudio(_BGSFX);
        _animator.TweenImageOpacity(_BG.rectTransform, _bgEndOpacity, _bgFadeInDuration, onComplete: () =>
        {
            PlayAudio(_anvilSFX);
            _animator.TweenImageOpacity(_anvilImg.rectTransform, 255, _anvilAnimDuration);
            _animator.Scale(_choicesLeftImg.rectTransform, Vector3.one, _choicesAmountAnimDuration);
            _animator.TweenMoveToBouncy
            (_anvilImg.rectTransform, _anvilEndPosition, _anvilMovementOffset, _anvilAnimDuration, 0, _anvilMovementBounces, onBounceComplete: () =>
            {
                onComplete?.Invoke();
                for (int i = 0; i < _upgradeItemsInstance.Length; i++)
                {
                    var item = _upgradeItemsInstance[i];
                    var endScale = _itemStartingScales[i];

                    _animator.TweenScaleBouncy(item, endScale, _itemsScaleOffset, _itemScaleAnimDuration, 0, _scaleBounces);
                    PlayAudio(_upgradeItemSFX);
                }

            });
        });
    }

    public void PlayCloseAnimation(Action onComplete)
    {
        _animator.Scale(_menuParent, Vector3.zero, _bgFadeInDuration, onComplete: onComplete);
    }

    void PlayAudio(AudioClip clip) => _audio?.PlayWithVaryingPitch(clip);

    private void OnDrawGizmosSelected() 
    {
        if(Application.isPlaying) return;
        if(_canvas == null) _canvas = _BG.canvas;
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireCube(_canvas.TranslateUiToWorldPoint(_anvilEndPosition), Vector3.one * 35);
    }
}
