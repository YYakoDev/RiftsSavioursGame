using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Store Upgrades List" , menuName = "ScriptableObjects/StoreUpgradesList")]
public class SOStoreUpgradesList : ScriptableObject
{
    [SerializeField]List<SOUpgradeBase> _startingUpgrades;
    List<SOUpgradeBase> _upgrades; 

    //properties
    //public List<SOUpgradeBase> StartingUpgrades => _startingUpgrades;
    public List<SOUpgradeBase> Upgrades => _upgrades;

    public void Initialize()
    {
        _upgrades = new(_startingUpgrades);
        //you can ask here for the player manager from the upgradesmanager so you can read the player stats and maybe reduce cost or reduce/increase the rarities of the upgrades

    }


    public void AddUpgrade(SOUpgradeBase upgrade)
    {
        _upgrades.Add(upgrade);
    }
    public void RemoveUpgrade(int index)
    {
        _upgrades[index] = null;
    }
}
