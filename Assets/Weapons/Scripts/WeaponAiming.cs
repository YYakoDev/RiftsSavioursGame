using System;
using UnityEngine;

public class WeaponAiming : MonoBehaviour
{
    [Header("References")]
    [SerializeField] Camera _mainCamera;
    [SerializeField] Transform _crosshair;
    [SerializeField] PlayerMovement _playerMovement;
    KeyInput _switchAimKey;
    private Vector3 _mousePosition;
    Vector2 _targetDirection;
    WeaponBase _currentWeapon;

    //enemy detection
    Vector2 _closestEnemyPosition;
    Timer _enemyDetectionTimer;
    LayerMask _enemyLayer;
    Collider2D[] _targetsDetected = new Collider2D[15];
    int _resultsCount = 0;

    //Flip
    FlipLogic _flipLogic;
    [SerializeField] private float _flipOffset = 2f;
    //public bool IsFlipped => _isFlipped;
    private float Sign => (_flipLogic.IsFlipped) ? -1 : 1;
    public float MouseFlipOffset => _flipOffset * Sign;


    [Header("Values")]
    [SerializeField] float _aimSmoothing = 8f;
    float _detectionRadius = 3.5f;
    float _stopAimingTime = 0f;
    bool _autoAiming = false;
    public event Action<bool> OnAimingChange;


    // properties
    Vector2 _targetPoint;
    public Vector2 TargetPoint => _targetPoint;
    public int EnemyResultsCount => _resultsCount;
    public float AimSmoothing{ get => _aimSmoothing; set => _aimSmoothing = value; }

    private void Awake()
    {
        _enemyDetectionTimer = new(0.1f, true);
        _enemyDetectionTimer.onEnd += DetectEnemy;
        _flipLogic = new(transform, false, true, 0.12f);
    }

    public void Initialize(LayerMask layer)
    {
        _enemyLayer = layer;
    }

    void Start()
    {
        if (_mainCamera == null) _mainCamera = Camera.main;
        _switchAimKey = YYInputManager.GetKey(KeyInputTypes.SwitchAim);
        _switchAimKey.OnKeyPressed += SwitchAim;
        _crosshair.gameObject.SetActive(false);
        if(_currentWeapon != null) _currentWeapon.onAttack += StopAiming;
    }

    // Update is called once per frame
    void Update()
    {
        _flipLogic.UpdateLogic();
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
            _targetDirection = _closestEnemyPosition;
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
            //_mousePosition = _mainCamera.ScreenToWorldPoint(YYInputManager.MousePosition); 
            _closestEnemyPosition = _currentWeapon.PrefabTransform.position;
            SetCameraTargetPoint(transform.position);
            return;
        }
        _closestEnemyPosition = Vector2.zero;
        float distance = 51f;
        int closestEnemyIndex = 0;
        for(int i = 0; i < _resultsCount; i++)
        {
            if(!_targetsDetected[i].isTrigger || !_targetsDetected[i].gameObject.activeInHierarchy) continue;
            var distanceToEnemy = Vector3.Distance(_targetsDetected[i].transform.position, transform.position);
            if(distanceToEnemy < distance)
            {
                distance = distanceToEnemy;
                closestEnemyIndex = i;
            }
        }
        if(distance >= 50f)
        {
            _crosshair.gameObject.SetActive(false);
            return;
        }
        Vector2 directionToEnemy = _targetsDetected[closestEnemyIndex].transform.position - transform.position;
        _closestEnemyPosition = directionToEnemy;
        _crosshair.gameObject.SetActive(_stopAimingTime <= 0);
        SetCameraTargetPoint(_targetsDetected[closestEnemyIndex].transform.position);
    }

    void PointToTarget()
    {
        if(_stopAimingTime > 0) return;
        //_targetDirection.Normalize();
        float angle = Mathf.Atan2(-_targetDirection.y, -_targetDirection.x) * Mathf.Rad2Deg;
        angle = Mathf.LerpAngle(transform.rotation.eulerAngles.z, angle, Time.fixedDeltaTime * _aimSmoothing);
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        _crosshair.position = (Vector3)_targetDirection + transform.position;
        //if(_autoAiming) _flipLogic.FlipCheck(_targetDirection.x + (MouseOffset / 3f));
        //else _flipLogic.FlipCheck(_targetDirection.x + MouseOffset);
        var offset = (_autoAiming) ? MouseFlipOffset / 3f : MouseFlipOffset;
        if(_autoAiming && _resultsCount == 0)_playerMovement.FlipLogic?.FlipCheck(_playerMovement.Movement.x);
        else _playerMovement.FlipLogic?.FlipCheck(_targetDirection.x + offset); //this is the real one, the line bellow is for testing
        //else _playerMovement.FlipLogic?.FlipCheck(-_targetDirection.x);
        //_playerMovement.FlipLogic?.FlipCheck(_playerMovement.Movement.x);
        _flipLogic.FlipCheck(_targetDirection.x + offset);

    }

    public void LockFlip(float time)
    {
        _flipLogic.LockFlip(time);
        _playerMovement.FlipLogic.LockFlip(time);
    }


    void StopAiming()
    {
        _stopAimingTime = _currentWeapon.AtkDuration / 2f;
        _crosshair.gameObject.SetActive(false);
    }

    public void SwitchCurrentWeapon(WeaponBase weapon)
    {
        if(_currentWeapon != null) _currentWeapon.onAttack -= StopAiming;
        _currentWeapon = weapon;
        _currentWeapon.onAttack += StopAiming;
    }

    private void OnDestroy() {
        if(_switchAimKey != null) _switchAimKey.OnKeyPressed -= SwitchAim;
        _enemyDetectionTimer.onEnd -= DetectEnemy;
        _currentWeapon.onAttack -= StopAiming;
    }
}
