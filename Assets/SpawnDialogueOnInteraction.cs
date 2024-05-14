using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnDialogueOnInteraction : MonoBehaviour, IInteractable
{
    [SerializeField] DialogueObject _dialoguePrefab;
    [SerializeField, TextArea] string _dialogueText;
    DialogueObject _dialogueInstance;
    private bool _alreadyInteracted;
    private Vector3 _offset;

    public bool AlreadyInteracted { get => _alreadyInteracted; set => _alreadyInteracted = value; }

    public Vector3 Offset => _offset;

    public AudioClip InteractSfx => null;

    public void Interact()
    {
        if(_dialogueInstance == null) CreatePrefab();
        _dialogueInstance.transform.position = transform.position + Offset;
        _dialogueInstance.SetDialogue(_dialogueText, 1f, () => 
        {
            _alreadyInteracted = false;
        });
    }

    void CreatePrefab()
    {
        _dialogueInstance = Instantiate(_dialoguePrefab);
    }
}
