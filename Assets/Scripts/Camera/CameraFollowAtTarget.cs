using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowAtTarget : MonoBehaviour
{
    [SerializeField]CameraTarget _cameraTargetManager;
    Transform _currentTarget;
    [SerializeField]float _smoothFollow = 10f;
    [SerializeField]Vector2 _offset;
    [SerializeField] float _distanceThreshold = 0.4f;

    private void OnEnable() => _cameraTargetManager.onTargetSwitch += SwitchTarget;
    
    // Start is called before the first frame update
    void Start()
    {
        if(_cameraTargetManager == null) _cameraTargetManager = GameObject.FindGameObjectWithTag("Player").GetComponent<CameraTarget>();
        
        _currentTarget = _cameraTargetManager.Target;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 currentPosition = transform.position;
        if(Mathf.Abs(Vector3.Distance(currentPosition, _currentTarget.position)) < _distanceThreshold) return;
        MoveCamera(currentPosition);
    }

    void MoveCamera(Vector3 currentPos)
    {
        Vector3 targetPosition = _currentTarget.position + (Vector3)_offset;
        targetPosition.z = currentPos.z;
        transform.position = Vector3.Lerp(currentPos, targetPosition, _smoothFollow * Time.fixedDeltaTime);
    }

    void SwitchTarget()
    {
        _currentTarget = _cameraTargetManager.Target;
    }

    private void OnDisable() => _cameraTargetManager.onTargetSwitch -= SwitchTarget;
}
