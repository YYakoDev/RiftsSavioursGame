using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIBlacksmithUpgrade : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _title, _description;
    [SerializeField] Image _icon;
    [SerializeField] Button _button;

    public void Init(SOUpgradeBase upgrade, Action<SOUpgradeBase> onClick)
    {
        _title.SetText(upgrade?.Name);
        _description.SetText(upgrade?.Description);
        _icon.sprite = upgrade?.Sprite;
        _button.RemoveAllEvents();
        _button.AddEventListener(onClick, upgrade);
    }
}
