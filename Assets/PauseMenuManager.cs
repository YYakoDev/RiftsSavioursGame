using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class PauseMenuManager : MonoBehaviour
{
    [SerializeField] GameObject _pauseMenuParent;
    [SerializeField] GameObject _confirmationObj;
    [SerializeField] GameObject _continueButton;
    [SerializeField] UpgradesMenu _upgradeMenu;
    bool _activeMenu = false;
    bool _goToMainMenu = false;
    float _previousTimeScale = 1f;
    EventSystem _eventSys;
    GameObject _previouslySelectedObj;

    // Start is called before the first frame update
    void Start()
    {
        _activeMenu = false;
        _confirmationObj.SetActive(false);
        _upgradeMenu.OnMenuClose += ClosingCheck;
        _eventSys = EventSystem.current;
    }
    

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P))
        {
            SwitchPauseMenu(!_activeMenu);
        }
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
        }
        _activeMenu = state;
        _confirmationObj.SetActive(false);
        _goToMainMenu = false;
        _pauseMenuParent.SetActive(state);
        float timeScale = (_activeMenu) ? 0 : _previousTimeScale;
        TimeScaleManager.ForceTimeScale(timeScale);

        if(!state)
        {
            YYInputManager.ResumeInput();
            _eventSys.SetSelectedGameObject(_previouslySelectedObj);
        }
        else YYInputManager.StopInput();
    }

    void ClosingCheck()
    {
        if(_activeMenu)
        {
            YYInputManager.StopInput();
            Debug.Log("setting game to paused");
            TimeScaleManager.ForceTimeScale(0f);
        }
    }
    private void OnDestroy() {
        _upgradeMenu.OnMenuClose -= ClosingCheck;
    }
}
