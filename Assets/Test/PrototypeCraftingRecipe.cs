using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class PrototypeCraftingRecipe : MonoBehaviour, ISelectHandler
{
    [SerializeField] TextMeshProUGUI _name;
    [SerializeField] Image _icon;
    [SerializeField] Button _button;
    int _index;
    SOUpgradeBase _upgrade;
    Action<SOUpgradeBase, int> _onSelect;
    public SOUpgradeBase Upgrade => _upgrade;
    public Button Button => _button;

    public void InitializeRecipe(SOUpgradeBase upgrade, Action onClick, Action<SOUpgradeBase, int> onSelect, int index)
    {
        _upgrade = upgrade;
        _icon.sprite = upgrade.Sprite;
        _name.text = upgrade.Name;
        _index = index;
        _onSelect = onSelect;
        _button.RemoveAllEvents();
        _button.AddEventListener(onClick);
    }

    public void OnSelect(BaseEventData eventData)
    {
        _onSelect?.Invoke(_upgrade, _index);
    }

}
