using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableSeeker : MonoBehaviour
{
    GameObject _interactableObject;
    IInteractable _interactableInterface;
    KeyInput _interactKey;
    // Start is called before the first frame update
    void Start()
    {
        _interactKey = YYInputManager.GetKey(KeyInputTypes.Interact);
        _interactKey.OnKeyPressed += Interact;
    }

    private void Update() {
        if(_interactableObject != null)
        {
            Debug.Log("You can interact with the object:   " + _interactableObject.name);
        }
    }

    void Interact()
    {
        Debug.Log("Interacting");
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.TryGetComponent<IInteractable>(out _interactableInterface))
        {
            _interactableObject = other.gameObject;
        }
    }
    private void OnTriggerExit2D(Collider2D other) {
        if(other.gameObject == _interactableObject)
        {
            _interactableObject = null;
            _interactableInterface = null;
        }
    }

    private void OnDestroy() {
        _interactKey.OnKeyPressed -= Interact;
    }
}
