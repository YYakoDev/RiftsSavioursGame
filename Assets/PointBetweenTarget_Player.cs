using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointBetweenTarget_Player : MonoBehaviour
{
    [SerializeField] Transform _player;
    Camera _mainCamera;
    [HideInInspector]public Vector3 TargetPosition = Vector3.zero;
    [SerializeField] float _mouseDistance = 1f;

    private void Start()
    {
        _mainCamera = Camera.main;
    }

    private void FixedUpdate() {
        if(TargetPosition.sqrMagnitude > 0.1f) Move();
    }

    void Move()
    {
        Vector3 playerPosition = _player.position;
        TargetPosition.z = 0;
        Vector3 dirTowardsMouse = TargetPosition - playerPosition;
        dirTowardsMouse = Vector3.ClampMagnitude(dirTowardsMouse, _mouseDistance);
        transform.localPosition = playerPosition + dirTowardsMouse;
    }
}
