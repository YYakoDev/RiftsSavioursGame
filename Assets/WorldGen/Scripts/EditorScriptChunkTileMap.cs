using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

#if UNITY_EDITOR
[CustomEditor(typeof(ChunkTileMap))]
public class EditorScriptChunkTileMap : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        ChunkTileMap script = target as ChunkTileMap;
        if(GUILayout.Button("ADD CHUNK TO WORLD"))
        {
            script.AddChunkToParentWorld();
        }
    }
}
#endif