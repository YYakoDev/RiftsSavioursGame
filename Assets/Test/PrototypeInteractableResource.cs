using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrototypeInteractableResource : MonoBehaviour, IInteractable
{
    private bool _alreadyInteracted;
    [SerializeField] Vector3 _offset;
    [SerializeField] AudioClip _audioClip;
    [SerializeField] Animator _animator;
    [SerializeField] Dropper _dropper;
    public bool AlreadyInteracted { get => _alreadyInteracted; set => _alreadyInteracted = value; }

    public Vector3 Offset => _offset;

    public AudioClip InteractSfx => _audioClip;

    public void Interact()
    {
        CameraShake.Shake(1.5f);
        _animator.Play("Death");
        _dropper.Drop();
    }
}
