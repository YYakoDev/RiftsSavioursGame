using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerInventoryUI : MonoBehaviour
{
    [SerializeField] SOPlayerInventory _playerInventory;
    [SerializeField] TextMeshProUGUI _balance;
    [SerializeField] CraftingMaterial _coinItem;
    int _ownedCoins = -1;
    private int _xpTextEmojiIndex = 0;

    private void Start() {
        _playerInventory.onInventoryChange += GetCoins;
        GetCoins();
    }

    void GetCoins()
    {
        var newCoins = _playerInventory.GetMaterialOwnedCount(_coinItem);
        if(newCoins != _ownedCoins)
        {
            _ownedCoins = newCoins;
            SetBalance();
        }
            
    }

    void SetBalance()
    {
        _balance.text = $"<sprite={_xpTextEmojiIndex}>{_ownedCoins}";
    }

    private void OnDestroy() {
        _playerInventory.onInventoryChange -= GetCoins;
    }
}
