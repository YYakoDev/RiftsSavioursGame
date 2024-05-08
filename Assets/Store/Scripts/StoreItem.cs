using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class StoreItem : MonoBehaviour
{
    [SerializeField] Image _icon;
    [SerializeField] TextMeshProUGUI t_name, t_description, t_price;


    public void Initialize(Sprite icon, string name, string description, int price, int coinIconIndex)
    {
        _icon.sprite = icon;
        t_name.text = name;
        t_description.text = description;
        t_price.text = $"<sprite={coinIconIndex}>{price}";
    }
}
