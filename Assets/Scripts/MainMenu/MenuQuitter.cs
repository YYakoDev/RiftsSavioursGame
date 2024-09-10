using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MenuQuitter : MonoBehaviour
{
    [SerializeField] GameObject _menuToReturnTo;
    [SerializeField] AudioSource _audio;
    [SerializeField] InputActionReference _escapeInput;
    GameObject _currentMenu;
    [SerializeField] AudioClip _closingUISfx;

    private void Start() {
        _escapeInput.action.performed += CloseCurrentMenu;
    }

    private void OnDestroy() {
        _escapeInput.action.performed -= CloseCurrentMenu;
    }

    public void SetCurrentMenu(GameObject menu) => _currentMenu = menu;

    public void CloseCurrentMenu(InputAction.CallbackContext obj)
    {
        Debug.Log("Closing menu");
        if(_currentMenu != null && !_menuToReturnTo.activeInHierarchy)PlayCloseSound();
        _menuToReturnTo.SetActive(true);
        _currentMenu?.SetActive(false);
    }

    void PlayCloseSound()
    {
        if(_audio == null) return;
        _audio.PlayWithVaryingPitch(_closingUISfx);
    }
}
