using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropsPool : MonoBehaviour
{
    private static NotMonoObjectPool dropsPool;
    private static DropPrefab _dropPrefab;

    private void Awake() {
        if(_dropPrefab == null) _dropPrefab = Resources.Load<DropPrefab>("DropPrefab/DropPrefab");
        if(dropsPool == null) dropsPool = new(250, _dropPrefab.gameObject, transform, true);
    }
    public static GameObject GetDropPrefab()
    {
        return dropsPool.GetObjectFromPool();
    }

    private void OnDestroy() {
        dropsPool = null;
    }
}
