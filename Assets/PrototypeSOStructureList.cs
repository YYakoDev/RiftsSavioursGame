using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Prototypes/StructureList")]
public class PrototypeSOStructureList : ScriptableObject
{
    [SerializeField] SOPrefabSpawner[] _structuresList = new SOPrefabSpawner[0];

    public SOPrefabSpawner GetRandomStructure()
    {
        var prefabSpawner = _structuresList[Random.Range(0, _structuresList.Length)];
        
        if(prefabSpawner.OriginalPrefab.TryGetComponent<PrototypeStructure>(out var component))
        {
            return prefabSpawner;
        }else
        {
            Debug.LogError("You have added a non structure prefab spawner");
            return null;
        }
    }
}
