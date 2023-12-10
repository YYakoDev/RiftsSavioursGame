using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectAndComponentPool<TComponent> 
where TComponent : Component 
{
    GameObject _objToPool;
    int _amountToPool;
    Transform _parent;
    bool _isResizable;
    Dictionary<GameObject, TComponent> _objsAndComponents = new();
    NotMonoObjectPool _pool;
    GameObject[] _pooledGameObjs;
    KeyValuePair<GameObject, TComponent> _nullPair;

    public ObjectAndComponentPool(int amountToPool, GameObject objToPool, Transform parent, bool isResizable)
    {
        if(!objToPool.TryGetComponent<TComponent>(out var component))
        {
            Debug.LogError("Couldnt get the component in the GameObject to pool:   " + objToPool.name); 
            return;
        }
        
        SetNullPair();

        _amountToPool = amountToPool;
        _objToPool = objToPool;
        _isResizable = isResizable;
        _parent = parent;

        if(_pool == null) _pool = new(amountToPool, objToPool, parent, isResizable);
        _pooledGameObjs = _pool.PooledObjects;
        
        AddComponentsToPool();
    }
    void AddComponentsToPool()
    {
        foreach(GameObject obj in _pooledGameObjs)
        {
            if(obj.TryGetComponent<TComponent>(out var component))
            {
                _objsAndComponents.Add(obj, component);
            }else
            {
                var addedComponent = obj.AddComponent<TComponent>();
                _objsAndComponents.Add(obj, addedComponent);
            }

        }
    }

    public KeyValuePair<GameObject, TComponent> GetObjectWithComponent()
    {
        foreach(var pairedObj in _objsAndComponents)
        {
            if(pairedObj.Key.activeInHierarchy) continue;
            return pairedObj;
        }

        if(_isResizable)
        {
            return CreateNew();
        }else return _nullPair;
    }

    KeyValuePair<GameObject, TComponent> CreateNew()
    {
        GameObject obj = GameObject.Instantiate(_objToPool, Vector3.zero, Quaternion.identity);
        obj.transform.SetParent(_parent);
        obj.SetActive(false);
        var addedComponent = obj.AddComponent<TComponent>();
        _objsAndComponents.Add(obj, addedComponent);
        KeyValuePair<GameObject, TComponent> pairedObj = new(obj, addedComponent);
        return pairedObj;

    }

    void SetNullPair()
    {
        GameObject nullObj = null;
        TComponent nullComp = null;
        _nullPair = new(nullObj, nullComp);
    }
}
