using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(Tilemap))]
public class ChunkTileMap : MonoBehaviour
{
    public float xOffset = 0.5f;
    public float yOffset = 0f;
    Tilemap _tileMap;
    [SerializeField]ResourcePointer[] _resources;
    public ResourcePointer[] Resources => _resources;


    Vector2Int _positionOnGrid;

    private void Awake() {
        _tileMap = GetTilemap();
    }

    public Tilemap GetTilemap()
    {
        if(_tileMap == null) _tileMap = GetComponent<Tilemap>();
        return _tileMap;
    }

    public void SetPosition(Vector2Int pos)
    {
        _positionOnGrid = pos;
    }
    public void CheckDistance(Vector2Int playerPosOnGrid)
    {
        if(Vector2Int.Distance(_positionOnGrid, playerPosOnGrid) > 3) gameObject.SetActive(false);
    }

    private void OnDrawGizmos() {
        if(Application.isPlaying) return;
        Vector2 chunkSize = new Vector2Int(33, 24); // this value is the same as the _reference tilemap size (the starting TILEMAP used for REFERENCE in TILEMAPGENERATOR SCRIPT) 

        Gizmos.color = Color.red;
        Gizmos.DrawWireCube( transform.position + (Vector3.right*xOffset) + (Vector3.up*yOffset),
                             chunkSize);
    }
}
