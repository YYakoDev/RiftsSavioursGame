using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldGenerator : MonoBehaviour
{
    [SerializeField] ChunkGenerator _chunkGenerator;
    [SerializeField] ResourcePool _resourcePool;
    // Start is called before the first frame update
    void Start()
    {
        _resourcePool.gameObject.SetActive(false);
        _chunkGenerator.gameObject.SetActive(true);
        _chunkGenerator.StartCreation();
        _resourcePool.gameObject.SetActive(true);
    }
}
