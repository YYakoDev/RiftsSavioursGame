using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class SavedUpgradesInventory : MonoBehaviour
{
    SOUpgradeBase[] _savedUpgrades;
    [SerializeField] RectTransform _menuParent;
    [SerializeField] Transform _upgradesContainer;
    [SerializeField] UpgradeItemPrefab _upgradePrefab;
    [SerializeField] GameObject _upgradesFromUpgradeMenuContainer;
    SOPlayerInventory _inventory;
    Action<SOUpgradeBase, int> _craftAction;
    UpgradeItemPrefab[] _prefabInstances = new UpgradeItemPrefab[0];
    int _arrayLength = 0;
    EventSystem _eventSys;

    public void Initialize(SOUpgradeBase[] savedUpgrades, SOPlayerInventory inventory, Action<SOUpgradeBase,int> craftAction, int arrayLength)
    {
        _savedUpgrades = savedUpgrades;
        _inventory = inventory;
        _craftAction = craftAction;
        _arrayLength = arrayLength;
    }

    private void Start() {
        _eventSys = EventSystem.current;
        _menuParent.gameObject.SetActive(false);
    }

    public void Open()
    {
        if(_savedUpgrades == null)
        {
            return;
        }
        _upgradesFromUpgradeMenuContainer?.SetActive(false);
        CheckCreatedItems();
        _menuParent.gameObject.SetActive(true);
        _eventSys.SetSelectedGameObject(_prefabInstances[1].gameObject);
    }

    void CheckCreatedItems()
    {
        /*foreach(var equippedUpgrade in _inventory.EquippedUpgrades)
        {
            for (int i = 0; i < _savedUpgrades.Length; i++)
            {
                var savedUpgrade = _savedUpgrades[i];
                if(savedUpgrade == null) continue;
                if (equippedUpgrade == savedUpgrade.GroupParent)
                {
                    _savedUpgrades[i] = null;
                    continue;
                }
            }
        }*/
        if(_prefabInstances == null || _prefabInstances.Length <= 0)
        {
            Array.Resize<UpgradeItemPrefab>(ref _prefabInstances, _arrayLength);
            for (int i = 0; i < _arrayLength; i++)
            {
                _prefabInstances[i] = Instantiate(_upgradePrefab);
                _prefabInstances[i].transform.SetParent(_upgradesContainer);
                _prefabInstances[i].gameObject.SetActive(false);
            }
        }
        for (int i = 0; i < _arrayLength; i++)
        {
            var item = _prefabInstances[i];
            if(i >= _savedUpgrades.Length)
            {
                _prefabInstances[i].gameObject.SetActive(false);
                continue;
            }
            var upgrade = _savedUpgrades[i];
            if(upgrade == null)
            {
                _prefabInstances[i].gameObject.SetActive(false);
                continue;
            }
            item.Initialize(_inventory, upgrade, CraftSavedUpgrade, i);
            item.transform.localScale = Vector3.one;
            item.gameObject.SetActive(true);
        }
    }

    public void CraftSavedUpgrade(SOUpgradeBase upgrade, int index)
    {
        _savedUpgrades[index] = null;
        _prefabInstances[index].gameObject.SetActive(false);
        _craftAction.Invoke(upgrade, index);
        Close();
    }

    public void Close()
    {
        _menuParent.gameObject.SetActive(false);
        _upgradesFromUpgradeMenuContainer?.SetActive(true);
    }
}
