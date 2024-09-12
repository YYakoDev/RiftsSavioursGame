using System;
using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Random = UnityEngine.Random;
using Debug = UnityEngine.Debug;
using TMPro;
using UnityEngine.InputSystem;
public class StoreMenu : MonoBehaviour
{
    [SerializeField] World _currentWorld;
    [SerializeField] PlayerInput _inputController;
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
    int _ownedCoins = -1, _rerollPrice = 0, _rerollsTries = 0;
    [SerializeField] int _rerollInitialPrice = 10;


    //ui stuff
    bool _menuState = false;
    EventSystem _eventSys;

    //cursor stuff
    bool _previousCursorState;
    CursorLockMode _previousCursorLockMode;

    //audio stuff
    [SerializeField] AudioSource _audio;
    [SerializeField] AudioClip _buySFX, _rerollSFX, _closeSFX;

    private Stopwatch sw2 = new(), sw3 = new();

    UpgradeRarity[] _upgradeRarityLevels;
    Dictionary<UpgradeRarity, IndexStorage> _correspondingIndexesToRarity = new();
    Dictionary<UpgradeRarity, bool> _rarityAvailability = new();

    //input
    [SerializeField] InputActionReference _escapeInput;

    private class IndexStorage
    {
        int[] _array;
        private int _usedIndexesCount;
        public int UsedIndexesCount => _usedIndexesCount;
        public int[] Array => _array;
        public IndexStorage(int[] array, int usedIndexesCount)
        {
            _array = array;
            _usedIndexesCount = usedIndexesCount;
        }
        public bool AddIndexToArray(int index)
        {
            for (int i = 0; i < _array.Length; i++)
            {
                if(_array[i] != -1) continue;
                _array[i] = index;
                this._usedIndexesCount++;
                if(_usedIndexesCount >= _array.Length) return false;
                return true;
            }
            return false;
        }

        public bool RemoveItemFromArray(int index)
        {
            for (int i = 0; i < _array.Length; i++)
            {
                if(_array[i] != index) continue;
                _array[i] = -1;
                this._usedIndexesCount--;
                return true;
            }
            return false;
        }
}

    #region Menu & Object Logic

    private void Awake()
    {
        sw2.Reset();
        sw2.Start();
        _upgradeRarityLevels = Enum.GetValues(typeof(UpgradeRarity)) as UpgradeRarity[];
        foreach (var rarity in _upgradeRarityLevels)
        {
            var array = new int[UpgradeCreator.GetUpgradesCount(rarity)];
            Array.Fill<int>(array, -1);
            IndexStorage indexArray = new(array, 0);
            _correspondingIndexesToRarity.Add(rarity, indexArray);
            _rarityAvailability.Add(rarity, true);
        }
        sw2.Stop();
        //Debug.Log("Populating used indexes array operation time:  " + sw2.ElapsedMilliseconds + " ms");
    }

    // Start is called before the first frame update
    void Start()
    {
        _eventSys = EventSystem.current;
        _escapeInput.action.performed += CloseMenuWithInput;
        //PickUpgrades();
        GameStateManager.OnStateEnd += StateSwitchCheck;
        _playerInventory.onInventoryChange += GetCoins;
        if (_parent.activeInHierarchy) OpenMenu();
    }

    void StateSwitchCheck(GameStateBase state)
    {
        if (state.GetType() == typeof(RestState))
        {
            OpenMenu();
        }
    }

    public void OpenMenu()
    {
        _menuState = true;
        _selectedItem = null;
        _inputController.SwitchCurrentActionMap("UI");
        YYInputManager.StopInput();
        TimeScaleManager.ForceTimeScale(0f);
        PauseMenuManager.DisablePauseBehaviour(true);
        _previousCursorState = Cursor.visible;
        _previousCursorLockMode = Cursor.lockState;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        GetCoins();
        SetRerollTextCost(_rerollPrice);
        _parent.SetActive(true);

        PickUpgrades();
        _rerollsTries = 0;
        SetRerollPrice();
    }

    void CloseMenuWithInput(InputAction.CallbackContext obj)
    {
        CloseMenu();
    }

