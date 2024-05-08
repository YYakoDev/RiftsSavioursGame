using UnityEngine;
using System.Collections;

public class KeyTest : MonoBehaviour
{
    void OnGUI()
    {
        if (Input.inputString != "") Debug.Log(Input.inputString);
    }
}
