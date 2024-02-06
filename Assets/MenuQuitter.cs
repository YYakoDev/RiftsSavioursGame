using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuQuitter : MonoBehaviour
{
    [SerializeField] GameObject _menuToReturnTo;
    [SerializeField] AudioSource _audio;
    GameObject _currentMenu;
    [SerializeField] AudioClip _closingUISfx;
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Backspace))
        {
            CloseCurrentMenu();
        }
    }


    public void SetCurrentMenu(GameObject menu) => _currentMenu = menu;

    public void CloseCurrentMenu()
    {
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
