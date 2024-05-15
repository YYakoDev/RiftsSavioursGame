using System;
using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Random = UnityEngine.Random;
using Debug = UnityEngine.Debug;
using TMPro;
public class StoreMenu : MonoBehaviour
{
    [SerializeField] World _currentWorld;
    [SerializeField] GameObject _parent;
    [SerializeField] StoreMenuAnimation _animations;
    [SerializeField] PlayerUpgradesManager _upgradesManager;
    [SerializeField] SOPlayerInventory _playerInventory;
    [SerializeField] SOStoreUpgradesList _upgradesList;
    [SerializeField] StoreItem[] _storeItems;
    [SerializeField] GameObject _closeButton;
    GameObject _selectedItem;
    int _amountToPick = 3;
    [SerializeField] int _xpTextEmojiIndex = 0;
    [SerializeField] TextMeshProUGUI _balance, _rerollCost;
    [SerializeField] CraftingMaterial _coinItem;
    int _ownedCoins = -1;
    int[] _usedIndexes = new int[0], _currentUsedIndexes;
    int _rerollPrice = 0;
    [SerializeField] int _rerollInitialPrice = 10;
    int _rerollsTries = 0;
    //
    //ui stuff
    EventSystem _eventSys;
    //cursor stuff
    bool _previousCursorState;
    CursorLockMode _previousCursorLockMode;

    //audio stuff
    [SerializeField]AudioSource _audio;
    [SerializeField] AudioClip _buySFX, _rerollSFX, _closeSFX;

    private Stopwatch sw2 = new();

    private void Awake() {
        _currentUsedIndexes = new int[_amountToPick];
    }

    // Start is called before the first frame update
    void Start()
    {
        _eventSys = EventSystem.current;
        
        //PickUpgrades();
        GetCoins();
        GameStateManager.OnStateEnd += StateSwitchCheck;
        _playerInventory.onInventoryChange += GetCoins;
        if(_parent.activeInHierarchy)OpenMenu();
    }

    void StateSwitchCheck(GameStateBase state)
    {
        if(state.GetType() == typeof(RestState))
        {
            OpenMenu();
        }
    }

    public void OpenMenu()
    {
        _selectedItem = null;
        YYInputManager.StopInput();
        TimeScaleManager.ForceTimeScale(0f);
        PauseMenuManager.DisablePauseBehaviour(true);
        _previousCursorState = Cursor.visible;
        _previousCursorLockMode = Cursor.lockState;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        SetRerollTextCost(_rerollPrice);
        _parent.SetActive(true);
        
        PickUpgrades();
        _rerollsTries = 0;
        SetRerollPrice();
    }

    void GetCoins()
    {
        var newCoins = _playerInventory.GetMaterialOwnedCount(_coinItem);
        if(newCoins != _ownedCoins)
        {
            _ownedCoins = newCoins;
            SetBalance();
        }
            
    }

    void SetRerollTextCost(int cost)
    {
        _rerollCost.text = $"<sprite={_xpTextEmojiIndex+1}> Reroll: <sprite={_xpTextEmojiIndex}>{cost}";
    }
    void SetBalance()
    {
        _balance.text = $"<sprite={_xpTextEmojiIndex}>{_ownedCoins}";
    }

    void RecalculateCosts()
    {
        for (int i = 0; i < _storeItems.Length; i++)
        {
            _storeItems[i].UpdateCostText(CheckIfItsAffordable(_storeItems[i].Cost), _xpTextEmojiIndex);
        }
        CheckIfThereAreNoUpgradesAffordables();
        _eventSys.SetSelectedGameObject(_selectedItem);
    }
    bool CheckIfItsAffordable(int cost)
    {
        return _ownedCoins >= cost;
    }

    void CheckIfThereAreNoUpgradesAffordables()
    {
        for (int i = 0; i < _amountToPick; i++)
        {
            var item = _storeItems[i];
            var affordable = CheckIfItsAffordable(item.Cost);
            if(affordable)
            {
                return;
            }
        }
        _selectedItem = _closeButton;
    }
    void SetRerollPrice()
    {
        var offset = Random.Range(0, 5);
        _rerollPrice = (_rerollInitialPrice) * (_rerollsTries+1);
        SetRerollTextCost(_rerollPrice);
    }

    public void Reroll()
    {
        if(!_animations.IsFinished) return;
        if(!ChargePlayer(_rerollPrice))return;
        //put a price that scales with the amount of rerolls used in this menu opening
        //this means that the amount of rerolls used gets reseted each time this menu opens
        PickUpgrades();
        _rerollsTries++;
        SetRerollPrice();
        _audio.PlayWithVaryingPitch(_rerollSFX);
    }

