using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrototypeResourceCollection : MonoBehaviour
{
    [SerializeField] SOPlayerStats _playerStats;
    [SerializeField] LayerMask _resourceLayer;
    [SerializeField] Canvas _worldCanvas;
    [SerializeField] PrototypeResourceProgressBar _progressBarPrefab;
    Collider2D[] _resourcesDetected = new Collider2D[10];
    int _detectionsCount;
    Timer _detectionTimer;
    Dictionary<Resource, PrototypeResourceProgressBar> _resourcesToInteract = new();

    private void Awake() {
        _detectionTimer = new(_playerStats.InteractCooldown, true);
        _detectionTimer.onEnd += DetectResources;
    }

    void DetectResources()
    {
        //_worldCanvas.transform.position = transform.position;
        _detectionsCount = Physics2D.OverlapCircleNonAlloc(transform.position, _playerStats.CollectingRange, _resourcesDetected, _resourceLayer);
        for (int i = 0; i < _detectionsCount; i++)
        {
            var resourceColl = _resourcesDetected[i];
            if(resourceColl == null) continue;
            var resource = resourceColl.GetComponent<Resource>();
            if(resource == null) continue;
            AddResource(resource);
        }
    }

    void AddResource(Resource resource)
    {
        if(_resourcesToInteract.TryGetValue(resource, out var progressBar))
        {
            DamageResource(resource, progressBar);
        }else
        {
            var progressBarInstance = Instantiate(_progressBarPrefab);
            progressBarInstance.transform.SetParent(_worldCanvas.transform, false);
            _resourcesToInteract.Add(resource, progressBarInstance);
            DamageResource(resource, progressBarInstance);
        }
    }

    void DamageResource(Resource resource, PrototypeResourceProgressBar progressBar)
    {
        resource.TakeDamage(0);
        progressBar.transform.position = resource.transform.position;
        
        if(progressBar.SetValue(resource))
        {
            resource.Die();
            //resource is about to die
            _resourcesToInteract.Remove(resource);
            progressBar.gameObject.SetActive(false);
            return;
        }
    }

    private void Update() {
        _detectionTimer.UpdateTime();
    }

    private void OnDestroy() {
        _detectionTimer.onEnd -= DetectResources;
    }
}
