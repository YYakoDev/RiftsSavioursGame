using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrototypeEnemyConversion : MonoBehaviour
{
    [SerializeField] CollectingManager _collectingManager;
    [SerializeField] LayerMask _resourceLayer;
    [SerializeField] EnemyWaveSpawner _enemySpawner;
    [SerializeField] SOEnemy _stoneGolem, _ent;
    [SerializeField] Transform _otherSideParent;
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
            if(i >= 1) break;
            resourceLogic = results[i].GetComponent<Resource>();
        }
        if(resourceLogic == null) return;
        Debug.Log("Detected  " + resourceLogic.name);


        if(resourceLogic.CurrentHealth <= 1)
        {
            switch(resource.ResourceType)
            {
                case ResourcesTypes.Ore:
                    ConvertResourceToEnemy(_stoneGolem, resource.ResourcePosition);
                    break;
                case ResourcesTypes.Wood:
                    ConvertResourceToEnemy(_ent, resource.ResourcePosition);
                    break;
                default:
                    ConvertResourceToEnemy(_ent, resource.ResourcePosition);
                    break;
            }
        }
    }

    void ConvertResourceToEnemy(SOEnemy enemy, Vector3 position)
    {
        Debug.Log("SpawningEnemy");
        var obj = _enemySpawner.CreateEnemy(enemy);
        if(obj == null) return;
        obj.transform.position = position;
        obj.transform.SetParent(_otherSideParent);
    }

    private void OnDestroy() {
        _collectingManager.onResourceInteraction -= CheckResource;
    }
}
