using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourcePool : MonoBehaviour
{
    [SerializeField] GameObject _resourcePrefab;
    [SerializeField] ChunkGenerator _chunkGenerator;
    [SerializeField] int _amountToPool = 10;
    ObjectAndComponentPool<Resource> _resourcesPool;

    private void Awake()
    {
        _resourcesPool = new(_amountToPool, _resourcePrefab, transform, true, SkipCondition);
        ResourcePointer.OnSignal += OnPointerSignal;
    }


    GameObject SpawnResource(ResourceInfo info, Vector3 position)
    {
        var resource = _resourcesPool.GetObjectWithComponent();
        if(resource.Key == null) return null;
        resource.Key.transform.position = position;
        resource.Key.transform.SetParent(_chunkGenerator.GetChunkFromWorldPosition(position));
        resource.Value.SetResourceInfo(info);

        resource.Key.SetActive(true);
        return resource.Key;
    }

    void OnPointerSignal(ResourcePointer pointer)
    {
        if(pointer.SpawnedResource != null && pointer.SpawnedResource.activeInHierarchy 
        && pointer.SpawnedResource.transform.position == pointer.Position) return;
        pointer.SpawnedResource = SpawnResource(pointer.Info, pointer.Position);
    }

    private void OnDestroy()
    {
        ResourcePointer.OnSignal -= OnPointerSignal;
    }
    
    bool SkipCondition(Resource r)
    {
        return (r.IsBroken || (r.CurrentHealth > 0 && r.CurrentHealth != r.MaxHealth));
    }
}
