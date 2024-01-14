using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointBetweenTarget_Player : MonoBehaviour
{
    [SerializeField] Transform _player;
    [SerializeField] WeaponAiming _aimingScript;
    Camera _mainCamera;
    private Vector3 TargetPosition => _aimingScript.TargetPoint;
    [SerializeField] float _mouseDistance = 1f;
    float _realDistance;
    [SerializeField] float _distanceThreshold = 1f;
    float _realThreshold;
    private bool _autoAiming;

    private void Start()
    {
        _mainCamera = Camera.main;
        _aimingScript.OnAimingChange += ChangeAimMode;
        _realThreshold = _distanceThreshold;
        _realDistance = _mouseDistance;
    }

    private void FixedUpdate() {
        Move();
    }

    void Move()
    {
        Vector3 playerPosition = _player.position;
        float distance = Vector3.Distance(TargetPosition, playerPosition);
        if(distance < _realThreshold)
        {
            MoveTowardsPlayer();
            return;
        }
        Vector3 dirTowardsMouse = TargetPosition - playerPosition;
        dirTowardsMouse = Vector3.ClampMagnitude(dirTowardsMouse, _realDistance);
        dirTowardsMouse.z = 0;
        transform.localPosition = playerPosition + dirTowardsMouse;
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
            _realDistance = _mouseDistance / 1.75f;
            _realThreshold = _distanceThreshold / 3f;
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
