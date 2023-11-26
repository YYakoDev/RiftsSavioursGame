using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    Animator _animator;
    [SerializeField]World _currentWorld;
    [SerializeField]SOPlayerInventory _playerInventory;
    [SerializeField]UICraftingMaterialPrefab _craftingMaterialPrefab;
    UICraftingMaterialPrefab[] _instantiatedMaterials;

    const float InventoryVisibleTime = 2f;
    float _countdown = 1f;

    private void Awake() {
        _animator = GetComponent<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {
        _playerInventory.onInventoryChange += SetMaterialAmount;
        WorldManager.onWorldChange += WorldChange;    
        InitializeUI();
    }

    void InitializeUI()
    {
        SetCraftingMaterialLength();
        ShowCraftingMaterials();
        SetMaterialAmount();
    }

    private void Update() {
        if(_countdown >= 0)
        {
            _countdown -= Time.deltaTime;
        }else
        {
            CloseUI();
        }
    }



    void SetCraftingMaterialLength()
    {
        Array.Resize<UICraftingMaterialPrefab>(ref _instantiatedMaterials, _currentWorld.CurrentCraftingMaterials.Length);
    }

    void ShowCraftingMaterials()
    {
        if(_instantiatedMaterials != null) DestroyPreviousItems(); // this may cause problems when loading and saving?
        for(int i = 0; i < _currentWorld.CurrentCraftingMaterials.Length; i++)
        {
            UICraftingMaterialPrefab instantiatedMaterial = Instantiate(_craftingMaterialPrefab, transform);
            _instantiatedMaterials[i] = instantiatedMaterial;

            /* 
            GameObject instantiatedMaterial = Instantiate(_craftingMaterialPrefab.gameObject, transform.position, Quaternion.identity);
            _instantiatedMaterials[i] = instantiatedMaterial.GetComponent<UICraftingMaterialPrefab>();
            */
        }
    }

    void SetMaterialAmount()
    {
        for(int i = 0; i < _instantiatedMaterials.Length; i++)
        {
            CraftingMaterial craftingMaterial = _currentWorld.CurrentCraftingMaterials[i];
            UICraftingMaterialPrefab instantiatedMaterial = _instantiatedMaterials[i];

            int materialsOwned = (_playerInventory.OwnedMaterials.ContainsKey(craftingMaterial)) ? _playerInventory.OwnedMaterials[craftingMaterial] : 0;
            instantiatedMaterial.Initialize(craftingMaterial, materialsOwned);
        }
        OpenUI();
    }

    void OpenUI()
    {
        _animator.enabled = true;
        _animator.Play("OpenInventory");
        _countdown = InventoryVisibleTime;
        
    }
    void CloseUI()
    {
        _animator.Play("CloseInventory");
    }

    public void DisableAnimator() // this one is being called by an animation event
    {
        _animator.enabled = false;
    }

    void WorldChange(World world)
    {
        _currentWorld = world;
        InitializeUI();
    }

    void DestroyPreviousItems()
    {
        foreach(var item in _instantiatedMaterials)
        {
            if(item == null) continue;
            Destroy(item.gameObject);
        }
    }

    private void OnDestroy()
    {
        WorldManager.onWorldChange -= WorldChange;    
        _playerInventory.onInventoryChange -= SetMaterialAmount;
    }

    
}
