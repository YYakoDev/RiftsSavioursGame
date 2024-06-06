using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Prototypes/PrefabSpawner")]
public class SOPrefabSpawner : ScriptableObject
{
    [SerializeField]GameObject _prefab;
    GameObject _instance;

    public GameObject OriginalPrefab => _prefab;

    public GameObject Spawn(Vector3 position)
    {
        return Instantiate(_prefab, position, Quaternion.identity);
    }
}
