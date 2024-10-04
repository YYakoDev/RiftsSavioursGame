using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BlacksmithMenu : MonoBehaviour
{
    [Header("References")]
    [SerializeField] MenuController _menuController;
    [SerializeField] GameObject _menuParent;
    [SerializeField] InputActionReference _escapeInput;
    
    [Header("Blacksmith Upgrades")]
    [SerializeField] UIBlacksmithUpgrade _itemPrefab;
    [SerializeField] RectTransform _upgradesParent;
    [SerializeField] SOPossibleUpgradesList _upgradesList;
    [SerializeField] UIWeaponSelectionItem[] _equippedWeaponsUI;
    int _menuItemsCount = 3;
    UIBlacksmithUpgrade[] _upgrades = null;
    WeaponBase _currentSelectedWeapon;

    private void Start() {
        _upgrades = null;
        SetUpgrades();
    }

    public void OpenMenu()
    {
        SetUpgrades();
    }


    void SetUpgrades()
    {
        if(_upgrades == null) CreateUpgrades();
        for (int i = 0; i < _upgrades.Length; i++)
        {
            var upgrade = GetUpgrade();
            var upgradeItem = _upgrades[i];
            upgradeItem.Init(upgrade, BuyUpgrade);
        }
    }

    SOUpgradeBase GetUpgrade() //here you will pass the weapon base to determine wheter an upgrade is valid for that weapon or not.
    {
        SOUpgradeBase upgrade = _upgradesList.PossibleUpgrades[Random.Range(0, _upgradesList.PossibleUpgrades.Count)].GetUpgrade();

        return upgrade;
    }

    void BuyUpgrade(SOUpgradeBase upgrade)
    {
        Debug.Log("Buying upgrade:   " + upgrade.Name);
    }

    void CreateUpgrades()
    {
        _upgrades = new UIBlacksmithUpgrade[_menuItemsCount];
        for (int i = 0; i < _upgrades.Length; i++)
        {
            _upgrades[i] = Instantiate(_itemPrefab, _upgradesParent);
            _upgrades[i].Init(null, null);
        }
    }

    void CloseMenuWithInput(InputAction.CallbackContext obj) => CloseMenu();
    public void CloseMenu()
    {
        
    }
}
