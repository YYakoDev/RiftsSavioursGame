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
    static Vector3 _direction;
    static int _loops;
    static int _maxLoops = 7;
    [Range(0,5)]static int _shakeStrength = 2;

    private void Awake() {
        _camera = GetComponent<Camera>();
        _animator = GetComponent<TweenAnimator>();
        _cameraTransform = _camera.transform;
        _initialPos = _cameraTransform.localPosition;
        SetNewDirection(1);
    }

    public static void Shake(float strength)
    {
        SetNewDirection(strength);
        _animator.TweenTransformMoveTo(_cameraTransform, _direction, 0.02f, CurveTypes.EaseInOut, true, CheckShakeLoops);
    }


    static void CheckShakeLoops()
    {
        _loops++;
        if(_loops > _maxLoops)
        {
            _animator.Clear();
            //_animator.TweenTransformMoveTo(_cameraTransform, _initialPos, 0.01f, CurveTypes.EaseInOut);
            _loops = 0;
        }
    }

    static void SetNewDirection(float strength)
    {
        _direction = _cameraTransform.position + Vector3.one * (Random.Range(-0.06f, 0.06f) * strength);
    }
}
