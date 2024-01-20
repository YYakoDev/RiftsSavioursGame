using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;
public class ChunkGenerator : MonoBehaviour
{
    //creo que te vendria bien usar un dictionary almacenando como key: un vector2int y como value: un boolean de si la posicion esta ocupada o no

    //List<Vector2Int> _spawnedChunkPositions = new List<Vector2Int>();
    [SerializeField]World _currentWorld;
    Dictionary<Vector2Int,ChunkTileMap> _spawnedChunks = new();

    [Header("Chunk Reference")]
    [SerializeField]ChunkTileMap _chunkReference;
    Tilemap _chunkRenderer;
    Vector2  _chunkOrigin;
    Vector2Int _referenceSize;
    List<ChunkTileMap> _validChunks = new();

    [Header("References")]
    [SerializeField]Grid _gridParent;
    [SerializeField]Transform _playersTransform;
    Vector2Int _playerPositionOnGrid;




    public void StartCreation()
    {
        //failsafe in case you lose the assigned references in the editor
        if(_gridParent == null) _gridParent = new GameObject("gridParent",typeof(Grid)).GetComponent<Grid>();
        if(_playersTransform == null) _playersTransform = GameObject.FindGameObjectWithTag("Player").transform;
        

        //this sets up the values of the reference chunk, and all other chunks must be built like this one
        // BUT REMEMBER that the chunktilemap script draws its gizmos based on a manual value instead of the _referenceSize one, so be careful
        _chunkRenderer = _chunkReference.GetTilemap();
        _chunkRenderer.CompressBounds();
        
        _referenceSize = new Vector2Int((int)_chunkRenderer.size.x, (int)_chunkRenderer.size.y);
        _chunkOrigin = _chunkReference.transform.position;
        //Debug.Log(_referenceSize); // this value determines whether the chunks are valid or not

        var chunks = _currentWorld.Chunks;

        //get the valid chunks, just in case you add a chunk that is bigger or smaller than the others
        foreach(var chunk in chunks)
        {
            if(chunk == null) continue;
            Tilemap tilemap = chunk.GetTilemap();
            tilemap.CompressBounds();
            if((Vector2Int)tilemap.size != _referenceSize) 
            Debug.Log($"<color=#ff0000>REMOVED CHUNK {chunk.name} BECAUSE IT DIDNT MATCH THE REFERENCE SIZE ({_referenceSize}) </color>");

            else _validChunks.Add(chunk);
            
        }
        //spawn a bunch of chunks at the start
        _spawnedChunks.Add(Vector2Int.zero,  _chunkReference); // this position is the same as the chunk used as a reference //SHOULD BE THE SAME*
        SpawnChunksIn8Direction(Vector2Int.zero);
    }

    // Update is called once per frame
    void LateUpdate()
    {
        //CreateChunksBasedOnPlayer();
        CheckPlayerPosition();
        
        //you can put this into a method and invoke it repeating in the start instead of calculating this every frame
        /*Vector2Int playerPositionOnGrid = new Vector2Int //store this positions so you can do an if check if the list contains that position so you dont need to calculate the player's position everytime
        (
            Mathf.RoundToInt(_playersTransform.position.x/_referenceSize.x),
            Mathf.RoundToInt(_playersTransform.position.y/_referenceSize.y)
        );*/

        
        

    }

    void CheckPlayerPosition()
    {
        Vector2Int playerPosOnGridLocal = new Vector2Int
        (
            Mathf.RoundToInt(_playersTransform.position.x/_referenceSize.x),
            Mathf.RoundToInt(_playersTransform.position.y/_referenceSize.y)
        );
        if(_playerPositionOnGrid != playerPosOnGridLocal)
        {
            //_previousPlayerPosition = _playerPositionOnGrid;
            _playerPositionOnGrid = playerPosOnGridLocal;
            //Debug.Log("Player's NEW position on grid:  " + _playerPositionOnGrid);
            TryToSpawnNewChunks();
        }
    }

    void TryToSpawnNewChunks()
    {
        //Debug.Log("Render Distance:   " + _playerPositionOnGrid);
        //Debug.Log($"sign of x {System.MathF.Sign(_playerPositionOnGrid.x)} \n  sign of y {System.Math.Sign(_playerPositionOnGrid.y)} ");
        //spawn chunk on players position
        //_spawnedChunkPositions.Add(_playerPositionOnGrid);
        //SpawnChunk(GetChunkPosition(_playerPositionOnGrid));
        //also spawn chunks in every 8 direction based on player's position on grid and also ONLY SPAWN if the position is NOT occupied
        //SpawnChunksIn8Direction(renderDistance);
        SpawnChunksIn8Direction(_playerPositionOnGrid);
    }

