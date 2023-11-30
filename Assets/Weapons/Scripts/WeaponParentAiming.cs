using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponParentAiming : MonoBehaviour
{
    [Header("References")]
    [SerializeField]PlayerManager _player;
    float _stopAimingTime = 0f;

    [Header("Point To Mouse References")]
    [SerializeField]private Camera _mainCamera;
    Vector2 _mouseDirection = Vector2.zero;

    [Header("Auto Targetting")]
    //References
    [SerializeField]RectTransform _selectionIcon;
    LayerMask _targetLayer;
    Collider2D[] _targetsDetected = new Collider2D[10];

    //Stats
    Vector2 _pointingDirection = Vector2.zero;
    [SerializeField]float _detectionRadius = 2f;
    bool _autoTargetting = true;
    int _resultsCount = 0;
    [SerializeField]float _aimSmoothing = 10f;
    const float DetectionRate = 0.1f; //seconds
    float _nextDetectionTime = 0f;


    //Weapon
    [Header("Weapon")]
    private WeaponBase _currentWeapon;
    private float _attackDuration => _currentWeapon.AtkDuration;


    //CameraTarget
    int _weaponCameraIndex = 0;
    WaitForSeconds _waitForAttackDuration;
    Coroutine _returnCameraTarget;

    //properties
    public bool AutoTargetting => _autoTargetting;
    public Vector3 MouseDirection => _mouseDirection;
    public Vector3 PointingDirection => _pointingDirection;

    private IEnumerator Start() {
        if (_mainCamera == null)
        {
            _mainCamera = Camera.main;
        }
        yield return null; //just in case

        //default values
        _autoTargetting = true;
        _nextDetectionTime = 1f;

        _weaponCameraIndex = _player.CameraTarget.AddTarget(_currentWeapon.PrefabTransform);
        _waitForAttackDuration = new WaitForSeconds(_attackDuration + 0.25f);

        Cursor.lockState = CursorLockMode.Locked;

        //Events
        _currentWeapon.onAttack += StopAiming;
        _currentWeapon.onAttack += PointCameraToWeapon;
    }

    public void Initialize(LayerMask enemyLayer, WeaponBase currentWeapon)
    {
        _targetLayer = enemyLayer;
        _currentWeapon = currentWeapon;
    }

    void Update()
    {
        if(Input.GetButtonDown("SwitchAim"))
        {
            _autoTargetting = !_autoTargetting;
            if(_autoTargetting)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }else
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
        }
        if(_stopAimingTime > 0)
        {
            _stopAimingTime -= Time.deltaTime;
        }
    }

    private void FixedUpdate()
    {
        if(_stopAimingTime > 0)
        {
            _selectionIcon.gameObject.SetActive(false);
            return;
        }
        if(_autoTargetting)
        {
            if(_nextDetectionTime <= Time.time )
            {
                _nextDetectionTime = Time.time + DetectionRate;
                DetectTargets();
            }
            PointToTarget();
        }else
        {
            RotateWithMouse();
        }

    }

    #region Weapon Aiming

        #region Point To Mouse
    void RotateWithMouse()
    {
        _mouseDirection = _mainCamera.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        float angle = Mathf.Atan2(-_mouseDirection.y,-_mouseDirection.x) * Mathf.Rad2Deg;
        angle = Mathf.LerpAngle(transform.rotation.eulerAngles.z, angle, Time.fixedDeltaTime * _aimSmoothing);
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        FlipWeapon(_mouseDirection);

        _selectionIcon.gameObject.SetActive(false);

    }
    #endregion
        #region Auto Targetting
    void DetectTargets()
    {
        _resultsCount = Physics2D.OverlapCircleNonAlloc(transform.position, _detectionRadius, _targetsDetected, _targetLayer);
    }
    void PointToTarget()
    {
        bool activeIcon = false;
        if(_resultsCount == 0)
        {
            _selectionIcon.gameObject.SetActive(activeIcon);
            return;
        }
        _pointingDirection = Vector2.zero;

        for(int i = 0; i < _resultsCount; i++)
        {
            if(!_targetsDetected[i].isTrigger)
            {
                continue;
            }
            Vector2 directionToTarget = _targetsDetected[i].bounds.center - transform.position;
            if(_pointingDirection == Vector2.zero)
            {
                _pointingDirection = directionToTarget;
                continue;
            }
            if(directionToTarget.magnitude < _pointingDirection.magnitude)
            {
                _pointingDirection = directionToTarget;
            }
        }

        activeIcon = (_pointingDirection != Vector2.zero);
        if(!activeIcon) return;
        float angle = Mathf.Atan2(-_pointingDirection.y,-_pointingDirection.x) * Mathf.Rad2Deg;
        angle = Mathf.LerpAngle(transform.rotation.eulerAngles.z, angle, Time.fixedDeltaTime * _aimSmoothing);
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        FlipWeapon(_pointingDirection);
        
        _selectionIcon.gameObject.SetActive(activeIcon);
        _selectionIcon.position = (Vector3)_pointingDirection + transform.position;
    }
    #endregion
    void FlipWeapon(Vector2 direction)
    {
        if(_stopAimingTime > 0) return;
        Vector2 scale = transform.localScale;
        if(direction.x < 0)
        {
            scale.y = 1; 
        }
        else if(direction.x > 0)
        {
            scale.y = -1; 
        }
    
        transform.localScale = scale;
    }
    void StopAiming()
    {
        _stopAimingTime = _attackDuration / 1.25f;
    }
    #endregion

    #region Camera Effect

    void PointCameraToWeapon()
    {
        if(!_currentWeapon.PointCameraOnAttack) return;
        _player.CameraTarget.SwitchTarget(_weaponCameraIndex);
        if(_returnCameraTarget != null) StopCoroutine(_returnCameraTarget);
        _returnCameraTarget = StartCoroutine(StopPointingCamera());
    }

    IEnumerator StopPointingCamera()
    {
        yield return _waitForAttackDuration;
        _player.CameraTarget.SwitchTarget(0);
    }

    #endregion

    private void OnDrawGizmosSelected() {
        if(Application.isPlaying) return;
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position,_detectionRadius);

    }

    private void OnDestroy() {
        _currentWeapon.onAttack -= StopAiming;
        _currentWeapon.onAttack -= PointCameraToWeapon;
    }
}
