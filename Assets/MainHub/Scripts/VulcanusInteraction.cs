using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VulcanusInteraction : MonoBehaviour, IInteractable
{

    private bool _alreadyInteracted;
    [SerializeField] Vector3 _interactButtonOffset;
    public bool AlreadyInteracted { get => _alreadyInteracted; set => _alreadyInteracted = value; }
    public Vector3 Offset => _interactButtonOffset;
    public AudioClip InteractSfx => null;
    public void Interact()
    {
        //Open the crafting menu here
        Debug.Log("Opening Forge");
    }
    void CloseInteraction()
    {
        //call this when closing the crafting menu
        _alreadyInteracted = false;
    }
}
