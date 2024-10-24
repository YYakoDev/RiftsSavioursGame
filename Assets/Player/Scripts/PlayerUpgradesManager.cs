using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUpgradesManager : MonoBehaviour
{

    [SerializeField]PlayerManager _playerManager;
    [SerializeField]SOPossibleUpgradesList _possibleUpgradesList;
    public PlayerManager Player => _playerManager;
    
    //SOUpgradeBase[] _equipedUpgrades;

    //properties
    //public SOUpgradeBase[] PossibleUpgrades => _possibleUpgrades;
    //public SOPossibleUpgradesList PossibleUpgradesList => _possibleUpgradesList;
    //public SOUpgradeBase[] EquippedUpgrades => _equipedUpgrades;
    //public SOPlayerStats Stats => _playerManager.Stats;


    private void Awake()
    {
        gameObject.CheckComponent<PlayerManager>(ref _playerManager);
        _possibleUpgradesList.Initialize();
    }

    public float StatUp(float amount) => amount;
    public float StatUp(float stat, int percent) => GetPercentage(stat, percent);
    //public int StatUp(int stat, int amount) => stat += amount;
    //public int StatUpByPercent(int stat, int percent) => stat += (int)GetPercentage(stat, percent);

    public void AddMaterial(CraftingMaterial mat)
    {
        _playerManager.Inventory.AddMaterial(mat);
    }
    public void AddMaterial(CraftingMaterial mat, int amount)
    {
        _playerManager.Inventory.AddMaterial(mat, amount);
    }

    float GetPercentage(float stat, int percent) => stat * (percent / 100f);



}
