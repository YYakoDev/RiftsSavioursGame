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
        _inventory.OnTokenAddition += CheckTokens;
        _inventory.OnTokenConsumption += OnTokenConsumption;
    }


    void CheckTokens()
    {
        _tokenItemUI.Play();
    }

    void OnTokenConsumption(RewardType rewardType)
    {
        var hasTokens = _inventory.HasTokens();
        if(!hasTokens) _tokenItemUI.Hide();
        else _tokenItemUI.Shake();
    }

    private void OnDestroy() {
        _inventory.OnTokenAddition -= CheckTokens;
        _inventory.OnTokenConsumption -= OnTokenConsumption;
    }

}
