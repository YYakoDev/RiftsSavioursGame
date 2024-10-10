using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class SharedBloodObjPool
{
    static bool _initialized = false;
    static BloodSplatterFX _prefab;
    static ObjectAndComponentPool<BloodSplatterFX> _pool;

    static void Init()
    {
        if(_initialized) return;
        _prefab = Resources.Load<BloodSplatterFX>("BloodPrefab");
        _pool = new(200, _prefab.gameObject, null, false);
        _initialized = true;
    }

    public static BloodSplatterFX GetPrefab(SOBloodFX bloodData)
    {
        if(!_initialized) Init();
        var obj = _pool.GetObjectWithComponent();
        obj.Value.Set(bloodData);
        obj.Key.SetActive(true);
        return obj.Value;
    }

    public static void UnloadPool() => _initialized = false;
}