   /* void CreateChunksBasedOnPlayer()
    {
        _playerPositionOnGrid = new Vector2Int
        (
            Mathf.RoundToInt(_playersTransform.position.x/_referenceSize.x),
            Mathf.RoundToInt(_playersTransform.position.y/_referenceSize.y)
        );
        //Vector2Int renderDistance = _playerPositionOnGrid *_chunkRenderDistance;
        //Vector2Int renderDistance = _playerPositionOnGrid *_chunkRenderDistance;
        Vector2Int renderDistance = new Vector2Int
        (
            _playerPositionOnGrid.x + _chunkRenderDistance.x * (int)System.MathF.Sign(_playerPositionOnGrid.x),

            _playerPositionOnGrid.y + _chunkRenderDistance.y * (int)System.MathF.Sign(_playerPositionOnGrid.y)
        );
        if(!_spawnedChunkPositions.Contains(renderDistance))
        {
            
            Debug.Log("Player's position on grid:  " + _playerPositionOnGrid);
            Debug.Log("Render Distance:   " + renderDistance);
            //Debug.Log($"sign of x {System.MathF.Sign(_playerPositionOnGrid.x)} \n  sign of y {System.Math.Sign(_playerPositionOnGrid.y)} ");

            //spawn chunk on players position
            _spawnedChunkPositions.Add(_playerPositionOnGrid);
            SpawnChunk(GetChunkPosition(_playerPositionOnGrid));

            //also spawn chunks in every 8 direction based on player's position on grid and also ONLY SPAWN if the position is NOT occupied
            //SpawnChunksIn8Direction(renderDistance);
            SpawnChunksIn8Direction(_playerPositionOnGrid);
        }
    }*/
    public Transform GetChunkFromWorldPosition(Vector2 position)
    {
        Vector2Int positionOnGrid = new Vector2Int
        (
            Mathf.RoundToInt(position.x/_referenceSize.x),
            Mathf.RoundToInt(position.y/_referenceSize.y)
        );
        if(_spawnedChunks.ContainsKey(positionOnGrid)) return _spawnedChunks[positionOnGrid].transform;
        else
        {
            Debug.Log("No parent found in the chunk:  " + positionOnGrid + " \n original position:  " + position);
            return null;
        }
    }

    Vector2 GetChunkPosition(Vector2Int chunkPositionOnGrid) 
    {
        /*
        if(!_spawnedChunkPositions.ContainsKey(chunkPos))
        {
            _spawnedChunkPositions.Add(chunkPos,true);
        }else
        {
            Debug.Log("<b>You tried to spawn a new chunk in an already existing one at:   </b>" + chunkPos);
        }*/

        Vector2 chunkPosition = _chunkOrigin + (chunkPositionOnGrid * _referenceSize);
        return chunkPosition;
    }

    ChunkTileMap SpawnChunk(Vector2 worldPosition)
    {
        
        //_validChunks[0].CompressBounds();
        /*Vector2 newPosition = new Vector2(
            position.x + Mathf.Abs(_validChunks[0].size.x - _referenceSize.x),
            position.y + Mathf.Abs(_validChunks[0].size.y - _referenceSize.y)
            
        );*/
        //Debug.Log(newPosition);
        int randomChunkIndex = Random.Range(0,_validChunks.Count);

        var chunk = Instantiate(_validChunks[randomChunkIndex], worldPosition, Quaternion.identity);
        chunk.transform.SetParent(_gridParent.transform);
        return chunk;

    }

    void SpawnChunksIn8Direction(Vector2Int spawnPosition)
    {
        Vector2Int[] newChunkPositions = new Vector2Int[8];
        for(int i = 0; i < Directions.eightDirections.Length; i++)
        {
            Vector2Int directionalPosition = Directions.eightDirections[i];
            //ACA ES DONDE DEBERIAS TENER EN CUENTA LA RENDER DISTANCE Y LA DISTANCIA ENTRE EL JUGADOR Y SPAWNEAR CHUNKS A LO LARGO DE ESA DISTANCIA
            Vector2Int newChunkPosition =  directionalPosition + spawnPosition;
            newChunkPositions[i] = newChunkPosition;
            if(!_spawnedChunks.ContainsKey(newChunkPosition))
            {
                var newChunk = SpawnChunk(GetChunkPosition(newChunkPosition));
                newChunk.SetPosition(newChunkPosition);
                newChunk.CheckDistance(spawnPosition);
                _spawnedChunks.Add(newChunkPosition , newChunk);
            }
            LoadChunk(newChunkPosition, true);

        }
        DistanceCheck(spawnPosition);
    }

    void LoadChunk(Vector2Int gridPosition, bool activeState)
    {
        _spawnedChunks[gridPosition].gameObject.SetActive(activeState);
    }

    void DistanceCheck(Vector2Int position)
    {
        foreach(var chunksData in _spawnedChunks)
        {
            if(!chunksData.Value.gameObject.activeInHierarchy) continue;
            chunksData.Value.CheckDistance(position);
        }
    }

    void LoadSpecificChunks(List<Vector2Int> specificChunks)
    {
        foreach(var chunkData in _spawnedChunks)
        {
            if(chunkData.Key == _playerPositionOnGrid)continue;
            foreach(Vector2Int chunkPosition in specificChunks)
            {
                if(chunkData.Key == chunkPosition)
                {
                    chunkData.Value.gameObject.SetActive(true);
                    break;
                }
                else
                {
                    chunkData.Value.gameObject.SetActive(false);
                }
 
            }
        }
    }
}
