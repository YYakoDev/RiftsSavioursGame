using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrototypeINRunAstrolabe : MonoBehaviour, IInteractable
{
    [SerializeField] PrototypeGameState _gameState;
    [SerializeField]Vector3 _offset;
    bool _alreadyInteracted;

    public bool AlreadyInteracted { get => _alreadyInteracted; set => _alreadyInteracted = value; }
    public Vector3 Offset => _offset;
    public AudioClip InteractSfx => null;

    private void OnEnable() {
        _alreadyInteracted = false;
    }

    public void Interact()
    {
        //_gameState.ChangeWorld();
    }
}
