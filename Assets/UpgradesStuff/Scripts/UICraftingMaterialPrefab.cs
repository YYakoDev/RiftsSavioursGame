using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UICraftingMaterialPrefab : MonoBehaviour
{
    [SerializeField]Image i_icon;
    [SerializeField]TextMeshProUGUI t_amount;
    int _amount;
    private void Awake()
    {
        if(i_icon == null) i_icon = GetComponent<Image>();
        if(t_amount == null) t_amount = GetComponent<TextMeshProUGUI>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void Initialize(CraftingMaterial material, int amount)
    {
        i_icon.sprite = material.Sprite;

        if(_amount == amount && _amount != 0) return;
        _amount = amount;
        t_amount.text = _amount.ToString();
    }
}
