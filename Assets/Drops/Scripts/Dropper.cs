using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
public class Dropper : MonoBehaviour
{
    private static DropPrefab _dropPrefab;
    private static NotMonoObjectPool DropsPool;
    [SerializeField] Drop[] _drops = new Drop[0];
    int _currentLength = 0;
    Vector3 _dropOffset = Vector3.right / 2;

    private void Awake() {
        if(_dropPrefab == null) _dropPrefab = Resources.Load<DropPrefab>("DropPrefab/DropPrefab");
        if(DropsPool == null) DropsPool = new(250, _dropPrefab.gameObject, null, true);
    }

    public void AddDrop(Drop drop)
    {
        _currentLength++;
        Array.Resize<Drop>(ref _drops, _currentLength);
        _drops[_currentLength - 1] = drop;
    }

    public void Clear()
    {
        _currentLength = 0;
        Array.Clear(_drops, 0, _drops.Length);
    }

    public void Drop()
    {
        Vector3 previousDropPosition = transform.position;
        foreach(Drop drop in _drops)
        {
            if(Random.Range(0,101) > drop.DropChance)continue;
            if(drop == null) continue;
            GameObject dropGO = DropsPool.GetObjectFromPool();
            Vector3 randomYOffset = Vector3.zero;
            randomYOffset.y = Random.Range(-0.25f, 0.25f);
            dropGO.transform.position = previousDropPosition + _dropOffset + randomYOffset;
            previousDropPosition = dropGO.transform.position;

            dropGO.SetActive(true);
            dropGO.GetComponent<DropPrefab>().Initialize(drop);
        }
    }

    private void OnValidate() {
        _currentLength = _drops.Length;
    }

}
