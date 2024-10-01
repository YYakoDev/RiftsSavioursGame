using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIRewardItem : MonoBehaviour
{
    [SerializeField] RectTransform _rect;
    [SerializeField] Image i_icon;
    [SerializeField] TextMeshProUGUI t_tokenName, t_tokenDescription;

    public RectTransform Rect => _rect;

    public void Set(Sprite icon, string name, string description)
    {
        i_icon.sprite = icon;
        t_tokenName.SetText(name);
        t_tokenDescription.SetText(description);
    }
}
