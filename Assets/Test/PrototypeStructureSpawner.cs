using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrototypeStructureSpawner : MonoBehaviour
{
    [SerializeField] PrototypeSOStructureList _structureList;

    private void Start() {
        _structureList.GetRandomStructure()?.Spawn(transform.position);
    }
}
