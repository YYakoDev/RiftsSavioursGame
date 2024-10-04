using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

public class RewardSystem : MonoBehaviour
{
    [SerializeField] RewardAnimator _rewardsAnimator;
    [SerializeField] TokenMenu _tokenMenu;
    [SerializeField] WaveSystem _waveSys;
    [SerializeField] int _rewardGrantsThreshold = 2;
    [SerializeField] SORewardItem[] _possibleRewards;
    [SerializeField] InputActionReference _tokenConsumeInput;
    [SerializeField] SOPlayerInventory _playerInventory;
    [SerializeField] StoreMenu _storeMenu;
    [SerializeField] UIWeaponSelector _weaponSelector;
    [SerializeField] List<WeaponBase> _weaponSelection = new();
    int _wavesUntilReward;
    public InputActionReference TokenInput => _tokenConsumeInput;
    private void Start() {
        _tokenConsumeInput.action.performed += OpenTokenMenu;
        _waveSys.OnWaveEnd += GrantReward;
        _tokenMenu.Initialize(_possibleRewards);
        _playerInventory.OnTokenConsumption += UseToken;
        _wavesUntilReward = _rewardGrantsThreshold;
    }

    void GrantReward()
    {
        _wavesUntilReward--;
        if(_wavesUntilReward > 0) return;
        _wavesUntilReward = _rewardGrantsThreshold;
        var reward = _possibleRewards[Random.Range(0, _possibleRewards.Length)];
        _playerInventory.AddToken(reward.Type);
        _rewardsAnimator.Play(reward);
        Debug.Log("Reward granted");
    }
    
    void UseToken(RewardType type)
    {
        switch (type)
        {
            case RewardType.StoreToken:
                _storeMenu.OpenMenu();
                break;
            case RewardType.CraftToken:
                _storeMenu.OpenMenu();
                break;
            case RewardType.WeaponToken:
                OpenWeaponSelection();
                break;
        }

    }

    void OpenTokenMenu(InputAction.CallbackContext obj)
    {
        if(!_playerInventory.HasTokens())
        {
            NotificationSystem.SendNotification(NotificationType.Bottom, "No tokens available", null, 0.7f, 0.6f, 0.1f);
            return;
        }
        if(_playerInventory.GetTotalTokenCount() == 1)
        {
            SORewardItem item = null;
            var token = _playerInventory.GetFirstTokenAvailable();
            foreach(var reward in _possibleRewards)
            {
                if(reward.Type == token)
                {
                    item = reward;
                    break;
                }
            }
            if(item != null)_tokenMenu.ConsumeToken(item);
            return;
        }
        _tokenMenu.Open();
    }

    void OpenWeaponSelection()
    {
        //get a randomWeapon that is NOT the current one equipped
        var randomWeapon = _weaponSelection[Random.Range(0, _weaponSelection.Count)];
        _weaponSelector.OpenMenu(randomWeapon, null);
    }

    private void OnDestroy() {
        _tokenConsumeInput.action.performed -= OpenTokenMenu;
        _waveSys.OnWaveEnd -= GrantReward;
        _playerInventory.OnTokenConsumption -= UseToken;
    }
}
