using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Possible Upgrades List" , menuName = "ScriptableObjects/PossibleUpgradesList")]
public class SOPossibleUpgradesList : ScriptableObject
{
    [SerializeField]List<UpgradeGroup> _startingUpgrades;
    UpgradeGroup[] _possibleUpgrades; 

    //properties
    public List<UpgradeGroup> StartingUpgrades => _startingUpgrades;

    public UpgradeGroup[] PossibleUpgrades => _possibleUpgrades;
    
    public void Initialize()
    {
        _possibleUpgrades = _startingUpgrades.ToArray();

    }
}
