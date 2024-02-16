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
        _cameraTransform = _camera.transform;
        _initialPos = _cameraTransform.localPosition;
    }
    public static void Shake(float strength, float duration = 0.012f)
    {
        if(_animator.AnimationsQueued > 3) _animator.Clear();
        int maxIterations = 6 + (int)(30f * duration);
        Vector3 previousDir = _cameraTransform.localPosition;
        for (int i = 0; i < maxIterations; i++)
        {
            var randomSign = Mathf.Sign(Random.Range(-1, 3));
            var realStrength = ((strength - (float)(i / 3.2f)));
            realStrength = Mathf.Clamp(realStrength, -strength/2f, strength);
            var dir = GetNewDirection(0.5f + realStrength * randomSign);
            var durationOffset = Random.Range(-0.002f, 0.0055f);
            var realDuration = (duration - durationOffset) / (float)maxIterations;
            _animator.TweenTransformMoveTo(_cameraTransform, previousDir, dir, realDuration, CurveTypes.EaseInOut);
            previousDir = dir;
        }
    }

    public static void StopShake()
    {
        _animator.Clear();
        _loops = 0;
    }

    static Vector3 GetNewDirection(float strength)
    {
        Vector3 randomDir = new Vector3(GetRandomShakeRange * strength, GetRandomShakeRange * strength, 0f) ;
        return _cameraTransform.position + randomDir;
    }
}
