using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterIconSetup : MonoBehaviour
{
    [SerializeField]Image _icon;
    // Start is called before the first frame update
    void Start()
    {
        PlayerManager.onCharacterChange += UpdateIcon;
        UpdateIcon();
    }

    void UpdateIcon()
    {
        if(PlayerManager.SelectedChara != null)
        {
            _icon.sprite = PlayerManager.SelectedChara.Sprite;
        }
    }

    private void OnDestroy() {
        PlayerManager.onCharacterChange -= UpdateIcon;
    }

    
}
