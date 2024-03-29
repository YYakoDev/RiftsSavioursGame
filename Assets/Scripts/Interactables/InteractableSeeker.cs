using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class InteractableSeeker : MonoBehaviour
{
    GameObject _interactableObject;
    IInteractable _interactableInterface;
    KeyInput _interactKey;
    [SerializeField]InteractableHotkey _hotkeyPrefab;
    InteractableHotkey _hotkeyPrefabInstance;
    [SerializeField] AudioSource _audio;
    [SerializeField] AudioClip _keyPressSfx;
    // Start is called before the first frame update
    void Start()
    {
        _hotkeyPrefabInstance = Instantiate(_hotkeyPrefab);
        _hotkeyPrefabInstance.Self.transform.SetParent(transform);
        _hotkeyPrefabInstance.Self.SetActive(false);
        _interactKey = YYInputManager.GetKey(KeyInputTypes.Interact);
        _interactKey.OnKeyPressed += Interact;
        _hotkeyPrefabInstance.Text.text = _interactKey.PrimaryKey.ToString();
    }
    void Interact()
    {
        if(_interactableInterface == null) return;
        if(_interactableInterface.AlreadyInteracted) return;
        AudioClip sfx = (_interactableInterface.InteractSfx == null) ? _keyPressSfx : _interactableInterface.InteractSfx;
        _audio?.PlayWithVaryingPitch(sfx);
        _hotkeyPrefabInstance.ShakeAnim(DisableInteractIcon);
        _interactableInterface.AlreadyInteracted = true;
        _interactableInterface.Interact();
    }

    void DisableInteractIcon()
    {
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
            _hotkeyPrefabInstance.Self.transform.SetParent(collidedObj);
            _hotkeyPrefabInstance.Self.transform.position = collidedObj.position + _interactableInterface.Offset;
        }
    }
    private void OnTriggerExit2D(Collider2D other) {
        if(other.gameObject == _interactableObject)
        {
            _interactableObject = null;
            _interactableInterface = null;
            _hotkeyPrefabInstance.ChangeState(false);
            //_hotkeyPrefabInstance.Self.transform.SetParent(transform);
        }
    }

    private void OnDestroy() {
        _interactKey.OnKeyPressed -= Interact;
    }
}
