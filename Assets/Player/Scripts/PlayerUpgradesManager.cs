using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUpgradesManager : MonoBehaviour
{

    [SerializeField]PlayerManager _playerManager;

    [SerializeField]SOPossibleUpgradesList _possibleUpgradesList;
    SOUpgradeBase[] _equipedUpgrades;


    //properties
    //public SOUpgradeBase[] PossibleUpgrades => _possibleUpgrades;
    public SOPossibleUpgradesList PossibleUpgradesList => _possibleUpgradesList;
    public SOUpgradeBase[] EquippedUpgrades => _equipedUpgrades;
    public SOPlayerStats Stats => _playerManager.Stats;


    private void Awake()
    {
        gameObject.CheckComponent<PlayerManager>(ref _playerManager);
        _possibleUpgradesList.Initialize();
    }

    public float StatUp(float stat, float amount) => stat += amount;
    public float StatUp(float stat, int percent) => stat += GetPercentage(stat, percent);
    //public int StatUp(int stat, int amount) => stat += amount;
    //public int StatUpByPercent(int stat, int percent) => stat += (int)GetPercentage(stat, percent);

    public void AddMaterial(CraftingMaterial mat)
    {
        _playerManager.Inventory.AddMaterial(mat);
    }

    float GetPercentage(float stat, int percent) => stat * (percent / 100f);



}
