using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUpgradesManager : MonoBehaviour
{

    [SerializeField]PlayerManager _playerManager;

    [SerializeField]SOPossibleUpgradesList _possibleUpgradesList;
    SOUpgradeBase[] _equipeedUpgrades;


    //properties
    //public SOUpgradeBase[] PossibleUpgrades => _possibleUpgrades;
    public SOPossibleUpgradesList PossibleUpgradesList => _possibleUpgradesList;
    public SOUpgradeBase[] EquippedUpgrades => _equipeedUpgrades;
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


}
