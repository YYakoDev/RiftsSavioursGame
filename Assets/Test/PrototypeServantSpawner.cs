using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrototypeServantSpawner : MonoBehaviour
{
    [SerializeField] PrototypeServant _servantPrefab;
    // Start is called before the first frame update
    void Start()
    {
        Instantiate(_servantPrefab);
        Instantiate(_servantPrefab);
        Instantiate(_servantPrefab);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
