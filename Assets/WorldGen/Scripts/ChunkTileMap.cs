using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ChunkTileMap : MonoBehaviour
{
    public float xOffset = 0.5f;
    public float yOffset = 0f;
    /*
    [HideInInspector] public Vector2Int position;
    private Tilemap _tileMapComponent;
    public Tilemap TileMap => _tileMapComponent;
    private void Awake() {
        gameObject.CheckComponent<Tilemap>(ref _tileMapComponent);
    }

    private void OnValidate() {
        

    }*/
 
    private void OnDrawGizmos() {
        if(Application.isPlaying) return;
        Vector2 chunkSize = new Vector2Int(33, 24); // this value is the same as the _reference tilemap size (the starting TILEMAP used for REFERENCE in TILEMAPGENERATOR SCRIPT) 

        Gizmos.color = Color.red;
        Gizmos.DrawWireCube( transform.position + (Vector3.right*xOffset) + (Vector3.up*yOffset),
                             chunkSize);
    }
}
