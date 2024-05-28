using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PrototypeResultPrefab : MonoBehaviour // this could be a generic class btw
{
    [SerializeField] TextMeshProUGUI _name;
    [SerializeField] Image _icon;

    public void InitializeResult(string name, Sprite sprite)
    {
        _name.text = name;
        _icon.sprite = sprite;
    }
}
