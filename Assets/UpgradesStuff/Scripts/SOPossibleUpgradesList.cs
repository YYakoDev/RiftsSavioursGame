using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Possible Upgrades List" , menuName = "ScriptableObjects/PossibleUpgradesList")]
public class SOPossibleUpgradesList : ScriptableObject
{
    [SerializeField]List<UpgradeGroup> _startingUpgrades;
    List<UpgradeGroup>  _possibleUpgrades; 

    //properties
    public List<UpgradeGroup> StartingUpgrades => _startingUpgrades;
    public List<UpgradeGroup>  PossibleUpgrades => _possibleUpgrades;
    
    public void Initialize()
    {
        foreach(var upgrade in _startingUpgrades)
        {
            upgrade.Initialize(this);
        }
        _possibleUpgrades = new(_startingUpgrades);

    }
}
