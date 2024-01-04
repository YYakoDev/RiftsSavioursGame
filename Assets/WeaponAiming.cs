using System;
using UnityEngine;

public class WeaponAiming : MonoBehaviour
{
    [Header("References")]
    [SerializeField]Camera _mainCamera;
    [SerializeField]Transform _crosshair;

    private Vector3 _mousePosition;
    Vector2 _targetDirection;
    WeaponBase _currentWeapon;

    //enemy detection
    Vector2 _closestEnemyPos;
    Timer _enemyDetectionTimer;
    LayerMask _enemyLayer;
    Collider2D[] _targetsDetected = new Collider2D[15];
    int _resultsCount = 0;


    [Header("Values")]
    [SerializeField] float _aimSmoothing = 8f;
    float _detectionRadius = 5f;
    float _stopAimingTime = 0f;
    bool _autoAiming = false;
    public event Action<bool> OnAimingChange;


    // properties
    Vector2 _targetPoint;
    public Vector2 TargetPoint => _targetPoint;
    

    private void Awake()
    {
        _enemyDetectionTimer = new(0.1f, true);
        _enemyDetectionTimer.onEnd += DetectEnemy;
    }

    public void Initialize(WeaponBase currentWeapon, LayerMask layer)
    {
        _currentWeapon = currentWeapon;
        _enemyLayer = layer;
    }

    void Start()
    {
        if(_mainCamera == null) _mainCamera = Camera.main;
        _crosshair.gameObject.SetActive(false);
        _currentWeapon.onAttack += StopAiming;
    }

    // Update is called once per frame
    void Update()
    {
        if(_stopAimingTime > 0)
        {
            _stopAimingTime -= Time.deltaTime;
            return;
        }
        if(Input.GetButtonDown("SwitchAim"))
        {
            _autoAiming = !_autoAiming;
            if(_autoAiming)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }else
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                _crosshair.gameObject.SetActive(false);
            }
            OnAimingChange?.Invoke(_autoAiming);
        }

        if(_autoAiming) _enemyDetectionTimer.UpdateTime();
    }

    private void FixedUpdate()
    {
        if(_stopAimingTime > 0) return;
        if(_autoAiming)
        {
            GetNearestEnemy();
            _targetDirection = _closestEnemyPos;
        }
        else
        {
            _mousePosition = _mainCamera.ScreenToWorldPoint(Input.mousePosition); 
            _targetDirection = _mousePosition - transform.position;
            SetTargetPoint(_mousePosition);
        }

        if (_targetDirection.sqrMagnitude > 0.1f) PointToTarget(); //maybe is less expensive if we dont calculate the sqr magnitude and just point to target?
     
    }

    private void SetTargetPoint(Vector3 v)
    {
        _targetPoint = v;
    }

    void DetectEnemy()
    {
        _resultsCount = Physics2D.OverlapCircleNonAlloc(transform.position, _detectionRadius, _targetsDetected, _enemyLayer);
    }

    void GetNearestEnemy()
    {
        if(_resultsCount == 0)
        {
            _crosshair.gameObject.SetActive(false);
            _mousePosition = _mainCamera.ScreenToWorldPoint(Input.mousePosition); 
            SetTargetPoint(_mousePosition);
            return;
        }
        _closestEnemyPos = Vector2.zero;

        for(int i = 0; i < _resultsCount; i++)
        {
            if(!_targetsDetected[i].isTrigger) continue;
            
            Vector2 directionToTarget = _targetsDetected[i].bounds.center - transform.position;
            if(_closestEnemyPos == Vector2.zero)
            {
                _closestEnemyPos = directionToTarget;
                continue;
            }
            if(directionToTarget.magnitude < _closestEnemyPos.magnitude)
            {
                _closestEnemyPos = directionToTarget;
            }
        }
        if(_closestEnemyPos == Vector2.zero)
        {
            _crosshair.gameObject.SetActive(false);
            return;
        }
        _crosshair.gameObject.SetActive(true);
        SetTargetPoint(_closestEnemyPos);
    }

    void PointToTarget()
    {
        //_targetDirection.Normalize();
        float angle = Mathf.Atan2(-_targetDirection.y, -_targetDirection.x) * Mathf.Rad2Deg;
        angle = Mathf.LerpAngle(transform.rotation.eulerAngles.z, angle, Time.fixedDeltaTime * _aimSmoothing);
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        _crosshair.position = (Vector3)_targetDirection + transform.position;
        FlipWeapon(_targetDirection.x);
    }

    void FlipWeapon(float xPoint)
    {
        if(_stopAimingTime > 0) return;

        Vector2 scale = transform.localScale;
        if(xPoint < 0) scale.y = 1; 
        else if(xPoint > 0) scale.y = -1; 
        
        transform.localScale = scale;
    }
    void StopAiming()
    {
        _stopAimingTime = _currentWeapon.AtkDuration / 1.15f;
        _crosshair.gameObject.SetActive(false);
    }

    private void OnDestroy() {
        _enemyDetectionTimer.onEnd -= DetectEnemy;
        _currentWeapon.onAttack -= StopAiming;
    }
}
