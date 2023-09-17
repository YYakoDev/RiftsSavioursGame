using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Upgrades/.UpgradeGroup")]
public class UpgradeGroup : ScriptableObject
{
    int _currentIndex = 0;
    [SerializeField]int _upgradesAmount = 5;
    [SerializeField]SOUpgradeBase[] _upgrades;

    //properties
    public int CurrentIndex => _currentIndex;


    public SOUpgradeBase GetNextUpgrade()
    {
        if(_upgrades.Length != _upgradesAmount)
        {
            ResizeUpgrades();
        }
        if(_currentIndex >= _upgrades.Length) //if it is greater or equal that means you will get an array out of bounds exception
        {
            //you have unlocked every upgrade of this group, congrats
            return null; //in the script that handles which upgrades have a chance of showing, if one is null then you would pop them from the list
        }
        return _upgrades[_currentIndex];
    }

    void ResizeUpgrades()
    {
        int sizeDifference = _upgrades.Length - _upgradesAmount;
        if(sizeDifference < 0)
        {
            Array.Resize<SOUpgradeBase>(ref _upgrades, _upgradesAmount);
            sizeDifference *= -1;
            for(int i = 0; i < sizeDifference; i++)
            {
                _upgrades[(_upgrades.Length-1) - i] = _upgrades[0];
            }
        }else
        {
            //you could equate the _upgradesAmount to the _upgrades.length here
        }
    }
}
