using UnityEngine;
public interface IQuickSwitchHandler
{
    public void QuickSwitch(QuickSwitchInfo info);
    public QuickSwitchInfo GetSwitchInfo();
}

public class QuickSwitchInfo
{
    int _comboIndex;
    bool _isHeavyAttack, _wasHoldingHeavyButton;
    public int ComboIndex => _comboIndex;
    public bool IsHeavyAttack => _isHeavyAttack;
    public bool WasHoldingHeavyAtkButton => _wasHoldingHeavyButton;

    public QuickSwitchInfo(int comboIndex, bool heavyAttack, bool wasHoldingHeavyButton)
    {
        _comboIndex = comboIndex;
        _isHeavyAttack = heavyAttack;
        _wasHoldingHeavyButton = wasHoldingHeavyButton;
    }

    public void SetComboIndex(int index) => _comboIndex = index;
    public void SetHeavyAttack(bool state) => _isHeavyAttack = state;
    public void SetHoldingButtonState(bool state) => _wasHoldingHeavyButton = state;
}
