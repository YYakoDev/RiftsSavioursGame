using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PrototypeLevelUpSystem : MonoBehaviour
{
    [SerializeField]SOPlayerInventory _inventory;
    [SerializeField] CraftingMaterial _xpMat;
    [SerializeField] StoreMenu _storeMenu;
    [SerializeField] InputActionReference _levelUpHotkey;
    int _xpAmount = 0, _currentLevel = 1;
    float _levelUpThreshold;
    public event Action OnXPChange, OnLevelUp;
    public int XPAmount => _xpAmount;
    public float LevelUpThreshold => _levelUpThreshold;

    private void Awake() {
        _currentLevel = 1;
        SetLevelUpThreshold();
        _xpAmount = 0;
    }

    private void Start() {
        CheckXPAmount();
        _inventory.onInventoryChange += CheckXPAmount;
        _levelUpHotkey.action.performed += LevelUp;
    }

    void CheckXPAmount()
    {
        var currentCount = _inventory.GetMaterialOwnedCount(_xpMat);
        if(_xpAmount != currentCount)
        {
            _xpAmount = currentCount;
            OnXPChange?.Invoke();
        }
    }

    void LevelUp(InputAction.CallbackContext obj)
    {
        if(_xpAmount < Mathf.RoundToInt(_levelUpThreshold)) return;
        _currentLevel++;
        _storeMenu.OpenMenu();
        SetLevelUpThreshold();
        OnLevelUp?.Invoke();

    }

    void SetLevelUpThreshold()
    {
        _levelUpThreshold = 50f * _currentLevel;
    }

    private void OnDestroy() {
        _inventory.onInventoryChange -= CheckXPAmount;
        _levelUpHotkey.action.performed -= LevelUp;
    }
}
