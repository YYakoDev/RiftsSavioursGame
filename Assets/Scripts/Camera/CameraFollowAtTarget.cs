using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowAtTarget : MonoBehaviour
{
    [SerializeField]CameraTarget _cameraTargetManager;
    Transform _currentTarget;
    [SerializeField]float _smoothFollow = 10f;
    [SerializeField]Vector2 _offset;

    private void OnEnable() => _cameraTargetManager.onTargetSwitch += SwitchTarget;
    
    // Start is called before the first frame update
    void Start()
    {
        if(_cameraTargetManager == null)
        {
            _cameraTargetManager = GameObject.FindGameObjectWithTag("Player").GetComponent<CameraTarget>();
        }
        _currentTarget = _cameraTargetManager.Target;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(Mathf.Abs(Vector3.Distance(transform.position, _currentTarget.position)) < 0.2f)return;
        MoveCamera();
    }

    void MoveCamera()
    {
        Vector3 targetPosition = _currentTarget.position + (Vector3)_offset;
        targetPosition.z = transform.position.z;
        transform.position = Vector3.Lerp(transform.position, targetPosition, _smoothFollow * Time.fixedDeltaTime);
    }

    void SwitchTarget()
    {
        _currentTarget = _cameraTargetManager.Target;
    }

    private void OnDisable() => _cameraTargetManager.onTargetSwitch -= SwitchTarget;
}
