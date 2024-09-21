using System;
using UnityEngine;

public class WeaponEvents 
{
    public event Action OnAttack, OnHeavyAttackCharging, OnHeavyAttackChargeComplete, OnHeavyAttack, OnSwitchAttack;
    public event Action<int> OnComboAttack;
    public event Action<Transform> OnEnemyHit;
    public void Initialize()
    {
        OnAttack = null; OnHeavyAttackCharging = null; OnHeavyAttackChargeComplete = null; OnHeavyAttack = null; OnSwitchAttack = null; OnComboAttack = null; OnEnemyHit = null;
    }

    public void FireAttackEvent() => OnAttack?.Invoke();
    public void FireHeavyAttackChargingEvent() => OnHeavyAttackCharging?.Invoke();
    public void FireHeavyAttackChargeCompleteEvent() => OnHeavyAttackChargeComplete?.Invoke();
    public void FireHeavyAttackEvent() => OnHeavyAttack?.Invoke();
    public void FireSwitchAttackEvent() => OnSwitchAttack?.Invoke();
    public void FireComboAttackEvent(int comboIndex) => OnComboAttack?.Invoke(comboIndex);
    public void FireEnemyHitEvent(Transform enemy) => OnEnemyHit?.Invoke(enemy);


}
