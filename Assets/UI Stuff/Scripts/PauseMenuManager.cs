using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PauseMenuManager : MonoBehaviour
{
    [SerializeField] PlayerInput _input;
    [SerializeField] MenuController _menuController;
    static bool _disablePauseBehaviour = false;
    [SerializeField] GameObject _pauseMenuParent, _confirmationObj, _continueButton;
    //[SerializeField] UpgradesMenu _upgradeMenu;
    bool _activeMenu = false;
    bool _goToMainMenu = false;
    float _previousTimeScale = 1f;
    EventSystem _eventSys;
    GameObject _previouslySelectedObj;
    [SerializeField]InputActionReference _escapeButton, _UICancelButton;


    public static void DisablePauseBehaviour(bool state)
    {
        _disablePauseBehaviour = state;
    }

    // Start is called before the first frame update
    void Start()
    {
        _disablePauseBehaviour = false;
        _activeMenu = false;
        _confirmationObj.SetActive(false);
        //_upgradeMenu.OnMenuClose += ClosingCheck;
        _eventSys = EventSystem.current;
        _escapeButton.action.performed += SwitchPauseMenuWithInput;
        _UICancelButton.action.performed += SwitchPauseMenuWithInput;
    }
    void SwitchPauseMenuWithInput(InputAction.CallbackContext obj)
    {
        //
        if(_disablePauseBehaviour)return;
        SwitchPauseMenu(!_activeMenu);
    }

    public void Continue()
    {
        SwitchPauseMenu(false);
    }
    public void GoToMainMenu()
    {
        _confirmationObj.SetActive(true);
        if(!_goToMainMenu)
        {
            _goToMainMenu = true;
            return;
        }
        SceneManager.LoadScene(0);
    }

    void SwitchPauseMenu(bool state)
    {
        if(state)
        {
            _previousTimeScale = (TimeScaleManager.IsForced) ?  0f : 1f;
            _previouslySelectedObj = _eventSys.currentSelectedGameObject;
            _eventSys.SetSelectedGameObject(_continueButton);
            _disablePauseBehaviour = false;
            _input.SwitchCurrentActionMap("UI");
            _menuController.SwitchCurrentMenu(_pauseMenuParent);
        }
        _activeMenu = state;
        _confirmationObj.SetActive(false);
        _goToMainMenu = false;
        _pauseMenuParent.SetActive(state);
        float timeScale = (_activeMenu) ? 0 : _previousTimeScale;
        TimeScaleManager.ForceTimeScale(timeScale);

        if(!state)
        {
            //YYInputManager.ResumeInput();
            _eventSys.SetSelectedGameObject(_previouslySelectedObj);
            _input.SwitchCurrentActionMap("GAMEPLAY");
            _menuController.SwitchCurrentMenu(null);
        }
    }

    private void OnDestroy() {
        _escapeButton.action.performed -= SwitchPauseMenuWithInput;
        _UICancelButton.action.performed -= SwitchPauseMenuWithInput;
    }
}
