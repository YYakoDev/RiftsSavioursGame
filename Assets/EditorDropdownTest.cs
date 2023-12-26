using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

#if UNITY_EDITOR
[CustomEditor(typeof(EditorTestMono))]
public class EditorDropdownTest : Editor
{
    GUIContent content = new("hey you", "baby you dont know, how bad is gonna get");
    GenericMenu e = new();
    string[] options = new string[3]
    {
        "option 1",
        "option 2",
        "option 3"
    };
    int index = 0;
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        /*var cont1 = new GUIContent("help me");
        var cont2 = new GUIContent("im losing my heart again");
        var cont3 = new GUIContent("is it gone");

        AddMenuItem(content, Func, content);
        AddMenuItem(cont1, Func, cont1);
        AddMenuItem(cont2, Func, cont2);
        AddMenuItem(cont3, Func, cont3);
        if(EditorGUILayout.DropdownButton(content, FocusType.Keyboard))
        {
            e.ShowAsContext();
        }*/

        index = EditorGUILayout.Popup(index, options);
    }

    void AddMenuItem(GUIContent content, GenericMenu.MenuFunction2 function, object userdata)
    {
        e.AddItem(content, false, function, userdata);
    }
    void Func(object c)
    {
        Debug.Log("Hey you baby  " + c);
    }
}
#endif
