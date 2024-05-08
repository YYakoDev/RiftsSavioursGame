using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TweenAnimator))]
public class CameraShake : MonoBehaviour
{
    static Camera _camera;
    static Transform _cameraTransform;
    static TweenAnimator _animator;
    static Vector3 _initialPos;
    static int _loops;

    private static float GetRandomShakeRange => Random.Range(-0.045f, 0.055f);

    private void Awake() {
        _camera = GetComponent<Camera>();
        _animator = GetComponent<TweenAnimator>();
        _animator.ChangeTimeScalingUsage(TweenAnimator.TimeUsage.UnscaledTime);
        _cameraTransform = _camera.transform;
        _initialPos = _cameraTransform.localPosition;
    }
    public static void Shake(float strength, float duration = 0.021f)
    {
        if(_animator.AnimationsQueued > 3) _animator.Clear();
        int maxIterations = 6 + (int)(30f * duration);
        Vector3 previousDir = _cameraTransform.localPosition;
        for (int i = 0; i < maxIterations; i++)
        {
            var randomSign = Mathf.Sign(Random.Range(-1, 2));
            var dir = GetNewDirection(1.5f + strength * randomSign);
            var realDuration = (duration) / (float)maxIterations;
            _animator.TweenTransformMoveTo(_cameraTransform, previousDir, dir, realDuration, CurveTypes.EaseInOut);
            previousDir = dir;
        }
    }

    static Vector3 GetNewDirection(float strength)
    {
        Vector3 randomDir = new Vector3(GetRandomShakeRange * strength, GetRandomShakeRange * strength, 0f) ;
        return _cameraTransform.position + randomDir;
    }
}
