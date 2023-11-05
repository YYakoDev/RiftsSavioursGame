using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerAnimationController))]
public class PlayerManager : MonoBehaviour
{
    //References
    [Header("References")]
    [SerializeField]SOPlayerStats _stats;
    [SerializeField]PlayerAnimationController _animatorController;
    [SerializeField]Rigidbody2D _rigidBody;
    [SerializeField]SpriteRenderer _renderer;
    [SerializeField]SOPlayerInventory _inventory;
    [SerializeField]PlayerLevelManager _levelManager;
    [SerializeField]PlayerMovement _movementScript;
    [SerializeField]CameraTarget _cameraTarget;

    public SOPlayerStats Stats => _stats;
    public PlayerAnimationController AnimController => _animatorController;
    public Rigidbody2D RigidBody => _rigidBody;
    public SpriteRenderer Renderer => _renderer;
    public SOPlayerInventory Inventory => _inventory;
    public PlayerLevelManager LevelManager => _levelManager;
    public PlayerMovement MovementScript => _movementScript;
    public CameraTarget CameraTarget => _cameraTarget;


    public Vector3 Position => transform.position;

    // Start is called before the first frame update
    void Awake()
    {
        GameObject thisGO = gameObject;
        thisGO.CheckComponent<PlayerAnimationController>(ref _animatorController);
        thisGO.CheckComponent<Rigidbody2D>(ref _rigidBody);
        thisGO.CheckComponent<SpriteRenderer>(ref _renderer);
        thisGO.CheckComponent<PlayerLevelManager>(ref _levelManager);
        _levelManager.SetPlayerStats(_stats);
        thisGO.CheckComponent<PlayerMovement>(ref _movementScript);
        thisGO.CheckComponent<CameraTarget>(ref _cameraTarget);
        _inventory.Initialize();
        
    
    }
    void Start()
    {

    }
}
