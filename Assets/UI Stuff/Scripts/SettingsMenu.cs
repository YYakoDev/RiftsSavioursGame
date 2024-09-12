using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using TMPro;

public class SettingsMenu : MonoBehaviour
{
    [SerializeField]InputActionReference _uiEscHotkey, _gameplayEscHotkey;
    [SerializeField] PlayerInputController _inputController;
    [SerializeField] MenuController _menuController;
    [SerializeField] GameObject _menuParent, _pauseMenuParent;
    [SerializeField]Canvas _canvas;
    //esto de abajo no tiene que ser parte de esta clase
    [SerializeField] UISkillsManager _uiSkillsManager;
    [SerializeField] Sprite _settingsIcon;
    [SerializeField] Vector3 _iconPosition;
    UISkill _inputItem;
    //
    [SerializeField] AudioMixer _masterMixer;
    [SerializeField] Slider _soundsSlider, _musicSlider;
    [SerializeField] TextMeshProUGUI t_soundsValue, t_musicValue;
    bool _menuState;
    float _soundsStartValue = -1f, _musicStartValue = -1f;
    private void Awake() {
        _menuState = _menuParent.activeInHierarchy;
        if(_menuState) OpenMenu();
    }

    // Start is called before the first frame update
    void Start()
    {
        Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
        _inputItem = _uiSkillsManager.SetInputSkill(_uiEscHotkey, _settingsIcon);
        var itemTransform = _inputItem.transform;
        itemTransform.SetParent(_uiSkillsManager.transform, false);
        itemTransform.localPosition = _iconPosition;
        _uiEscHotkey.action.performed += PlaySettingsButtonAnimation;
        _gameplayEscHotkey.action.performed += PlaySettingsButtonAnimation;
        //Debug.Log(_inputItem.GetItemSize());
        _soundsStartValue = PlayerPrefs.GetFloat("SoundsVolume", 50);
        _musicStartValue = PlayerPrefs.GetFloat("MusicVolume", 50);
        
        SetSliders(); 
    }

    void SetSliders()
    {
        _soundsSlider.minValue = 0;
        _soundsSlider.maxValue = 100;
        _soundsSlider.wholeNumbers = true;
        _soundsSlider.value = _soundsStartValue;
        _musicSlider.minValue = 0;
        _musicSlider.maxValue = 100;
        _musicSlider.wholeNumbers = true;
        _musicSlider.value = _musicStartValue;
        SetSoundsValue(_soundsStartValue);
        SetMusicValue(_musicStartValue);
    }

    void PlaySettingsButtonAnimation(InputAction.CallbackContext obj)
    {
        _inputItem.Interact();
        if(_menuState) CloseMenu();
    }

    public void OpenMenu()
    {
        _menuController.SwitchCurrentMenu(gameObject);
        _menuState = true;
        //do opening sound and animation
        PauseMenuManager.DisablePauseBehaviour(true);
        _inputController.ChangeInputToUI();
        _menuParent.SetActive(true);
        TimeScaleManager.ForceTimeScale(0f);
    }

    public void CloseMenu()
    {
        //do closing sound and animation
        _menuState = false;
        PauseMenuManager.DisablePauseBehaviour(false);
        //_inputController.ChangeInputToGameplay();
        _menuParent.SetActive(false);
        _menuController.SwitchCurrentMenu(_pauseMenuParent);
        //TimeScaleManager.ForceTimeScale(1f);
    }

    public void ToggleFullscreen(bool state)
    {
        Debug.Log("FUllscreen!!! :)");
        if(state)
        {
            Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
        }else
        {
            Screen.fullScreenMode = FullScreenMode.Windowed;
        }
        Screen.fullScreen = !Screen.fullScreen;
    }

    public void SetSoundsValue(float value)
    {
        t_soundsValue.SetText((value).ToString());
        PlayerPrefs.SetFloat("SoundsVolume", value);
        var volume = Mathf.Lerp(-42, 12, value / 100f);
        if(volume <= -41) volume = -80;
        _masterMixer.SetFloat("SoundsVolume", volume);
    }

    public void SetMusicValue(float value)
    {
        t_musicValue.SetText((value).ToString());
        PlayerPrefs.SetFloat("MusicVolume", value);
        var volume = Mathf.Lerp(-42, 12, value / 100f);
        if(volume <= -41) volume = -80;
        _masterMixer.SetFloat("MusicVolume", volume);
    }

    private void OnDestroy() {
        _uiEscHotkey.action.performed -= PlaySettingsButtonAnimation;
        _gameplayEscHotkey.action.performed -= PlaySettingsButtonAnimation;
    }

    private void OnDrawGizmosSelected() {
        if(Application.isPlaying) return;
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireCube(_canvas.TranslateUiToWorldPoint(_iconPosition), Vector3.one * 30f);
    }

}
 