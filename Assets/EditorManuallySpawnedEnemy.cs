using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

#if UNITY_EDITOR
[CustomEditor(typeof(ManualEnemySpawner))]
public class EditorManuallySpawnedEnemy : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        ManualEnemySpawner script = target as ManualEnemySpawner;
        if(GUILayout.Button("CreateEnemy"))
        {
            script.SpawnEnemy();
        }
    }
}
#endif