    void PickUpgrades()
    {
        if(_storeItems == null) return;

        //int[] usedIndexes = new int[_amountToPick]; //used indexes should be the chosen upgrades from this batch but also the upgrades the player possess!! so you should be storing the index of that upgrade everytime the player applies one!
        //you can achieve the latter by adding a custom method to the button that sends the index back to here to add to a BoughtUpgrades[] array

        for (int i = 0; i < _amountToPick; i++)
        {
            var item = _storeItems[i];            
            if(item.IsLocked)
            {
                var affordable = CheckIfItsAffordable(_storeItems[i].Cost);
                _storeItems[i].UpdateCostText(affordable, _xpTextEmojiIndex);
                if(affordable && _selectedItem == null) _selectedItem = _storeItems[i].gameObject;
                continue;
            }
            RemoveUsedIndex(_currentUsedIndexes[i]);
            SetStoreItem(i);
        }
        CheckIfThereAreNoUpgradesAffordables();
        _eventSys.SetSelectedGameObject(_selectedItem);
    }



    void SetStoreItem(int storeItemIndex)
    {
        var item = _storeItems[storeItemIndex];
        //we could create a pool of item prefabs that we can then mark as active or inactive and initialize the store item data. this pool will be stored in the list above?
        var upgrade = (SOStoreUpgrade)ScriptableObject.CreateInstance(typeof(SOStoreUpgrade));
        UpgradeRarity rarity = GetUpgradeRarity();
        var upgradeIndex = GetRandomUpgrade(rarity, _usedIndexes); // do the while loop of getting the random upgrade index until that index no longer matches the ones in usedIndexes!!!! you can copy the and paste the already written method of the upgradecreator class
        //you can get two upgrades repeated if you dont try to get a new one that doesnt match any of the previous used indexes
        StoreUpgradeData storeUpgradeData = UpgradeCreator.GetUpgradeFromList(rarity, upgradeIndex);
        upgrade.Initialize(storeUpgradeData, upgradeIndex);
        var affordable = CheckIfItsAffordable(item.Cost);

        item.Initialize(upgrade, BuyUpgrade, upgrade, storeItemIndex);

        var usedIndexFromArray = AddNewUsedIndex(upgradeIndex);

        if(affordable && _selectedItem == null) _selectedItem = item.gameObject;
        item.UpdateCostText(affordable, _xpTextEmojiIndex);
        _currentUsedIndexes[storeItemIndex] = usedIndexFromArray;
    }
    int GetRandomUpgrade(UpgradeRarity rarity, int[] indexesToSkip)
    {
        sw2.Reset();
        sw2.Start();
        var index = UpgradeCreator.GetRandomUpgradeIndex(rarity, indexesToSkip);
        sw2.Stop();
        Debug.Log("Elapsed time from get random upgrade operation:   " + sw2.ElapsedMilliseconds + " ms");
        return index;


    }

    void BuyUpgrade(SOStoreUpgrade upgrade, int storeItemIndex)
    {
        if(!_animations.IsFinished) return;
        var cost = upgrade.Costs[0].Cost;
        if(!ChargePlayer(cost)) return;
        upgrade.ApplyEffect(_upgradesManager);
        AddNewUsedIndex(upgrade.ListIndex);
        SetStoreItem(storeItemIndex);
        _audio.PlayWithVaryingPitch(_buySFX);
        RecalculateCosts();
    }

    bool ChargePlayer(int price)
    {
        if(price > _ownedCoins) return false;
        _playerInventory.RemoveMaterial(_coinItem, price);
        return true;
    }

    int AddNewUsedIndex(int index)
    {
        var length = _usedIndexes.Length;
        Array.Resize<int>(ref _usedIndexes, length + 1);
        _usedIndexes[length] = index;
        return length;
    }

    void RemoveUsedIndex(int indexFromArray)
    {
        if(indexFromArray < 0 || indexFromArray >= _usedIndexes.Length) return;
        _usedIndexes[indexFromArray] = -1;
    }

    UpgradeRarity GetUpgradeRarity()
    {
        int randomNumber = Random.Range(0, 100);
        UpgradeRarity rarity = randomNumber switch
        {
            <1 => UpgradeRarity.Legendary,
            <10 => UpgradeRarity.Epic,
            <20 => UpgradeRarity.Rare,
            <35 => UpgradeRarity.Uncommon,
            <75 => UpgradeRarity.Common,
            <100 => UpgradeRarity.Broken,
            _ => UpgradeRarity.Common
        };
        return rarity;
    }

    public void CloseMenu()
    {
        if(!_animations.IsFinished) return;
        _parent.SetActive(false);
        YYInputManager.ResumeInput();
        TimeScaleManager.ForceTimeScale(1f);
        PauseMenuManager.DisablePauseBehaviour(false);
        Cursor.visible = _previousCursorState;
        Cursor.lockState = _previousCursorLockMode;
        _audio.PlayWithVaryingPitch(_closeSFX);
    }

    private void OnDestroy() {
        _playerInventory.onInventoryChange -= GetCoins;
        GameStateManager.OnStateEnd -= StateSwitchCheck;
    }
}
