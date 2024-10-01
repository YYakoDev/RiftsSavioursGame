using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrototypeStructure : MonoBehaviour, IInteractable
{
    [SerializeField]Vector3 _offset;
    bool _alreadyInteracted;

    public bool AlreadyInteracted { get => _alreadyInteracted; set => _alreadyInteracted = value; }
    public Vector3 Offset => _offset;
    public AudioClip InteractSfx => null;

    public virtual void Interact()
    {
        
    }
}
