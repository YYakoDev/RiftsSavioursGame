using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] Button _startButton;
    EventSystem _currentEventSys;
    [SerializeField] GameObject _selectionMenu;
    [SerializeField] GameObject _feedbackMenuParent, _feedbackMenuLogic;
    [SerializeField] MenuQuitter _menuQuitter;
    [SerializeField]AudioSource _audio;
    [SerializeField] AudioClip _selectionSFX;

    // Start is called before the first frame update
    void Start()
    {
        _currentEventSys = EventSystem.current;
        _selectionMenu?.SetActive(false);
        _currentEventSys.SetSelectedGameObject(_startButton.gameObject);
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
        _feedbackMenuParent.SetActive(true);
        _currentEventSys.SetSelectedGameObject(_feedbackMenuLogic);
        _audio.PlayWithVaryingPitch(_selectionSFX);
        _menuQuitter.SetCurrentMenu(_feedbackMenuParent);
    }

    public void CloseFeedbackMenu()
    {
        gameObject.SetActive(true);
        _feedbackMenuParent.SetActive(false);
        _audio.PlayWithVaryingPitch(_selectionSFX);
        _menuQuitter.SetCurrentMenu(null);
    }
}
