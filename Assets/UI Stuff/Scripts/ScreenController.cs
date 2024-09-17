using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenController : MonoBehaviour
{
    public static event Action OnScreenChange;
    bool _fullscreen = true;
    public bool Fullscreen => _fullscreen;
    private void Awake() {
        _fullscreen = Screen.fullScreen;
    }

    private void Start() {
        _fullscreen = Screen.fullScreen;
    }

    public void ToggleFullscreen()
    {
        _fullscreen = !_fullscreen;
        if(_fullscreen) SetFullScreen();
        else SetWindowed();
    }

    public void SetWindowed()
    {
        _fullscreen = false;
        Screen.fullScreen = _fullscreen;
        Screen.fullScreenMode = FullScreenMode.Windowed;
        OnScreenChange?.Invoke();
    }

    public void SetFullScreen()
    {
        _fullscreen = true;
        Screen.fullScreen = _fullscreen;
        Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
        OnScreenChange?.Invoke();
    }

    public void SetResolution(int width, int height, int refreshRate)
    {
        Screen.SetResolution(width, height, Screen.fullScreenMode, refreshRate);
        OnScreenChange?.Invoke();
    }
}
