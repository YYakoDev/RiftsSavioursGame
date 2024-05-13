using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreMenu : MonoBehaviour
{
    [SerializeField] SOStoreUpgradesList _upgradesList;
    [SerializeField] StoreItem[] _storeItems;
    int _amountToPick = 3;
    // Start is called before the first frame update
    void Start()
    {
        PickUpgrades();
    }

    public void Reroll()
    {
        PickUpgrades();
    }

    void PickUpgrades()
    {
        if(_storeItems == null) return;
        int[] usedIndexes = new int[_amountToPick]; //used indexes should be the chosen upgrades from this batch but also the upgrades the player possess!! so you should be storing the index of that upgrade everytime the player applies one!
        //you can achieve the latter by adding a custom method to the button that sends the index back to here to add to a BoughtUpgrades[] array
        for (int i = 0; i < _amountToPick; i++)
        {
            var item = _storeItems[i];
            //we could create a pool of item prefabs that we can then mark as active or inactive and initialize the store item data. this pool will be stored in the list above?
            var upgrade = (SOStoreUpgrade)ScriptableObject.CreateInstance(typeof(SOStoreUpgrade));
            int usedIndex = UpgradeCreator.GetRandomUpgrade(usedIndexes);
            var randomStoreUpgrade = UpgradeCreator.StoreUpgrades[usedIndex];
            upgrade.Initialize(randomStoreUpgrade.Name, randomStoreUpgrade.Icon, randomStoreUpgrade.Rarity, randomStoreUpgrade.StatsTypes, randomStoreUpgrade.Modifications, randomStoreUpgrade.Costs);
            item.Initialize(upgrade);
            usedIndexes[i] = usedIndex;
        }
    }
}
