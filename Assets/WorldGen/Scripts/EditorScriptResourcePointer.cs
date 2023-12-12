using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

#if UNITY_EDITOR
[CustomEditor(typeof(ResourcePointer)), CanEditMultipleObjects]
public class EditorScriptResourcePointer : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        //EditorGUILayout.HelpBox("This is a help box", MessageType.Info);
        //EditorGUILayout.LinkButton("help");

        ResourcePointer Script = (ResourcePointer) target;
        if(GUILayout.Button("Preview Sprite"))
        {
            Script.PreviewSprite();
        }
    }
}
#endif