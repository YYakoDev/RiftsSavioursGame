using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] PlayerInput _inputController;
    [SerializeField] Button _startButton;
    EventSystem _currentEventSys;
    [SerializeField] GameObject _selectionMenu;
    [SerializeField] SettingsMenu _settingsMenu;
    [SerializeField] FeedbackMenu _feedbackMenu;
    [SerializeField] MenuQuitter _menuQuitter;
    [SerializeField]AudioSource _audio;
    [SerializeField] AudioClip _selectionSFX;

    // Start is called before the first frame update
    void Start()
    {
        _inputController.SwitchCurrentActionMap("UI");
        _currentEventSys = EventSystem.current;
        _selectionMenu?.SetActive(false);
        _currentEventSys.SetSelectedGameObject(_startButton.gameObject);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private void OnEnable() {
        TimeScaleManager.ForceTimeScale(1f);
    }

    public void SetSelectionMenuActive()
    {
        gameObject.SetActive(false);
        _selectionMenu.SetActive(true);
        _audio.PlayWithVaryingPitch(_selectionSFX);
        _menuQuitter.SetCurrentMenu(_selectionMenu);
    }

    public void PlayButton()
    {
        _inputController.SwitchCurrentActionMap("GAMEPLAY");
        _audio.PlayWithVaryingPitch(_selectionSFX);
        UpgradeCreator.CreateUpgrades();
        SceneManager.LoadScene(1);
    }
    public void OpenSettings()
    {
        _audio.PlayWithVaryingPitch(_selectionSFX);
        _settingsMenu.OpenMenu();
        //_menuQuitter.SetCurrentMenu(_settingsMenu.gameObject);
    }

    public void QuitButton()
    {
        _audio.PlayWithVaryingPitch(_selectionSFX);
        //SAVE DATA HERE????
        Application.Quit();
    }

    public void CloseSelectionMenu()
    {
        gameObject.SetActive(true);
        _selectionMenu.SetActive(false);
        _menuQuitter.SetCurrentMenu(null);
    }
    
    public void OpenFeedbackMenu()
    {
        gameObject.SetActive(false);
        _feedbackMenu.Enable();
        _currentEventSys.SetSelectedGameObject(_feedbackMenu.gameObject);
        _audio.PlayWithVaryingPitch(_selectionSFX);
        //_menuQuitter.SetCurrentMenu(_feedbackMenu.gameObject);
    }
}
