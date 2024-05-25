using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrototypeResourceSeeker : MonoBehaviour
{
    [SerializeField] CollectingManager _collectingManager;
    List<IResources> _detectedResources = new();
    [SerializeField] float _grabCooldown = 0.5f;
    float _nextGrabTime;
    IResources _currentResource;

    private void FixedUpdate() {
        if(_nextGrabTime >= Time.time) return;
        GrabResources();
    }

    void GrabResources()
    {
        if(_currentResource != null)
        {
            _nextGrabTime = Time.time + _grabCooldown;
            _collectingManager.GrabResource(_currentResource);
        }
    }

    void PickCurrentResource()
    {
        if(_detectedResources.Count >= 1)
            _currentResource = _detectedResources[0];
        else _currentResource = null;
    }


    private void OnTriggerEnter2D(Collider2D other) {
        if(other.TryGetComponent<IResources>(out var resource))
        {
            Debug.Log("Adding Resource");
            _detectedResources.Add(resource);
            PickCurrentResource();
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if(other.TryGetComponent<IResources>(out var resource))
        {
            if(_detectedResources.Contains(resource))
            {
                Debug.Log("Removing Resource");
                _detectedResources.Remove(resource);
                PickCurrentResource();
            }
        }
    }
}
