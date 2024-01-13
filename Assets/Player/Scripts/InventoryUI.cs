using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    Animator _animator;
    Transform _cachedTransform;
    [SerializeField]SOPlayerInventory _playerInventory;
    [SerializeField]UICraftingMaterialPrefab _craftingMaterialPrefab;
    UICraftingMaterialPrefab[] _instantiatedMaterials;

    const float InventoryVisibleTime = 2f;
    float _countdown = 1f;

    private void Awake() {
        _animator = GetComponent<Animator>();
        _cachedTransform = transform;
    }

    // Start is called before the first frame update
    void Start()
    {
        _playerInventory.onInventoryChange += SetInventory;
        SetInventory();
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
    void SetInventory()
    {
        int ownMatCount = _playerInventory.OwnedMaterials.Count;
        if(_instantiatedMaterials == null)
        {
            CreateUIItem(ownMatCount);
        }
        else if(_instantiatedMaterials.Length < ownMatCount)
        {
            int diff = ownMatCount - _instantiatedMaterials.Length;
            CreateUIItem(diff, _instantiatedMaterials.Length);
        }
        SetMaterialItems();
    }
    void SetMaterialItems()
    {
        int index = 0;
        foreach(CraftingMaterial mat in _playerInventory.OwnedMaterials.Keys)
        {
            UICraftingMaterialPrefab instantiatedMaterial = _instantiatedMaterials[index];
            int matCount = _playerInventory.OwnedMaterials[mat];
            instantiatedMaterial.Initialize(mat, matCount);
            index++;
            instantiatedMaterial.gameObject.SetActive((matCount > 0)); // maybe pass this responsability to the initialize method of the material itself but idk
        }
        /*for(int i = 0; i < _instantiatedMaterials.Length; i++)
        {
            CraftingMaterial craftingMaterial = _playerInventory.OwnedMaterials.Keys.CopyTo();
            UICraftingMaterialPrefab instantiatedMaterial = _instantiatedMaterials[i];

            int materialsOwned = (_playerInventory.OwnedMaterials.ContainsKey(craftingMaterial)) ? _playerInventory.OwnedMaterials[craftingMaterial] : 0;
            instantiatedMaterial.Initialize(craftingMaterial, materialsOwned);
        }*/
        OpenUI();
    }
    void CreateUIItem(int amountToCreate, int startingIndex = 0)
    {
        if(_instantiatedMaterials != null)
        {
            int newLength = _instantiatedMaterials.Length + amountToCreate;
            Array.Resize<UICraftingMaterialPrefab>(ref _instantiatedMaterials, newLength);
        }else
        {
            _instantiatedMaterials = new UICraftingMaterialPrefab[amountToCreate];
        }
        for (int i = 0; i < amountToCreate; i++)
        {
            UICraftingMaterialPrefab instantiatedMaterial = Instantiate(_craftingMaterialPrefab, _cachedTransform);
            _instantiatedMaterials[startingIndex + i] = instantiatedMaterial;
        }
    }

    void OpenUI()
    {
        if(_instantiatedMaterials == null || _instantiatedMaterials.Length == 0) return;
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

    private void OnDestroy()
    {
        _playerInventory.onInventoryChange -= SetInventory;
    }

    
}
