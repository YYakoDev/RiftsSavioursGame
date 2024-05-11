using UnityEngine;
using System.Collections;

public class KeyTest : MonoBehaviour
{
    void OnGUI()
    {
        System.Array values = System.Enum.GetValues(typeof(JoystickKeyCodes));
        foreach(KeyCode code in values)
	        if(Input.GetKeyDown(code))  print(System.Enum.GetName(typeof(KeyCode), code)); 	

    }
}
