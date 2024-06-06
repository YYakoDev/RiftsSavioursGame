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
    Vector3 _dropOffset = Vector3.zero;

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
        var currentPosition = transform.position;
        Vector3 previousDropPosition = currentPosition;
        int iterator = 0;
        for (int i = 0; i < _drops.Length; i++)
        {
            var drop = _drops[i];
            if (drop == null) continue;
            if (Random.Range(0, 101) > drop.DropChance) continue;
            GameObject dropGO = DropsPool.GetObjectFromPool();
            var xOffset = (iterator % 2 == 0) ? 0.25f + (0.25f*iterator): -0.25f - (0.25f*iterator);
            var yOffset = Random.Range(-0.25f, 0.25f);
            _dropOffset.x = xOffset;
            _dropOffset.y = yOffset;
            dropGO.transform.position = previousDropPosition + _dropOffset;
            previousDropPosition = dropGO.transform.position;
            dropGO.SetActive(true);
            dropGO.GetComponent<DropPrefab>().Initialize(drop);
            iterator++;
        }
    }

    private void OnValidate() {
        _currentLength = _drops.Length;
    }

}
