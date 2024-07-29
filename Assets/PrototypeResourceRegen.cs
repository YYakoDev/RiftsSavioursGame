using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrototypeResourceRegen : MonoBehaviour
{
    [SerializeField] CollectingManager _collectingManager;
    [SerializeField] LayerMask _resourceLayer;
    [SerializeField] ResourcePointer _treePrefab, _rockPrefab, _plantPrefab;

    private void Awake() {
        _collectingManager.onResourceInteraction += CheckResource;
    }

    void CheckResource(IResources resource)
    {
        var results = Physics2D.OverlapCircleAll(resource.ResourcePosition, 0.5f, _resourceLayer);
        Resource resourceLogic = null;
        for (int i = 0; i < results.Length; i++)
        {
            Debug.Log(results[i]);
            if (i >= 1) break;
            resourceLogic = results[i].GetComponent<Resource>();
        }
        if (resourceLogic == null) return;
        Debug.Log("Detected  " + resourceLogic.name);
        if (resourceLogic.CurrentHealth <= 1)
        {
            YYExtensions.i.ExecuteEventAfterTime(2f, () =>
            {
                resourceLogic.gameObject.SetActive(false);
                switch(resourceLogic.ResourceType)
                {
                    case ResourcesTypes.Wood:
                        Instantiate(_treePrefab, resourceLogic.transform.position, Quaternion.identity);
                        break;
                    case ResourcesTypes.Ore:
                        Instantiate(_rockPrefab, resourceLogic.transform.position, Quaternion.identity);
                        break;
                    case ResourcesTypes.Herb:
                        Instantiate(_plantPrefab, resourceLogic.transform.position, Quaternion.identity);
                        break;
                }
            });
        }
    }

    private void OnDestroy() {
        _collectingManager.onResourceInteraction -= CheckResource;
    }
}
