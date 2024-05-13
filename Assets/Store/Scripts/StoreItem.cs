using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class StoreItem : MonoBehaviour
{
    [SerializeField] Image _icon;
    [SerializeField] TextMeshProUGUI t_name, t_description, t_price;
    [SerializeField] Material _textBlackOutline, _textWhiteOutline;

    public void Initialize(SOUpgradeBase data, int coinTextIconIndex = 0)
    {
        _icon.sprite = data.Sprite;
        string color = data.Rarity switch
        {
            UpgradeRarity.Broken => UIColors.GetHexColor(UIColor.Grey),
            UpgradeRarity.Common => UIColors.GetHexColor(UIColor.None),
            UpgradeRarity.Uncommon => UIColors.GetHexColor(UIColor.Green),
            UpgradeRarity.Rare => UIColors.GetHexColor(UIColor.Blue),
            UpgradeRarity.Epic => UIColors.GetHexColor(UIColor.Purple),
            UpgradeRarity.Legendary => UIColors.GetHexColor(UIColor.Orange),
            _ => UIColors.GetHexColor(UIColor.None)
        };
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


        t_name.text = $"<color={color}>{data.Name}</color>";
        t_description.text = data.Description;
        t_name.fontMaterial = mat;
        t_price.text = $"<sprite={coinTextIconIndex}>{data.Costs[0].Cost}";
        SetTextMeshAutoSize(true);
        StartCoroutine(DisableAutoSize());
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
