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
        _escapeInput.action.performed += CloseMenuWithInput;
    }

    private void OnDestroy() {
        _escapeInput.action.performed -= CloseMenuWithInput;
    }

    public void SetCurrentMenu(GameObject menu) => _currentMenu = menu;


    void CloseMenuWithInput(InputAction.CallbackContext obj)
    {
        CloseCurrentMenu();
    }

    public void CloseCurrentMenu()
    {
        Debug.Log("Closing menu");
        if(_currentMenu != null)
        {
            Debug.Log("Closing SOUND");
            PlayCloseSound();
        }
        _menuToReturnTo.SetActive(true);
        _currentMenu?.SetActive(false);
        _currentMenu = null;
    }

    void PlayCloseSound()
    {
        if(_audio == null) return;
        _audio.PlayWithVaryingPitch(_closingUISfx);
    }
}
