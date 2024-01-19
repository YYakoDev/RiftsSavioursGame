using System;
using UnityEngine;

public class WeaponAiming : MonoBehaviour
{
    [Header("References")]
    [SerializeField] Camera _mainCamera;
    [SerializeField] Transform _crosshair;
    KeyInput _switchAimKey;
    private Vector3 _mousePosition;
    Vector2 _targetDirection;
    WeaponBase _currentWeapon;

    //enemy detection
    Vector2 _closestEnemyPos;
    Timer _enemyDetectionTimer;
    LayerMask _enemyLayer;
    Collider2D[] _targetsDetected = new Collider2D[15];
    int _resultsCount = 0;

    //Flip
    bool _isFlipped;
    [SerializeField] private float _flipOffset = 2f;
    public bool IsFlipped => _isFlipped;
    private float Sign => (_isFlipped) ? -1 : 1;
    private float MouseOffset => _flipOffset * Sign;


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
        if (_mainCamera == null) _mainCamera = Camera.main;
        _switchAimKey = YYInputManager.GetKey(KeyInputTypes.SwitchAim);
        if(_switchAimKey != null) _switchAimKey.OnKeyPressed += SwitchAim;
        _crosshair.gameObject.SetActive(false);
        _currentWeapon.onAttack += StopAiming;
    }

    // Update is called once per frame
    void Update()
    {
        if (_stopAimingTime > 0)
        {
            _stopAimingTime -= Time.deltaTime;
            return;
        }
        if (_autoAiming) _enemyDetectionTimer.UpdateTime();
    }

    void SwitchAim()
    {
        _autoAiming = !_autoAiming;
        if (_autoAiming)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            _crosshair.gameObject.SetActive(false);
        }
        OnAimingChange?.Invoke(_autoAiming);
    }

private void FixedUpdate()
    {
        if(_autoAiming)
        {
            GetNearestEnemy();
            _targetDirection = _closestEnemyPos;
        }
        else
        {
            _mousePosition = _mainCamera.ScreenToWorldPoint(YYInputManager.MousePosition); 
            _targetDirection = _mousePosition - transform.position;
            SetCameraTargetPoint(_mousePosition);
        }

        //if (_targetDirection.sqrMagnitude > 0.1f)
        PointToTarget(); //maybe is less expensive if we dont calculate the sqr magnitude and just point to target?
     
    }

    private void SetCameraTargetPoint(Vector3 v) => _targetPoint = v;
    

    void DetectEnemy()
    {
        _resultsCount = Physics2D.OverlapCircleNonAlloc(transform.position, _detectionRadius, _targetsDetected, _enemyLayer);
    }

    void GetNearestEnemy()
    {
        if(_resultsCount == 0)
        {
            _crosshair.gameObject.SetActive(false);
            _mousePosition = _mainCamera.ScreenToWorldPoint(YYInputManager.MousePosition); 
            SetCameraTargetPoint(_mousePosition);
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
        _crosshair.gameObject.SetActive(_stopAimingTime <= 0);
        SetCameraTargetPoint(_closestEnemyPos);
    }

    void PointToTarget()
    {
        if(_stopAimingTime > 0) return;
        //_targetDirection.Normalize();
        float angle = Mathf.Atan2(-_targetDirection.y, -_targetDirection.x) * Mathf.Rad2Deg;
        angle = Mathf.LerpAngle(transform.rotation.eulerAngles.z, angle, Time.fixedDeltaTime * _aimSmoothing);
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        _crosshair.position = (Vector3)_targetDirection + transform.position;
        if(_autoAiming) CheckForFlip(_targetDirection.x, MouseOffset / 3f);
        else CheckForFlip(_targetDirection.x, MouseOffset);
    }

 void CheckForFlip(float xPoint, float offset)
    {
        Vector2 scale = transform.localScale;
        
        if(xPoint < offset && _isFlipped)
        {
            scale.y = 1;
            _isFlipped = false;
        } 
        else if(xPoint >= offset && !_isFlipped)
        {
            scale.y = -1;
            _isFlipped = true;
        }
        
        transform.localScale = scale;
    }
    void StopAiming()
    {
        _stopAimingTime = _currentWeapon.AtkDuration / 1.25f;
        _crosshair.gameObject.SetActive(false);
    }

    private void OnDestroy() {
        if(_switchAimKey != null) _switchAimKey.OnKeyPressed -= SwitchAim;
        _enemyDetectionTimer.onEnd -= DetectEnemy;
        _currentWeapon.onAttack -= StopAiming;
    }
}
