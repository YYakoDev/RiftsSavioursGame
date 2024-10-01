using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TweenAnimatorMultiple))]
public class RewardAnimator : MonoBehaviour
{
    [SerializeField] UIRewardItem _rewardPrefab;
    [SerializeField] TweenAnimatorMultiple _animator;
    UIRewardItem _rewardInstance;
    [SerializeField] Vector3 _endPos, _scaleOffset, _positionOffset;
    [SerializeField] int _spins = 5;
    [SerializeField] float _scaleUpDuration, _spinDuration, _moveDuration;
    [SerializeField] CurveTypes _scaleCurve, _spinCurve, _moveCurve;
    int _spinsCount;

    public void Play(RewardItem item)
    {
        _spinsCount = 0;
        _rewardInstance.Rect.localScale = Vector3.zero;
        _rewardInstance.Rect.localPosition = Vector3.zero;
        _rewardInstance.Set(item.Sprite, item.Name, item.Description);
        _animator.TweenRotate(_rewardInstance.Rect, 180f, _spinDuration, _spinCurve, false, CheckSpins);
        _animator.TweenScaleBouncy(_rewardInstance.Rect, Vector3.one, _scaleOffset, _scaleUpDuration, 0, 5, _scaleCurve);
        YYExtensions.i.ExecuteEventAfterTime(_scaleUpDuration / 1.75f, MoveAnimation);
    }

    void MoveAnimation()
    {
        _animator.TweenMoveToBouncy(_rewardInstance.Rect, _endPos, _positionOffset, _moveDuration, 0, 4, _moveCurve);
    }

    void CheckSpins()
    {
        if(_spinsCount >= _spins) return;
        _spinsCount++;
        _animator.TweenRotate(_rewardInstance.Rect, 180f, _spinDuration / _spins, _spinCurve, false, CheckSpins);
    }
}
