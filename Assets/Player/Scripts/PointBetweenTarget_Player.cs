using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointBetweenTarget_Player : MonoBehaviour
{
    [SerializeField] Transform _player;
    [SerializeField] WeaponAiming _aimingScript;
    [SerializeField] CurveTypes _curveType;
    AnimationCurve _curve;
    Camera _mainCamera;
    private Vector3 TargetPosition => _aimingScript.TargetPoint;
    [SerializeField] float _mouseDistance = 1f, _mouseFollowDuration = 1f, _distanceThreshold = 1f;
    float _realDistance, _realThreshold, _mouseFollowingTime;
    private bool _autoAiming;

    private void Start()
    {
        _mainCamera = Camera.main;
        _aimingScript.OnAimingChange += ChangeAimMode;
        _realThreshold = _distanceThreshold;
        _realDistance = _mouseDistance;
        _curve = TweenCurveLibrary.GetCurve(_curveType);
        
    }

    private void FixedUpdate() {
        Move();
    }

    void Move()
    {
        var percent = _mouseFollowingTime / _mouseFollowDuration;
        var targetPos = Vector3.Lerp(Vector3.zero, TargetPosition, _curve.Evaluate(percent));
        Vector3 playerPosition = _player.position;
        float distance = Vector3.Distance(TargetPosition, playerPosition);
        if(distance < _realThreshold)
        {
            if(_mouseFollowingTime > 0f)_mouseFollowingTime -= Time.fixedDeltaTime * 3f;
            MoveTowardsPlayer();
            return;
        }
        Vector3 dirTowardsMouse = targetPos;
        dirTowardsMouse = Vector3.ClampMagnitude(dirTowardsMouse, _realDistance);
        dirTowardsMouse.z = 0f;
        transform.localPosition = playerPosition + dirTowardsMouse;
        if(_mouseFollowingTime < _mouseFollowDuration) _mouseFollowingTime += Time.fixedDeltaTime;
    }

    void MoveTowardsPlayer()
    {
        Vector3 playerPosition = _player.position;
        transform.localPosition = playerPosition;
    }

    void ChangeAimMode(bool autoAiming)
    {
        _autoAiming = autoAiming;
        if(_autoAiming)
        {
            _realDistance = _mouseDistance / 1.55f;
            _realThreshold = _distanceThreshold * 0.05f;
        }else
        {
            _realThreshold = _distanceThreshold;
            _realDistance = _mouseDistance;
        }
    }
    
    
    private void OnDestroy() {
        _aimingScript.OnAimingChange -= ChangeAimMode;
    }
}
