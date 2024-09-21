using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(TweenAnimatorMultiple))]
public class TokenItemUIAnimation : MonoBehaviour
{
    [SerializeField] Canvas _canvas;
    TweenAnimatorMultiple _animator;
    [SerializeField] RectTransform _rect;
    [SerializeField] Vector2 _startPosition, _startScale, _endPosition, _endScale;
    [SerializeField] float _scaleDuration, _moveDuration;
    [SerializeField] CurveTypes _curveType;
    [SerializeField] TextMeshProUGUI _text;
    bool _hidden = true;
    //AnimationCurve _curve;

    private void Awake() {
        _animator = GetComponent<TweenAnimatorMultiple>();
        _rect.localScale = _startScale;
        _rect.localPosition = _startPosition;
        _hidden = true;
        //_curve = TweenCurveLibrary.GetCurve(_curveType);
    }

    public void SetText(string content)
    {
        _text.SetText(content);
    }

    public void Play()
    {
        if(_hidden)
        PlayAnimations(_endPosition, _endScale);
        else Shake();
    }

    public void Shake()
    {
        _animator.TweenScaleBouncy(_rect, _endScale * 1.25f, Vector3.up / 4f, _scaleDuration / 1.5f, 0, 7, _curveType, false, () => 
        {
            _animator.Scale(_rect, _endScale, _scaleDuration / 3.3f, _curveType);
        });
    }

    public void Hide()
    {
        PlayAnimations(_startPosition, _startScale);
        _hidden = true;
    }

    void PlayAnimations(Vector2 endPos, Vector2 endScale)
    {
        _hidden = false;
        _animator.MoveTo(_rect, endPos, _moveDuration, _curveType);
        _animator.Scale(_rect, endScale, _scaleDuration, _curveType);
    }

    private void OnDrawGizmosSelected() {
        if(Application.isPlaying) return;
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireCube(_canvas.TranslateUiToWorldPoint(_startPosition), Vector3.one * 15f);
        Gizmos.DrawWireCube(_canvas.TranslateUiToWorldPoint(_endPosition), Vector3.one * 25f);
    }

}
