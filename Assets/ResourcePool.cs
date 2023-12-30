using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourcePool : MonoBehaviour
{
    [SerializeField] GameObject _resourcePrefab;
    [SerializeField] ChunkGenerator _chunkGenerator;
    [SerializeField] int _amountToPool = 10;
    [SerializeField] bool _isResizable = true;
    ObjectAndComponentPool<Resource> _resourcesPool;

    private void Awake()
    {
        _resourcesPool = new(_amountToPool, _resourcePrefab, transform, _isResizable);
    }

    void OnEnable()
    {
        ResourcePointer.OnResourceSignal += SpawnResource;
    }

    void SpawnResource(ResourceInfo info, Vector3 position)
    {
        var resource = _resourcesPool.GetObjectWithComponent();
        if(resource.Key == null) return;
        resource.Key.transform.position = position;
        resource.Key.transform.SetParent(_chunkGenerator.GetChunkFromWorldPosition(position));
        resource.Value.SetResourceInfo(info);

        resource.Key.gameObject.SetActive(true);

    }

    private void OnDisable()
    {
        ResourcePointer.OnResourceSignal -= SpawnResource;
    }
}