    public void CloseMenu()
    {
        if (!_animations.IsFinished) return;
        if(!_menuState) return;
        _menuState = false;
        _parent.SetActive(false);
        _inputController.SwitchCurrentActionMap("Gameplay");
        TimeScaleManager.ForceTimeScale(1f);
        PauseMenuManager.DisablePauseBehaviour(false);
        Cursor.visible = _previousCursorState;
        Cursor.lockState = _previousCursorLockMode;
        _audio.PlayWithVaryingPitch(_closeSFX);
    }

    private void OnDestroy()
    {
        _playerInventory.onInventoryChange -= GetCoins;
        GameStateManager.OnStateEnd -= StateSwitchCheck;
        _escapeInput.action.performed -= CloseMenuWithInput;
    }

    #endregion

    void PickUpgrades()
    {
        if (_storeItems == null) return;
        //int[] usedIndexes = new int[_amountToPick]; //used indexes should be the chosen upgrades from this batch but also the upgrades the player possess!! so you should be storing the index of that upgrade everytime the player applies one!
        //you can achieve the latter by adding a custom method to the button that sends the index back to here to add to a BoughtUpgrades[] array
        RemoveIndexesFromStoreItems();
        for (int i = 0; i < _amountToPick; i++)
        {
            var item = _storeItems[i];
            if (item.IsLocked)
            {
                var affordable = CheckIfItsAffordable(_storeItems[i].Cost);
                _storeItems[i].UpdateCostText(affordable, _xpTextEmojiIndex);
                if (affordable && _selectedItem == null) _selectedItem = _storeItems[i].gameObject;
                continue;
            }
            SetStoreItem(i);
        }
        CheckIfThereAreNoUpgradesAffordables();
        _eventSys.SetSelectedGameObject(_selectedItem);
    }

    void RemoveIndexesFromStoreItems()
    {
        for (int i = 0; i < _amountToPick; i++)
        {
            var item = _storeItems[i];
            var index = item.UpgradeIndex;
            if(index != -1 && !item.IsLocked) RemoveUsedIndex(item.Rarity, index);
        }
    }

    void SetStoreItem(int storeItemIndex)
    {
        var item = _storeItems[storeItemIndex];
        //we could create a pool of item prefabs that we can then mark as active or inactive and initialize the store item data. this pool will be stored in the list above?
        var upgrade = (SOStoreUpgrade)ScriptableObject.CreateInstance(typeof(SOStoreUpgrade));
        UpgradeRarity rarity = GetUpgradeRarity();
        if (rarity == (UpgradeRarity)(-1))
        {
            item.gameObject.SetActive(false);
            return;
        }
        //Debug.Log("Setting a store item with the rarity:   " + rarity);
        var usedIndexes = GetUsedIndexes(rarity);
        var upgradeIndex = GetRandomUpgrade(rarity, usedIndexes);
        if (upgradeIndex == -1)
        {
            item.gameObject.SetActive(false);
            return;
        }
        AddNewUsedIndex(rarity, upgradeIndex);
        StoreUpgradeData storeUpgradeData = UpgradeCreator.GetUpgradeFromList(upgradeIndex);
        upgrade.Initialize(storeUpgradeData, upgradeIndex);
        var affordable = CheckIfItsAffordable(upgrade.Costs);
        if(!item.gameObject.activeInHierarchy) item.gameObject.SetActive(true);
        item.Initialize(upgrade, BuyUpgrade, storeItemIndex, upgradeIndex);
        if (affordable && _selectedItem == null) _selectedItem = item.gameObject;
        item.UpdateCostText(affordable, _xpTextEmojiIndex);
    }

    #region Used Indexes

    int[] GetUsedIndexes(UpgradeRarity rarity)
    {
        sw3.Reset();
        sw3.Start();

        int[] totalIndexes = _correspondingIndexesToRarity[rarity].Array;
        var size = _correspondingIndexesToRarity[rarity].UsedIndexesCount;
        //Debug.Log("Requested used indexes of  " + rarity + " of size:  " + size);
        int[] usedIndexes = new int[size];
        int iterator = 0;
        for (int i = 0; i < totalIndexes.Length; i++)
        {
            if(totalIndexes[i] == -1) continue;
            //Debug.Log(totalIndexes[i]);
            usedIndexes[iterator] = totalIndexes[i];

            iterator++;
        }
        sw3.Stop();
        //Debug.Log("Get used indexes operation time:   " + sw3.ElapsedTicks * 100 + " nanoseconds");
        return usedIndexes;
    }

