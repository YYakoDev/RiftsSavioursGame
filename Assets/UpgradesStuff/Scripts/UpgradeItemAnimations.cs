using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeItemAnimations : MonoBehaviour
{
    [SerializeField]UpgradeItemPrefab _upgradePrefab;
    UpgradeMenuAnimations _menuAnimations;
    [SerializeField]Animator _animator;
    [SerializeField] float _animDuration;
    Timer _animationTimer;

    private void Awake() {
        GameObject go = gameObject;
        go.CheckComponent<UpgradeItemPrefab>(ref _upgradePrefab);
        go.CheckComponent<Animator>(ref _animator);
        SetUiItemsState(false);
        _animator.enabled = true;
        _animationTimer = new(_animDuration, useUnscaledTime: true);
        _animationTimer.onEnd += ActivateUiItems;
        _animationTimer.Stop();
        _menuAnimations = go.transform.parent.GetComponentInParent<UpgradeMenuAnimations>();
        //_menuAnimations.OnOpenAnimationFinish += PlayAnimations;
    }

    private void Update() {
        _animationTimer.UpdateTime();
    }

    private void OnEnable() {
        SetUiItemsState(false);
        _animator.enabled = true;
    }

    void PlayAnimations()
    {
        _animationTimer.Start();
        _animator.Play("Animation");
    }
    void ActivateUiItems()
    {
        _animator.enabled = false;
        SetUiItemsState(true);
    }

    void SetUiItemsState(bool state)
    {
        _upgradePrefab.ItemsParent.gameObject.SetActive(state);
        /*_upgradePrefab.Name.enabled = state;
        _upgradePrefab.Description.enabled = state;
        _upgradePrefab.Icon.enabled = state;
        _upgradePrefab.CraftBtn.enabled = state;
        _upgradePrefab.ButtonText.enabled = state;
        _upgradePrefab.CostContainer.gameObject.SetActive(state);*/
    }

    private void OnDestroy() {
        _animationTimer.onEnd -= ActivateUiItems;
        //_menuAnimations.OnOpenAnimationFinish -= PlayAnimations;
    }

    
    
}
