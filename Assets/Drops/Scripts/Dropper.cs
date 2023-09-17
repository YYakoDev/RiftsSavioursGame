using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dropper : MonoBehaviour
{
    private static DropPrefab _dropPrefab;
    [SerializeField]Drop[] _drops;
    Vector3 _dropOffset = Vector3.right/2;

    private void Start() {
        _dropPrefab = Resources.Load<DropPrefab>("DropPrefab");
    }

    public void Drop()
    {
        Vector3 previousDropPosition = transform.position;
        foreach(Drop drop in _drops)
        {
            if(Random.Range(0,101) > drop.DropChance)continue;

            GameObject dropGO = Instantiate(_dropPrefab.gameObject, previousDropPosition + _dropOffset, Quaternion.identity);
            previousDropPosition = dropGO.transform.position;
    
            dropGO.GetComponent<DropPrefab>().Initialize(drop);
        }
    }

}
