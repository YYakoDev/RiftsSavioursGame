using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TweenAnimatorMultiple))]
public class RewardAnimator : MonoBehaviour
{
    [SerializeField] private Canvas _canvas;
    [SerializeField] UIRewardItem _rewardPrefab;
    [SerializeField] TweenAnimatorMultiple _animator;
    UIRewardItem _rewardInstance;
    [SerializeField] Vector3 _endPos, _scaleOffset, _positionOffset;
    [SerializeField] float _scaleUpDuration, _moveDuration, _readDuration;
    [SerializeField] CurveTypes _scaleCurve, _moveCurve;

    private void Awake()
    {
        if (_canvas == null) _canvas = transform.root.GetComponent<Canvas>();
        _rewardInstance = Instantiate(_rewardPrefab, transform);
        _rewardInstance.gameObject.SetActive(false);
        MoveAnimation();
    }

    public void Play(SORewardItem item)
    {
        _rewardInstance.Rect.localScale = Vector3.zero;
        _rewardInstance.Rect.localPosition = Vector3.zero;
        _rewardInstance.Set(item.Sprite, item.Name, item.Description);
        _rewardInstance.gameObject.SetActive(true);
        //_animator.TweenRotate(_rewardInstance.Rect, 180f, _spinDuration, _spinCurve, false, CheckSpins);
        _animator.TweenScaleBouncy(_rewardInstance.Rect, Vector3.one, _scaleOffset, _scaleUpDuration, 0, 5, _scaleCurve, onBounceComplete: ScaleDown);
        YYExtensions.i.ExecuteEventAfterTime(_scaleUpDuration / 1.75f, MoveAnimation);
    }

    void MoveAnimation()
    {
        _animator.TweenMoveToBouncy(_rewardInstance.Rect, _endPos, _positionOffset, _moveDuration, 0, 3, _moveCurve);
    }

    void ScaleDown()
    {
        _animator.Scale(_rewardInstance.Rect, Vector3.one, _readDuration, CurveTypes.Linear, onComplete: () => 
        {
            _animator.Scale(_rewardInstance.Rect, Vector3.zero, _scaleUpDuration, CurveTypes.EaseInOut);
        });
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireCube(_canvas.TranslateUiToWorldPoint(_endPos), Vector3.one * 25f);
    }
}
