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
        _audio?.PlayWithVaryingPitch(_keyPressSfx);
        _hotkeyPrefabInstance.ShakeAnim();
        _interactableInterface.Interact();
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.TryGetComponent<IInteractable>(out _interactableInterface))
        {
            Transform collidedObj = other.transform;
            _interactableObject = collidedObj.gameObject;
            _hotkeyPrefabInstance.ChangeState(true);
            _hotkeyPrefabInstance.Self.transform.SetParent(collidedObj);
            _hotkeyPrefabInstance.Self.transform.position = collidedObj.position;
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
