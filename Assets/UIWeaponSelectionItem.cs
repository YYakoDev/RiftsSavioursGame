using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIWeaponSelectionItem : MonoBehaviour
{
    [SerializeField] Image _icon;
    [SerializeField] TextMeshProUGUI t_weaponName;
    [SerializeField] Button _button;
    [SerializeField] Sprite _emptySprite;

    public void Init(WeaponBase weapon, Action<int> onSelect, int buttonIndex = 2)
    {
        if(weapon == null)
        {
            _icon.sprite = _emptySprite;
            t_weaponName.SetText("Empty");
            //_button.interactable = false;
            if(onSelect != null)
            {
                _button.AddEventListener<int>(onSelect, buttonIndex);
                _button.interactable = true;
            }
            return;
        }
        _button.RemoveAllEvents();
        t_weaponName.SetText(weapon.WeaponName);
        _icon.sprite = weapon.SpriteAndAnimationData.Sprite;
        
        if(onSelect != null)
        {
            _button.AddEventListener<int>(onSelect, buttonIndex);
            _button.interactable = true;
        }
        else _button.interactable = false;
    }
}
