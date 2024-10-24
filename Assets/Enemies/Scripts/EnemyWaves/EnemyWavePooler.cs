using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWavePooler : MonoBehaviour
{
    ObjectAndComponentPool<EnemyBrain> _pool;
    [SerializeField] EnemyBrain _enemyPrefab;
    [SerializeField] int _amountToPool = 400;
    private void Awake() {
        _pool = new(_amountToPool, _enemyPrefab.gameObject, transform, true);
    }

    public KeyValuePair<GameObject,EnemyBrain> GetPooledObject()
    {
        return _pool.GetObjectWithComponent();
    }
}