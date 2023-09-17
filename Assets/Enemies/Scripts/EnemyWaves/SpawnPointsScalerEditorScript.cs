using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

#if UNITY_EDITOR
[CustomEditor(typeof(SpawnPointsScaler))]
public class SpawnPointsScalerEditorScript : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        //EditorGUILayout.HelpBox("This is a help box", MessageType.Info);
        //EditorGUILayout.LinkButton("help");

        SpawnPointsScaler scalerScript = (SpawnPointsScaler)target;
        if(GUILayout.Button("SET SPAWNPOINTS"))
        {
            scalerScript.UpdateCameraValues();
            scalerScript.ScaleSpawnPointsPosition();
        }
    }
}
#endif