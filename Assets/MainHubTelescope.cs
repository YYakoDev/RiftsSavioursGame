using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainHubTelescope : MonoBehaviour, IInteractable
{
    [SerializeField]MainHubCharacterSelector _charSelector;
    [SerializeField]Vector3 _buttonOffset;
    bool _alreadyInteracted;

    //properties
    public bool AlreadyInteracted { get => _alreadyInteracted; set => _alreadyInteracted = value; }
    public Vector3 Offset => _buttonOffset;
    public AudioClip InteractSfx => null;

    private void Start() {
        _charSelector.onMenuClose += CharSelectorClosing;
    }

    public void Interact()
    {
        //Open Character Selector
        _charSelector.Open();
    }

    void CharSelectorClosing()
    {
        //Method to call when the closing of the character selector is invoked
        _alreadyInteracted = false;
    }

    private void OnDestroy() {
        _charSelector.onMenuClose -= CharSelectorClosing;
    }
}
