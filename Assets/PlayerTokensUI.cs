using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class PlayerTokensUI : MonoBehaviour
{
    [SerializeField] SOPlayerInventory _inventory;
    [SerializeField] RewardSystem _rewardSystem;
    int _tokenCount;
    [SerializeField] TokenItemUIAnimation _tokenItemUI;

    private void Start() {
        
        _tokenItemUI.SetText($"({_rewardSystem.TokenInput.action.GetBindingDisplayString()}) Token Available");
        _rewardSystem.OnTokenConsumption += OnTokenConsumption;
        _inventory.OnTokenAddition += CheckTokens;
    }


    void CheckTokens()
    {
        _tokenItemUI.Play();
    }

    void OnTokenConsumption(RewardType rewardType)
    {
        var token = _inventory.GetToken();
        if(token == null) _tokenItemUI.Hide();
        else _tokenItemUI.Shake();
    }

    private void OnDestroy() {
        _rewardSystem.OnTokenConsumption -= OnTokenConsumption;
        _inventory.OnTokenAddition -= CheckTokens;
    }

}
