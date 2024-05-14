using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Random = UnityEngine.Random;

public static class HelperMethods
{
    private static Camera _camera;
    public static Camera MainCamera
    {
        get{
            if(_camera == null) _camera = Camera.main;
            return _camera;
        }
    }

    /*private static readonly Dictionary<float, WaitForSeconds> WaitDictionary = new();
    public static WaitForSeconds GetWait(float time)
    {
        if(WaitDictionary.TryGetValue(time, out var waitForSeconds)) return waitForSeconds;

        WaitDictionary[time] = new WaitForSeconds(time);
        return WaitDictionary[time];
    }*/

    private static PointerEventData _eventDataCurrentPosition;
    private static List<RaycastResult> _results;

    public static bool IsOverUI()
    {
        EventSystem currentEventSys = EventSystem.current;
        _eventDataCurrentPosition = new PointerEventData(currentEventSys) {position = YYInputManager.MousePosition};
        _results = new();
        currentEventSys.RaycastAll(_eventDataCurrentPosition, _results);
        return _results.Count > 0;
    }

    public static string AddSpacesToSentence(string text, bool preserveAcronyms)
    {
        if (string.IsNullOrWhiteSpace(text))
           return string.Empty;
        StringBuilder newText = new StringBuilder(text.Length * 2);
        newText.Append(text[0]);
        for (int i = 1; i < text.Length; i++)
        {
            if (char.IsUpper(text[i]))
                if ((text[i - 1] != ' ' && !char.IsUpper(text[i - 1])) ||
                    (preserveAcronyms && char.IsUpper(text[i - 1]) && 
                     i < text.Length - 1 && !char.IsUpper(text[i + 1])))
                    newText.Append(' ');
            newText.Append(text[i]);
        }
        return newText.ToString();
    }

    public static int RandomNumberExcept(int min, int max, int except)
    {
        int number = Random.Range(min, max);
        do
        {
            number = Random.Range(min, max);
        } while (number == except);
        return number;
    }


}
