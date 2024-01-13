using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEditor;

#if UNITY_EDITOR
[CustomEditor(typeof(StatChangingUpgrade), true)]
public class StatChangingUpgradeEditor : Editor
{
    string[] options = new string[0];
    int index = 0;
    [SerializeField]int[] newDropdowns = new int[0];
    GUIStyle dropdownAddButton = new();
    GUIStyle dropdownRemoveButton = new();
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        StatChangingUpgrade script = (StatChangingUpgrade)target;
        
        if(GUILayout.Button("SEARCH"))
        {
            var QueryResult = script.SearchVariables();
            if(QueryResult == null) return;
            int queryCount = QueryResult.Length;
            Debug.Log(queryCount);
            if(queryCount != options.Length) Array.Resize<string>(ref options, queryCount);
            for (int i = 0; i < queryCount; i++)
            {
                var item = QueryResult[i];
                options[i] = item.Name;
            }
        }
        if(GUILayout.Button("ADD NEW STAT ELEMENT"))
        {
            script.ElementCount++;
            Debug.Log("Adding new element");
        }
        if (GUILayout.Button("REMOVE STAT ELEMENT"))
        {
            script.ElementCount--;
            if (script.ElementCount < 1) script.ElementCount = 1;
            Debug.Log("Removing last element");
        }
        Array.Resize<int>(ref newDropdowns, script.ElementCount);
        for (int i = 0; i < script.ElementCount; i++)
        {
            newDropdowns[i] = EditorGUILayout.Popup(newDropdowns[i], options);
        }
;
        if(GUILayout.Button("SAVE INDEX")) SaveIndexes(script);
    }

    void SaveIndexes(StatChangingUpgrade script)
    {
        script.SetIndexes(newDropdowns);
    }

}
#endif
