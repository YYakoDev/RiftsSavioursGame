using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUpgradesManager : MonoBehaviour
{

    PlayerManager _playerManager;

    [SerializeField]SOPossibleUpgradesList _possibleUpgradesList;
    SOUpgradeBase[] _equipeedUpgrades;


    //properties
    //public SOUpgradeBase[] PossibleUpgrades => _possibleUpgrades;
    public SOPossibleUpgradesList PossibleUpgradesList => _possibleUpgradesList;
    public SOUpgradeBase[] EquippedUpgrades => _equipeedUpgrades;


    private void Awake() {
        gameObject.CheckComponent<PlayerManager>(ref _playerManager);
    }

    public void SpeedUp(int amount)
    {
        float speedUp = (_playerManager.Stats.Speed * amount)/100;
        _playerManager.Stats.Speed += speedUp;
    }


}
