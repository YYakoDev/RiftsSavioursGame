using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TokenMenu : MonoBehaviour
{
    [SerializeField] SOPlayerInventory _playerInventory;
    [SerializeField] PlayerInputController _inputController;
    [SerializeField] InputActionReference _escapeInput;
    [SerializeField] MenuController _menuController;
    [SerializeField] GameObject _menuParent;
    [SerializeField] AudioSource _audio;
    [SerializeField] TokenMenuItem _tokenPrefab;
    [SerializeField] Transform _tokensParent;
    SORewardItem[] _possibleRewards;
    TokenMenuItem[] _tokens;

    private void Start() {
        _menuParent.SetActive(false);
        _escapeInput.action.performed += CloseWithInput;
    }

    public void Initialize(SORewardItem[] rewards)
    {
        _possibleRewards = rewards;
        SetItems();
    }

    public void Open()
    {
        if(_menuParent.activeInHierarchy) return;
        _menuController.SwitchCurrentMenu(_menuParent);
        PauseMenuManager.DisablePauseBehaviour(true);
        TimeScaleManager.ForceTimeScale(0f);
        SetItems();
        _menuParent.SetActive(true);
        _inputController.ChangeInputToUI();
    }

    void SetItems()
    {
        if(_tokens == null || _tokens.Length == 0) CreateItems();
        for (int i = 0; i < _tokens.Length; i++)
        {
            var reward = _possibleRewards[i];
            _tokens[i].Set(_playerInventory.GetTokenCount(reward.Type));
        }
    }

    void CreateItems()
    {
        _tokens = new TokenMenuItem[_possibleRewards.Length];
        for (int i = 0; i < _tokens.Length; i++)
        {
            _tokens[i] = Instantiate(_tokenPrefab, _tokensParent);
            var reward = _possibleRewards[i];
            _tokens[i].Set(reward.Name, _playerInventory.GetTokenCount(reward.Type), reward.Sprite, reward, ConsumeToken);
        }
    }


    public void ConsumeToken(SORewardItem reward)
    {
        _menuController.SwitchCurrentMenu(null);
        Close();
        TimeScaleManager.ForceTimeScale(1f);
        _audio.PlayOneShot(reward.Sfx);
        _playerInventory.ConsumeToken(reward.Type);
    }

    void CloseWithInput(InputAction.CallbackContext obj)
    {
        Close();
    }

    public void Close()
    {
        if(!_menuParent.activeInHierarchy) return;
        PauseMenuManager.DisablePauseBehaviour(false);
        _inputController.ChangeInputToGameplay();
        _menuParent.SetActive(false);
    }

    private void OnDestroy() {
        _escapeInput.action.performed -= CloseWithInput;
    }
}
