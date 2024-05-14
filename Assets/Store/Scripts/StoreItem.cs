using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class StoreItem : MonoBehaviour
{
    [SerializeField] Image _icon, _itemBorder;
    [SerializeField] TextMeshProUGUI t_name, t_description, t_price;
    [SerializeField] Material _textBlackOutline, _textWhiteOutline;
    [SerializeField] Button _buyButton, _lockButton;
    [SerializeField] Image _lockIcon;
    [SerializeField] Sprite _lockedSprite, _unlockedSprite;
    bool _locked = false;
    [SerializeField] AudioSource _audio;
    [SerializeField] AudioClip _lockingSFX, _unlockingSFX;
    int _cost = 0;
    public int Cost => _cost;
    public bool IsLocked => _locked;
    private void Start() {
        _lockButton.AddEventListener(LockSwitch);
    }
    public void Initialize(SOUpgradeBase data, Action<SOStoreUpgrade, int> buyAction, SOStoreUpgrade upgradeIndex, int storeItemIndex, bool affordableItem = true, int coinTextIconIndex = 0)
    {
        _locked = false;
        _icon.sprite = data.Sprite;
        int color = data.Rarity switch
        {
            UpgradeRarity.Broken => 8,
            UpgradeRarity.Common => 0,
            UpgradeRarity.Uncommon => 2,
            UpgradeRarity.Rare => 5,
            UpgradeRarity.Epic => 7,
            UpgradeRarity.Legendary => 6,
            _ => 0
        };
        _itemBorder.color = UIColors.GetColor((UIColor)color);
        Material mat = data.Rarity switch
        {
            UpgradeRarity.Broken => _textBlackOutline,
            UpgradeRarity.Common => _textBlackOutline,
            UpgradeRarity.Uncommon => _textBlackOutline,
            UpgradeRarity.Rare => _textWhiteOutline,
            UpgradeRarity.Epic => _textWhiteOutline,
            UpgradeRarity.Legendary => _textBlackOutline,
            _ => _textBlackOutline
        };


        t_name.text = $"<color={UIColors.GetHexColor((UIColor)color)}>{data.Name}</color>";
        t_description.text = data.Description;
        t_name.fontMaterial = mat;
        _cost = data.Costs[0].Cost;
        UpdateCostText(affordableItem, coinTextIconIndex);
        _buyButton.RemoveAllEvents();
        _buyButton.AddEventListener(buyAction, upgradeIndex, storeItemIndex);

        SetTextMeshAutoSize(true);
        StartCoroutine(DisableAutoSize());
    }

    void LockSwitch()
    {
        _locked = !_locked;
        _lockIcon.sprite = _locked ? _lockedSprite : _unlockedSprite;
        _audio.PlayWithVaryingPitch(_locked ? _lockingSFX : _unlockingSFX);
    }

    public void UpdateCostText(bool affordable, int coinTextIconIndex)
    {
        var color = (affordable) ? UIColors.GetHexColor(UIColor.None):UIColors.GetHexColor(UIColor.Red);
        t_price.text = $"<sprite={coinTextIconIndex}><color={color}>{_cost}</color>";
        t_name.fontStyle = (affordable) ? FontStyles.Bold : FontStyles.Strikethrough;
        _icon.color = (!affordable) ? Color.grey : Color.white;
        _buyButton.interactable = affordable;
    }

    IEnumerator DisableAutoSize()
    {
        yield return null;
        yield return null;
        SetTextMeshAutoSize(false);
    }
    void SetTextMeshAutoSize(bool state)
    {
        t_name.enableAutoSizing = state;
        t_description.enableAutoSizing = state;
    }
}
