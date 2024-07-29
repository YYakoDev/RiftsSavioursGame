using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrototypeChunkSpawner : MonoBehaviour
{
    [SerializeField] Grid _chunkParent;
    [SerializeField] ChunkTileMap _referenceChunk, _storeChunk, _fightChunk, _craftingChunk;
    Vector3 _chunkReferenceSize;
    Vector3 _referenceChunkPos, _storeChunkPos, _fightChunkPos, _craftingChunkPos;


    // Start is called before the first frame update
    void Start()
    {
        _referenceChunk.GetTilemap().CompressBounds();
        _chunkReferenceSize = _referenceChunk.GetTilemap().size;
        _referenceChunkPos = _referenceChunk.transform.position;
        CreateChunks();
    }

    void CreateChunks()
    {
        _storeChunkPos = _referenceChunkPos + new Vector3(_chunkReferenceSize.x, _chunkReferenceSize.y * 2f);
        _fightChunkPos = _referenceChunkPos + new Vector3(_chunkReferenceSize.x * 2f, _chunkReferenceSize.y * 0f);
        _craftingChunkPos = _referenceChunkPos + new Vector3(_chunkReferenceSize.x * -2f, _chunkReferenceSize.y * 2f);
        SpawnChunk(_storeChunk, _storeChunkPos);
        SpawnChunk(_fightChunk, _fightChunkPos);
        SpawnChunk(_craftingChunk, _craftingChunkPos);
    }

    public Vector3 GetStorePosition() => _storeChunkPos;
    public Vector3 GetFightPosition() => _fightChunkPos;
    public Vector3 GetCraftingPosition() => _craftingChunkPos;

    void SpawnChunk(ChunkTileMap prefab, Vector3 pos)
    {
        var transform = Instantiate(prefab, pos, Quaternion.identity).transform;
        transform.SetParent(_chunkParent.transform);
    }
}
