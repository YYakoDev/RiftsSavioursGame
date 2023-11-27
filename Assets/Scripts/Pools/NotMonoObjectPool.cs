using System;
using UnityEngine;

public class NotMonoObjectPool
{
    int _amountToPool;
    GameObject _objToPool;
    Transform _parent;
    GameObject[] _pooledObjs;
    bool _resizable;

    public NotMonoObjectPool(int amountToPool, GameObject objectToPool, Transform parent, bool isResizable)
    {
        _amountToPool = amountToPool;
        _objToPool = objectToPool;
        _parent = parent;
        _resizable = isResizable;
        _pooledObjs = new GameObject[amountToPool];
        PoolObjects();
    }

    void PoolObjects()
    {
        for (int i = 0; i < _amountToPool; i++)
        {
            CreateNew(i);
        }
    }

    public GameObject GetObjectFromPool()
    {
        for (int i = 0; i < _pooledObjs.Length; i++)
        {
            if(_pooledObjs[i].activeInHierarchy)continue;
            return _pooledObjs[i];
        }
        if(_resizable)
        {
            Array.Resize<GameObject>(ref _pooledObjs, _amountToPool++);
            CreateNew(_amountToPool);
            return _pooledObjs[_amountToPool];
        }
        return null;
    }

    void CreateNew(int indexToAdd)
    {
        GameObject obj = GameObject.Instantiate(_objToPool, Vector3.zero, Quaternion.identity);
        obj.transform.SetParent(_parent);
        obj.SetActive(false);
        _pooledObjs[indexToAdd] = obj;
    }
}