    void AddNewUsedIndex(UpgradeRarity rarity, int index)
    {
        bool result = _correspondingIndexesToRarity[rarity].AddIndexToArray(index);
        if(!result)SetRarityAvailability(rarity, false);
    }
    void RemoveUsedIndex(UpgradeRarity rarity, int usedIndex)
    {
        bool success = _correspondingIndexesToRarity[rarity].RemoveItemFromArray(usedIndex);
        if(success) SetRarityAvailability(rarity, true);

    }

    /*void RecalculateRarityAvailability(UpgradeRarity rarity)
    {
        var usedIndexes = _correspondingIndexesToRarity[rarity].UsedIndexesCount;
        var totalUpgrades = UpgradeCreator.GetUpgradesCount(rarity);
        _rarityAvailability[rarity] = (totalUpgrades > usedIndexes);
    }
    void RecaculcateAllRarityAvailabilities()
    {
        foreach(UpgradeRarity rarity in _upgradeRarityLevels)
        {
            RecalculateRarityAvailability(rarity);
        }
    }*/

    void SetRarityAvailability(UpgradeRarity rarity, bool state)
    {
        _rarityAvailability[rarity] = state;
    }

    #endregion

    #region GetRandomUpgrade

    int GetRandomUpgrade(UpgradeRarity rarity, int[] indexesToSkip)
    {
        sw2.Reset();
        sw2.Start();
        var index = UpgradeCreator.GetRandomUpgradeIndex(rarity, indexesToSkip);
        sw2.Stop();
        //Debug.Log("Elapsed time from get random upgrade operation:   " + sw2.ElapsedTicks * 100 + " nanoseconds");
        return index;
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
        var isAvailable = _rarityAvailability[rarity];
        //Debug.Log($"Is upgrade of rarity {rarity} available?  {isAvailable}");
        if(!isAvailable) return rarity switch
        {
            UpgradeRarity.Broken => UpgradeRarity.Common,
            UpgradeRarity.Common => UpgradeRarity.Uncommon,
            UpgradeRarity.Uncommon => UpgradeRarity.Rare,
            UpgradeRarity.Rare => UpgradeRarity.Epic,
            UpgradeRarity.Epic => UpgradeRarity.Legendary,
            UpgradeRarity.Legendary => (UpgradeRarity)(-1),
            _ => rarity
        };
        else return rarity;
    }
    #endregion

    #region Purchase Stuff

    void BuyUpgrade(SOStoreUpgrade upgrade, int storeItemIndex)
    {
        if(!_animations.IsFinished) return;
        var cost = upgrade.Costs;
        //maybe do a sound here if you cant charge the player that amount of money
        if(!ChargePlayer(cost)) return;
        upgrade.ApplyEffect(_upgradesManager);
        //RecaculcateAllRarityAvailabilities();
        SetStoreItem(storeItemIndex);
        _audio.PlayWithVaryingPitch(_buySFX);
        RecalculateCosts();
    }

    void GetCoins()
    {
        var newCoins = _playerInventory.GetMaterialOwnedCount(_coinItem);
        if(newCoins != _ownedCoins)
        {
            _ownedCoins = newCoins;
            SetBalanceText();
        }
            
    }

    void SetBalanceText() => _balance.text = $"<sprite={_xpTextEmojiIndex}>{_ownedCoins}";

    void RecalculateCosts()
    {
        GameObject objectToSelect = null;
        for (int i = 0; i < _storeItems.Length; i++)
        {
            var affordable = CheckIfItsAffordable(_storeItems[i].Cost);
            _storeItems[i].UpdateCostText(affordable, _xpTextEmojiIndex);
            if(affordable && objectToSelect == null) objectToSelect = _storeItems[i].gameObject;
        }
        CheckIfThereAreNoUpgradesAffordables();
        if(objectToSelect == null) objectToSelect = _selectedItem;
        _eventSys.SetSelectedGameObject(objectToSelect);
    }
    bool CheckIfItsAffordable(int cost) => _ownedCoins >= cost;
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

    #region Reroll Stuff
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
    
    void SetRerollTextCost(int cost) => _rerollCost.text = $"<sprite={_xpTextEmojiIndex+1}> Reroll: <sprite={_xpTextEmojiIndex}>{cost}";

    #endregion

    bool ChargePlayer(int price)
    {
        if(price > _ownedCoins) return false;
        _playerInventory.RemoveMaterial(_coinItem, price);
        return true;
    }

    #endregion

}
