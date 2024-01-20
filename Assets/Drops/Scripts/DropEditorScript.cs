using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ResourceDrop), true), CanEditMultipleObjects]
public class DropEditorScript : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        ResourceDrop script = (ResourceDrop)target;
        if(GUILayout.Button("Set Sprite"))
        {
            script.SetSprite();
        }
    }
}
