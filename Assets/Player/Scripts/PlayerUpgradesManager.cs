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
    public SOPlayerStats PlayerStats => _playerManager.Stats;


    private void Awake()
    {
        gameObject.CheckComponent<PlayerManager>(ref _playerManager);
        _possibleUpgradesList.Initialize();
}

    public void SpeedUp(int amount)
    {
        float speedUp = (_playerManager.Stats.Speed * amount)/100;
        _playerManager.Stats.Speed += speedUp;
    }

    public void AddMaterial(CraftingMaterial mat)
    {
        _playerManager.Inventory.AddMaterial(mat);
    }


}
