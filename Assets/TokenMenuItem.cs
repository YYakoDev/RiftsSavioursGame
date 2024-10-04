using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TokenMenuItem : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _name, _count;
    [SerializeField] Image _icon;
    [SerializeField] Button _button;

    public void Set(string name, int count, Sprite icon, SORewardItem token, Action<SORewardItem> onClick)
    {
        _name.SetText(name);
        _count.SetText("x"+count);
        _icon.sprite = icon;
        _button.RemoveAllEvents();
        _button.AddEventListener<SORewardItem>(onClick, token);
        _button.interactable = count > 0;
    }

    public void Set(int count)
    {
        _count.SetText("x"+count);
        _button.interactable = count > 0;
    }
}
