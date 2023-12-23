using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Upgrades/.UpgradeGroup")]
public class UpgradeGroup : ScriptableObject
{
    int _currentIndex = -1;
    [SerializeField] SOUpgradeBase[] _upgrades;

    //properties
    public int CurrentIndex => _currentIndex;

    public void Initialize()
    {
        _currentIndex = 0;
        foreach (var upgrade in _upgrades)
        {
            upgrade.SetGroup(this);
        }
    }

    public SOUpgradeBase GetUpgrade()
    {
        if(_currentIndex >= _upgrades.Length) //if it is greater or equal that means you will get an array out of bounds exception
        {
            //you have unlocked every upgrade of this group, congrats
            return null; //in the script that handles which upgrades have a chance of showing, if one is null then you would pop them from the list
        }
        return _upgrades[_currentIndex];
    }

    public void AdvanceIndex()
    {
        _currentIndex++;
        if(_currentIndex >= _upgrades.Length) //if it is greater or equal that means you will get an array out of bounds exception
        {
            //REMOVE FROM LIST!!!!
            //you have unlocked every upgrade of this group, congrats
            //in the script that handles which upgrades have a chance of showing, if one is null then you would pop them from the list
        }
    }
}
