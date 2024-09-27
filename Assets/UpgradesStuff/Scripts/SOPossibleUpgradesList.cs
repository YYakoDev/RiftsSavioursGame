using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Possible Upgrades List" , menuName = "ScriptableObjects/PossibleUpgradesList")]
public class SOPossibleUpgradesList : ScriptableObject
{
    [SerializeField]List<UpgradeGroup> _startingUpgrades;
    List<UpgradeGroup>  _possibleUpgrades; 
    List<UpgradeGroup> _weaponUpgrades; 

    //properties
    public List<UpgradeGroup> StartingUpgrades => _startingUpgrades;
    public List<UpgradeGroup>  PossibleUpgrades => _possibleUpgrades;
    public List<UpgradeGroup> WeaponUpgrades => _weaponUpgrades;

    public void Initialize()
    {
        foreach(var upgrade in _startingUpgrades)
        {
            upgrade.Initialize(this);
        }
        _possibleUpgrades = new(_startingUpgrades);

    }

    public void AddNewGroup(UpgradeGroup newUpgrades)
    {
        newUpgrades.Initialize(this);
        _possibleUpgrades.Add(newUpgrades);
    }
}
