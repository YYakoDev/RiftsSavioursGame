using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManualEnemySpawner : MonoBehaviour
{
    [SerializeField] SOEnemy _data;
    [SerializeField] ManuallySpawnedEnemy _enemyPrefab;
    [SerializeField] Transform _player;

    public void SpawnEnemy()
    {
        if(_data == null || _enemyPrefab == null || _player == null)
        {
            Debug.Log("<color=red> you need to assign some references before spawning an enemy </color>");
            return;
        }
        var instance = Instantiate(_enemyPrefab, transform);
        instance.transform.SetParent(null);
        instance.Initialize(_data, _player);
    }
}
