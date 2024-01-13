using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Upgrades/.UpgradeGroup")]
public class UpgradeGroup : ScriptableObject
{
    SOPossibleUpgradesList _upgradesList;
    [SerializeField] SOUpgradeBase[] _upgrades;
    SOUpgradeBase[] _instantiatedUpgrades;
    int _currentIndex = -1;

    //properties
    public int CurrentIndex => _currentIndex;
    public SOUpgradeBase[] Upgrades => _upgrades;

    public void Initialize(SOPossibleUpgradesList upgradesList)
    {
        _currentIndex = 0;
        _upgradesList = upgradesList;
        _instantiatedUpgrades = new SOUpgradeBase[_upgrades.Length];
        for (int i = 0; i < _upgrades.Length; i++)
        {
            _instantiatedUpgrades[i] = Instantiate(_upgrades[i]);
            _instantiatedUpgrades[i].SetGroup(this);
        }
    }

    public SOUpgradeBase GetUpgrade()
    {
        if(_currentIndex >= _instantiatedUpgrades.Length) //if it is greater or equal that means you will get an array out of bounds exception
        {
            _upgradesList.PossibleUpgrades.Remove(this);
            //you have unlocked every upgrade of this group, congrats
            return null; //in the script that handles which upgrades have a chance of showing, if one is null then you would pop them from the list
        }
        return _instantiatedUpgrades[_currentIndex];
    }

    public void AdvanceIndex()
    {
        _currentIndex++;
        if(_currentIndex >= _instantiatedUpgrades.Length) //if it is greater or equal that means you will get an array out of bounds exception
        {
            _upgradesList.PossibleUpgrades.Remove(this);
            //REMOVE FROM LIST!!!!
            //you have unlocked every upgrade of this group, congrats
            //in the script that handles which upgrades have a chance of showing, if one is null then you would pop them from the list
        }
    }
}
