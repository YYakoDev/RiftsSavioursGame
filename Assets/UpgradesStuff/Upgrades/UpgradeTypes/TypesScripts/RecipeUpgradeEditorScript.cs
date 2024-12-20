using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

#if UNITY_EDITOR
[CustomEditor(typeof(RecipeUpgrade), true), CanEditMultipleObjects]
public class RecipeUpgradeEditorScript : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        RecipeUpgrade script = (RecipeUpgrade)this.target;
        if(GUILayout.Button("Set Sprite"))
        {
            script.SetSprite();
        }
    }
}
#endif