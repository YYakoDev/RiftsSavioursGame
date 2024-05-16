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
        while (number == except)
        {
            number = Random.Range(min, max);
        }
        return number;
    }
    public static int GetRandomIndexExcept(int maxPossibilitiesSize, params int[] exceptions)
    {
        var size = maxPossibilitiesSize - exceptions.Length;
        int[] totalNumbers = new int[size];
        
        for (int i = 0; i < totalNumbers.Length; i++)
        {
            if(IndexMatchesExceptions(i)) continue;
            totalNumbers[i] = i;
        }

        bool IndexMatchesExceptions(int index)
        {
            for (int i = 0; i < exceptions.Length; i++)
            {
                var except = exceptions[i];
                if(except == index) return true;
            }
            return false;
        }

        return totalNumbers[Random.Range(0, size)];
    }

    public static int RandomRangeExcept(int min, int max, params int[] exceptions)
    {
        //Debug.Log($"Min size of random range: {min} \n Max size of random range: {max}");
        var size = (max - min) - exceptions.Length;
        if(size <= 0)
        {
            Debug.LogError("could not get a random range, too many exceptions \n exceptions count:  " + exceptions.Length + "   \n size of the array:  " + size);
            return -1;
        }
        int[] totalNumbers = new int[size+1];
        int iterator = 0;
        for (int i = min; i < max+1; i++)
        {
            if(IndexMatchesExceptions(i)) continue;
            totalNumbers[iterator] = i;
            iterator++;
        }
        bool IndexMatchesExceptions(int index)
        {
            for (int i = 0; i < exceptions.Length; i++)
            {
                var except = exceptions[i];
                if(except == index) return true;
            }
            return false;
        }
        return totalNumbers[Random.Range(0, size)];

    }
}
