using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManuallySpawnedEnemy : MonoBehaviour
{
    [SerializeField]SOEnemy _data;
    [SerializeField]Transform _target;
    [SerializeField] EnemyBrain _brain;
    private void Awake() {
        if(_data == null) gameObject.SetActive(false);
        if(_brain == null) _brain = GetComponent<EnemyBrain>();
        if(_target == null) _target = GameObject.FindGameObjectWithTag("Player").transform;
        InitializeBrain(_data, _target);
    }

    public void Initialize(SOEnemy data, Transform target)
    {
        _data = data;
        _target = target;
        _brain.Initialize(_data, _target);
    }
    void InitializeBrain(SOEnemy data, Transform target)
    {
        _brain.Initialize(_data, _target);
    }
}
