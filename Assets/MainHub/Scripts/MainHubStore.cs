using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainHubStore : MonoBehaviour, IInteractable
{
    [SerializeField]Vector3 _buttonOffset;
    bool _alreadyInteracted;
    public Vector3 Offset => _buttonOffset;
    public bool AlreadyInteracted { get => _alreadyInteracted; set => _alreadyInteracted = value; }

    public AudioClip InteractSfx => null;

    public void Interact()
    {
        
    }
}