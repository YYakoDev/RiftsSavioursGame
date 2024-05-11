using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Random = UnityEngine.Random;

[RequireComponent(typeof(TweenAnimatorMultiple))]
public class UILevelUpPopup : MonoBehaviour
{
    [SerializeField] bool _playOnStart;
    [SerializeField] SOPossibleUpgradesList _possibleUpgradesManager;
    [SerializeField] SOPlayerInventory _playerInventory;
    [SerializeField] RectTransform _levelUpRewardMenu;
    [SerializeField] RewardItem _rewardItemPrefab;
    [SerializeField] Vector3 _itemPosition;
    RectTransform _rewardItemRect;
    RewardItem _rewardItemInstance;
    GameObject _rewardMenuObj;
    TweenAnimatorMultiple _animator;
    EventSystem _eventSys;

    [Header("Animation Values")]
    [SerializeField] float _animDuration = 0.5f;

    [Header("Audio Stuff")]
    [SerializeField] AudioSource _audio;
    [SerializeField] AudioClip _levelUpSfx, _closingMenuSfx;
    private void Awake() {
        _animator = GetComponent<TweenAnimatorMultiple>();
        _animator.ChangeTimeScalingUsage(TweenAnimator.TimeUsage.UnscaledTime);
        //PlayerLevelManager.onLevelUp += Play;
    }

    private void Start() {
        _rewardMenuObj = _levelUpRewardMenu.gameObject;
        _eventSys = EventSystem.current;
        if(_playOnStart) Play();
        else _rewardMenuObj.SetActive(false);
    }

    void SetInitialStates(SOUpgradeBase upgrade)
    {
        _animator.Clear();

        CheckItem(upgrade, CloseMenu);
        _levelUpRewardMenu.localScale = Vector3.one;
        _rewardItemInstance.transform.localScale = Vector3.right;
        //maybe the position of the menu itself here
    }

    void Play()
    {
        var upgrade = ChooseRandomUpgrade();
        if(upgrade == null)
        {
            //Debug.Log("Player has all the upgrades, closing rewards menu");
            TimeScaleManager.ForceTimeScale(1f);
            _levelUpRewardMenu.gameObject.SetActive(false);
            return;
        }
        SetInitialStates(upgrade);
        PlaySound(_levelUpSfx);
        _rewardMenuObj.SetActive(true);
        TimeScaleManager.ForceTimeScale(0f);
        _rewardItemInstance.gameObject.SetActive(true);
        _animator.Scale(_rewardItemRect, Vector3.one, _animDuration, onComplete: SelectItemOnUI);

    }

    

    void CheckItem(SOUpgradeBase upgrade, Action closeMenuAction)
    {
        if(_rewardItemInstance == null)
        {
            _rewardItemInstance = Instantiate(_rewardItemPrefab);
            _rewardItemInstance.transform.SetParent(_rewardMenuObj.transform);
            _rewardItemInstance.transform.localPosition = _itemPosition;
            _rewardItemRect = _rewardItemInstance.GetComponent<RectTransform>();
            _rewardItemInstance.gameObject.SetActive(false);
        }
        _rewardItemInstance.Initialize(upgrade, _playerInventory, closeMenuAction);
    }

    SOUpgradeBase ChooseRandomUpgrade()
    {
        var possibleUpgrades = _possibleUpgradesManager.PossibleUpgrades;
        if(possibleUpgrades == null || possibleUpgrades.Count <= 0) return null;
        return possibleUpgrades[Random.Range(0, possibleUpgrades.Count)]?.GetUpgrade();
    }

    void SelectItemOnUI()
    {
        _eventSys.SetSelectedGameObject(_rewardItemInstance.gameObject);
    }

    void PlaySound(AudioClip clip)
    {
        if(_audio == null) return;
        _audio.PlayWithVaryingPitch(clip);
    }

    public void CloseMenu()
    {
        PlaySound(_closingMenuSfx);
        _animator.Scale(_levelUpRewardMenu, Vector3.zero, _animDuration / 2f, onComplete: () =>
        {
            _rewardMenuObj.SetActive(false);
            TimeScaleManager.ForceTimeScale(1f);

        });
    }

    private void OnDestroy() {
        //PlayerLevelManager.onLevelUp -= Play;
    }
}
