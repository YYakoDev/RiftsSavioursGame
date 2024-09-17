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
    [SerializeField] ScreenController _screenController;
    [SerializeField] MenuController _menuController;
    [SerializeField] GameObject _menuParent, _pauseMenuParent;
    [SerializeField]Canvas _canvas;
    bool _menuState;
    //esto de abajo no tiene que ser parte de esta clase
    [SerializeField] UISkillsManager _uiSkillsManager;
    [SerializeField] Sprite _settingsIcon;
    [SerializeField] Vector3 _iconPosition;
    UISkill _inputItem;
    //
    [SerializeField] AudioMixer _masterMixer;
    [SerializeField] Slider _soundsSlider, _musicSlider;
    [SerializeField] TextMeshProUGUI t_soundsValue, t_musicValue;
    float _soundsStartValue = -1f, _musicStartValue = -1f;

    [SerializeField] Toggle _toggle;
    [SerializeField] TMP_Dropdown _resolutionsDropdown;
    int _initialResolutionIndex = -1, _fullscreenValue = 0;

    private void Awake() {
        _menuState = _menuParent.activeInHierarchy;
        if(_menuState) OpenMenu();
    }

    // Start is called before the first frame update
    void Start()
    {
        //Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
        SetUISettingsKey();
        _uiEscHotkey.action.performed += PlaySettingsButtonAnimation;
        _gameplayEscHotkey.action.performed += PlaySettingsButtonAnimation;
        //Debug.Log(_inputItem.GetItemSize());
        _soundsStartValue = PlayerPrefs.GetFloat("SoundsVolume", 50);
        _musicStartValue = PlayerPrefs.GetFloat("MusicVolume", 50);
        
        SetSliders();
        _fullscreenValue = PlayerPrefs.GetInt("FullScreen", 0);
        if(_fullscreenValue == 1) _screenController.SetFullScreen();
        else _screenController.SetWindowed();
        _toggle.isOn = _screenController.Fullscreen;
        _resolutionsDropdown.options = new List<TMP_Dropdown.OptionData>(Screen.resolutions.Length);
        _initialResolutionIndex = PlayerPrefs.GetInt("InitialResolutionIndex", -1);
        if(_initialResolutionIndex == -1)
        {
            int currentRes = Screen.resolutions.Length - 1;
            for (int i = 0; i < Screen.resolutions.Length; i++)
            {
                var res = Screen.resolutions[i];
                var text = $"{res.width} x {res.height}  {res.refreshRate}Hz";
                TMP_Dropdown.OptionData item = new(text);
                _resolutionsDropdown.options.Add(item);
                if(res.width == Screen.currentResolution.width && res.height == Screen.currentResolution.height) currentRes = i;
            }
            _initialResolutionIndex = currentRes;
        }
        _resolutionsDropdown.SetValueWithoutNotify(_initialResolutionIndex);
        Debug.Log(_resolutionsDropdown.options.Count);
    }

    void SetUISettingsKey()
    {
        if(_uiSkillsManager == null) return;
        _inputItem = _uiSkillsManager.SetInputSkill(_uiEscHotkey, _settingsIcon);
        var itemTransform = _inputItem.transform;
        itemTransform.SetParent(_uiSkillsManager.transform, false);
        itemTransform.localPosition = _iconPosition;
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
        _inputItem?.Interact();
        if(_menuState) CloseMenu();
    }

    public void OpenMenu()
    {
        _toggle.isOn = Screen.fullScreen;
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
        if(_pauseMenuParent != null)_menuController.SwitchCurrentMenu(_pauseMenuParent);
        //TimeScaleManager.ForceTimeScale(1f);
    }

    //function called by the toggle on the ui
    public void ToggleFullscreen()
    {
        //Debug.Log("FUllscreen!!! :)");
        int fullscreenValue = _screenController.Fullscreen ? 1 : 0;
        PlayerPrefs.SetInt("FullScreen", fullscreenValue);
        _screenController.ToggleFullscreen();
    }

    //function called by the dropdown on the ui
    public void SetResolution(int index)
    {
        var res = Screen.resolutions[index];
        _screenController.SetResolution(res.width, res.height, res.refreshRate);
        PlayerPrefs.SetInt("ResolutionIndex", index);
        //Debug.Log("Dropdown option selected at:  " + index + "  \n " + _resolutionsDropdown.options[index].text);
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
 