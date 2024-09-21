using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;
public class InteractableSeeker : MonoBehaviour
{
    GameObject _interactableObject;
    IInteractable _interactableInterface;
    [SerializeField]InputActionReference _interactKey;
    [SerializeField]InteractableHotkey _hotkeyPrefab;
    InteractableHotkey _hotkeyPrefabInstance;
    [SerializeField] AudioSource _audio;
    [SerializeField] AudioClip _keyPressSfx;
    // Start is called before the first frame update
    void Start()
    {
        _hotkeyPrefabInstance = Instantiate(_hotkeyPrefab);
        _hotkeyPrefabInstance.Text.SetText(_interactKey.action.GetBindingDisplayString());
        _hotkeyPrefabInstance.Self.SetActive(false);
      
        _interactKey.action.performed += Interact;
        
    }
    void Interact(InputAction.CallbackContext obj)
    {
        if(_interactableInterface == null) return;
        if(_interactableInterface.AlreadyInteracted) return;
        AudioClip sfx = (_interactableInterface.InteractSfx == null) ? _keyPressSfx : _interactableInterface.InteractSfx;
        _audio?.PlayWithVaryingPitch(sfx);
        _hotkeyPrefabInstance.ShakeAnim(DisableInteractableItem);
        _interactableInterface.AlreadyInteracted = true;
        _interactableInterface.Interact();
    }

    void DisableInteractableItem()
    {
        _interactableObject = null;
        _interactableInterface = null;
        _hotkeyPrefabInstance.ChangeState(false);
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.TryGetComponent<IInteractable>(out var interactableInterface))
        {
            if(interactableInterface.AlreadyInteracted) return;
            _interactableInterface = interactableInterface;
            Transform collidedObj = other.transform;
            _interactableObject = collidedObj.gameObject;
            _hotkeyPrefabInstance.ChangeState(true);
            _hotkeyPrefabInstance.Text.SetText(_interactKey.action.GetBindingDisplayString());
            //_hotkeyPrefabInstance.Self.transform.SetParent(collidedObj);
            _hotkeyPrefabInstance.Self.transform.position = collidedObj.position + _interactableInterface.Offset;
        }
    }
    private void OnTriggerExit2D(Collider2D other) {
        if(other.gameObject == _interactableObject)
        {
            DisableInteractableItem();
            //_hotkeyPrefabInstance.Self.transform.SetParent(transform);
        }
    }

    private void OnDestroy() {
        _interactKey.action.performed -= Interact;
    }
}
