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
    private SOStoreUpgrade _usedUpgrade;
    int _upgradeIndex = -1;
    //public SOStoreUpgrade CurrentUpgrade => _usedUpgrade;
    public UpgradeRarity Rarity => _usedUpgrade.Rarity;
    public int UpgradeIndex => _upgradeIndex;
    public int Cost => _cost;
    public bool IsLocked => _locked;
    private void Start() {
        _lockButton.AddEventListener(LockSwitch);
    }
    public void Initialize
    (SOStoreUpgrade upgradeData, Action<SOStoreUpgrade, int> buyAction, int storeItemIndex, int upgradeIndex, bool affordableItem = true, int coinTextIconIndex = 0)
    {
        _usedUpgrade = upgradeData;
        _upgradeIndex = upgradeIndex;
        _locked = false;
        _icon.sprite = upgradeData.Sprite;
        Color color = UIColors.GetColor((UIColor)UIColors.GetRarityColorIndex(upgradeData.Rarity));
        _itemBorder.color = color;
        Material mat = upgradeData.Rarity switch
        {
            UpgradeRarity.Broken => _textBlackOutline,
            UpgradeRarity.Common => _textBlackOutline,
            UpgradeRarity.Uncommon => _textBlackOutline,
            UpgradeRarity.Rare => _textWhiteOutline,
            UpgradeRarity.Epic => _textWhiteOutline,
            UpgradeRarity.Legendary => _textBlackOutline,
            _ => _textBlackOutline
        };
        t_name.color = color;
        t_name.text = upgradeData.Name;
        t_description.text = upgradeData.Description;
        t_name.fontMaterial = mat;
        _cost = upgradeData.Costs[0].Cost;
        UpdateCostText(affordableItem, coinTextIconIndex);
        _buyButton.RemoveAllEvents();
        _buyButton.AddEventListener(buyAction, upgradeData, storeItemIndex);

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
        var priceColor = (affordable) ? UIColors.GetHexColor(UIColor.None):UIColors.GetHexColor(UIColor.Red);
        t_price.text = $"<sprite={coinTextIconIndex}><color={priceColor}>{_cost}</color>";
        var color = (affordable) ? UIColors.GetColor((UIColor)UIColors.GetRarityColorIndex(_usedUpgrade.Rarity)) :  UIColors.GetColor(UIColor.Transparent);
        t_name.color = UIColors.GetColor(UIColor.Grey);
        _itemBorder.color = color;
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
