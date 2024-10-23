using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponAiming : MonoBehaviour
{
    [Header("References")]
    [SerializeField] Camera _mainCamera;
    [SerializeField] CursorSetter _cursorSetter;
    [SerializeField] Transform _crosshair;
    [SerializeField] PlayerMovement _playerMovement;
    [SerializeField] SOPlayerAttackStats _attackStats;
    [SerializeField]InputActionReference _switchAimKey, _pointerPosition;
    [SerializeField] Sprite _uiAimingIcon;
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
    float _detectionRadius = 4f;
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
        _switchAimKey.action.performed += SwitchAim;
        _crosshair.gameObject.SetActive(false);
        if(_currentWeapon != null) _currentWeapon.WeaponEvents.OnAttack += StopAiming;
        _attackStats.onStatsChange += IncreaseDetectionRadius;
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

    void SwitchAim(InputAction.CallbackContext obj)
    {
        _autoAiming = !_autoAiming;
        if (_autoAiming)
        {
            NotificationSystem.SendNotification(NotificationType.Left, "AutoAiming enabled", _uiAimingIcon, 0.7f, 0.5f, 0.2f);
            _cursorSetter.HideCursor();
        }
        else
        {
            NotificationSystem.SendNotification(NotificationType.Left, "AutoAiming Disabled", _uiAimingIcon, 0.8f, 0.4f, 0.2f);
            _cursorSetter.ShowCursor();
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
            var mouseInput = _pointerPosition.action.ReadValue<Vector2>();
            _mousePosition = YYInputManager.i.GetMousePosition();
            if(mouseInput != Vector2.zero)
            {
                _targetDirection = _mousePosition - transform.position;
                SetCameraTargetPoint(_mousePosition);
            }else
            {
                _targetDirection = Vector2.right * _playerMovement.FacingDirection;
            }

        }

        //if (_targetDirection.sqrMagnitude > 0.1f)
        PointToTarget(); //maybe is less expensive if we dont calculate the sqr magnitude and just point to target?
     
    }

    private void SetCameraTargetPoint(Vector3 v)
    {
        _targetPoint = v;
    }
    private void IncreaseDetectionRadius() => _detectionRadius = 4.5f + _attackStats.AttackRange;

    void DetectEnemy()
    {
        _resultsCount = Physics2D.OverlapCircleNonAlloc(transform.position, _detectionRadius, _targetsDetected, _enemyLayer);
    }

    void GetNearestEnemy()
    {
        var currentPos = transform.position;
        if(_resultsCount == 0)
        {
            _crosshair.gameObject.SetActive(false);
            //_mousePosition = _mainCamera.ScreenToWorldPoint(YYInputManager.MousePosition); 
            _closestEnemyPosition = _playerMovement.transform.position;
            SetCameraTargetPoint(currentPos);
            return;
        }
        _closestEnemyPosition = Vector2.zero;
        float distance = 51f;
        int closestEnemyIndex = 0;
        for(int i = 0; i < _resultsCount; i++)
        {
            if(!_targetsDetected[i].isTrigger || !_targetsDetected[i].gameObject.activeInHierarchy) continue;
            var distanceToEnemy = Vector3.Distance(_targetsDetected[i].ClosestPoint(currentPos), currentPos);
            //Debug.Log("Distance:   " + distanceToEnemy);
            if(distanceToEnemy < (distance - 1.21f)) // this 1.21f is an arbitrary threshold the next closest enemy has to surpass in order for the target switch to occur
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
        //var offset = (_autoAiming) ? MouseFlipOffset / 3f : MouseFlipOffset;
        if(_autoAiming && _resultsCount == 0)_playerMovement.FlipLogic?.FlipCheck(_playerMovement.LastMovement.x);
        else _playerMovement.FlipLogic?.FlipCheck(_targetDirection.x + MouseFlipOffset);
        _flipLogic.FlipCheck(_targetDirection.x + MouseFlipOffset);

    }

    public void LockFlip(float time)
    {
        _flipLogic.LockFlip(time);
        _playerMovement.FlipLogic.LockFlip(time);
    }


    void StopAiming()
    {
        var mouseInput = _pointerPosition.action.ReadValue<Vector2>();
        if(mouseInput == Vector2.zero && (!_autoAiming || _resultsCount == 0))
        {
            var angle = Mathf.Atan2(-_playerMovement.LastMovement.y, -_playerMovement.LastMovement.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
        _stopAimingTime = _currentWeapon.AtkDuration / 2.3f;
        _crosshair.gameObject.SetActive(false);
    }

    public void SwitchCurrentWeapon(WeaponBase weapon)
    {
        if(_currentWeapon != null) _currentWeapon.WeaponEvents.OnAttack -= StopAiming;
        _currentWeapon = weapon;
        _currentWeapon.WeaponEvents.OnAttack += StopAiming;
    }

    private void OnDestroy() {
        _switchAimKey.action.performed -= SwitchAim;
        _enemyDetectionTimer.onEnd -= DetectEnemy;
        if(_currentWeapon != null)_currentWeapon.WeaponEvents.OnAttack -= StopAiming;
        _attackStats.onStatsChange -= IncreaseDetectionRadius;
    }
}
