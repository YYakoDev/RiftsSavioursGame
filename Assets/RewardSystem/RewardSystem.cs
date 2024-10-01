using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

public class RewardSystem : MonoBehaviour
{
    [SerializeField] RewardAnimator _rewardsAnimator;
    [SerializeField] WaveSystem _waveSys;
    [SerializeField] RewardItem[] _possibleRewards;
    [SerializeField] InputActionReference _tokenConsumeInput;
    [SerializeField] SOPlayerInventory _playerInventory;
    [SerializeField] StoreMenu _storeMenu;
    [SerializeField] UIWeaponSelector _weaponSelector;
    [SerializeField] List<WeaponBase> _weaponSelection = new();
    public event Action<RewardType> OnTokenConsumption;

    public InputActionReference TokenInput => _tokenConsumeInput;

    private void Start() {
        _tokenConsumeInput.action.performed += ConsumeToken;
        _waveSys.OnWaveEnd += GrantReward;
    }

    void GrantReward()
    {
        var reward = _possibleRewards[Random.Range(0, _possibleRewards.Length)];
        _playerInventory.AddToken(reward);
        _rewardsAnimator.Play(reward);
        //Instantiate(reward, Vector3.down + new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f), 0f), Quaternion.identity);
        Debug.Log("Reward granted");
    }
    
    void ConsumeToken(InputAction.CallbackContext obj)
    {
        var token = _playerInventory.GetToken();
        if(token == null)
        {
            NotificationSystem.SendNotification(NotificationType.Bottom, "No tokens available", null, 0.7f, 0.6f, 0.1f);
            return;
        }
        switch(token.Type)
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
        token.Available = false;
        OnTokenConsumption?.Invoke(token.Type);

    }

    void OpenWeaponSelection()
    {
        var randomWeapon = _weaponSelection[Random.Range(0, _weaponSelection.Count)];
        _weaponSelector.OpenMenu(randomWeapon, null);
    }

    private void OnDestroy() {
        _tokenConsumeInput.action.performed -= ConsumeToken;
        _waveSys.OnWaveEnd -= GrantReward;
    }
}
