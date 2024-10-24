using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerAnimationController))]
public class PlayerManager : MonoBehaviour
{
    static SOCharacterData SelectedCharacter;
    public static event Action onCharacterChange;
    //References
    [Header("References")]
    [SerializeField]SOCharacterData _charData;
    [SerializeField]SOPlayerStats _stats;
    [SerializeField]PlayerAnimationController _animatorController;
    [SerializeField]Rigidbody2D _rigidBody;
    [SerializeField]SpriteRenderer _renderer;
    [SerializeField]SOPlayerInventory _inventory;
    //[SerializeField]PlayerLevelManager _levelManager;
    [SerializeField]PlayerMovement _movementScript;
    [SerializeField]PlayerUpgradesManager _upgradesManager;
    [SerializeField]PlayerStatsManager _statsManager;
    [SerializeField] WeaponManager _weaponManager;
    bool _gettedComponents = false;

    public SOPlayerStats Stats => _stats;
    public PlayerAnimationController AnimController => _animatorController;
    public Rigidbody2D RigidBody => _rigidBody;
    public SpriteRenderer Renderer => _renderer;
    public SOPlayerInventory Inventory => _inventory;
    public WeaponManager WeaponManager => _weaponManager;
    //public PlayerLevelManager LevelManager => _levelManager;
    public PlayerMovement MovementScript => _movementScript;
    public PlayerStatsManager StatsManager => _statsManager;
    public SOCharacterData CharacterData => _charData;

    public Vector3 Position => transform.position;
    public static SOCharacterData SelectedChara => SelectedCharacter;

    private void Awake() {
        onCharacterChange += InitializeCharacterData;
    }

    private void Start() {
        GetComponents();
        InitializeCharacterData();
    }

    void GetComponents()
    {
        if(_gettedComponents) return;
        GameObject thisGO = gameObject;
        thisGO.CheckComponent<PlayerAnimationController>(ref _animatorController);
        thisGO.CheckComponent<Rigidbody2D>(ref _rigidBody);
        thisGO.CheckComponent<SpriteRenderer>(ref _renderer);
        //thisGO.CheckComponent<PlayerLevelManager>(ref _levelManager);
        thisGO.CheckComponent<PlayerMovement>(ref _movementScript);
        thisGO.CheckComponent<PlayerStatsManager>(ref _statsManager);
        _gettedComponents = true;
    }
    void InitializeCharacterData()
    {
        if(SelectedCharacter != null) _charData = Instantiate(SelectedCharacter);
        if(_stats != null) _stats.Initialize(_charData);
        //_levelManager?.SetPlayerStats(_stats);
        _animatorController.ChangeAnimator(_charData.Animator);
        _inventory?.Initialize(_upgradesManager, _charData.StartingInventory);
    }


    public static void ChangeSelectedCharacter(SOCharacterData chara)
    {
        SelectedCharacter = chara;
        onCharacterChange?.Invoke();
    }

    private void OnDestroy() {
        onCharacterChange -= InitializeCharacterData;
    }
}