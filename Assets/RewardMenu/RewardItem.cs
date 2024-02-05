using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class RewardItem : MonoBehaviour, ISelectHandler
{
    [SerializeField] Image _icon;
    [SerializeField] TextMeshProUGUI _name;
    [SerializeField] Transform _descriptionsParent;
    [SerializeField] TextMeshProUGUI _upgradeDescriptionPrefab;
    [SerializeField] Button _getButton, _dismantleButton;
    TextMeshProUGUI[] _descriptionEffects;
    SOPlayerInventory _inventory;
    public void Initialize(SOUpgradeBase upgrade, SOPlayerInventory inventory, Action closeMenuAction)
    {
        _icon.sprite = upgrade.Sprite;
        _name.text = upgrade.Name;
        _inventory = inventory;
        _getButton.RemoveAllEvents();
        _dismantleButton.RemoveAllEvents();
        _getButton.AddEventListener<SOUpgradeBase, Action>(GetUpgrade, upgrade, closeMenuAction);
        _dismantleButton.AddEventListener<SOUpgradeBase, Action>(DismantleUpgrade, upgrade, closeMenuAction);
        CheckDescriptionItems(upgrade);
    }

    void CheckDescriptionItems(SOUpgradeBase upgrade)
    {
        if(_descriptionEffects == null || _descriptionEffects.Length <= 0)
        {
            System.Array.Resize<TextMeshProUGUI>(ref _descriptionEffects, 5);
            for (int i = 0; i < 5; i++)
            {
                _descriptionEffects[i] = Instantiate(_upgradeDescriptionPrefab);
                _descriptionEffects[i].transform.SetParent(_descriptionsParent);
                _descriptionEffects[i].gameObject.SetActive(false);
            }
        }
        for (int i = 0; i < _descriptionEffects.Length; i++)
        {
            var item = _descriptionEffects[i];
            if(i >= upgrade.EffectDescriptions.Length)
            {
                item.gameObject.SetActive(false);
                continue;
            }
            var effect = upgrade.EffectDescriptions[i];
            item.text = effect.Text;
            item.color = effect.Color;
            item.gameObject.SetActive(true);
        }
    }

    void GetUpgrade(SOUpgradeBase upgrade, Action closeMenuAction)
    {
        _inventory.AddUpgrade(upgrade);
        closeMenuAction?.Invoke();
    }

    void DismantleUpgrade(SOUpgradeBase upgrade, Action closeMenuAction)
    {
        foreach(var upgradeCost in upgrade.Costs)
        {
            _inventory.AddMaterial(upgradeCost.CraftingMaterial, upgradeCost.Cost);
        }
        closeMenuAction?.Invoke();
    }

    private void OnDestroy() {
        _getButton.RemoveAllEvents();
        _dismantleButton.RemoveAllEvents();
    }

    public void OnSelect(BaseEventData eventData)
    {
        StartCoroutine(SelectButton());
    }

    IEnumerator SelectButton()
    {
        yield return null;
        yield return null;
        yield return null;
        EventSystem.current.SetSelectedGameObject(_getButton.gameObject);
    }
}
