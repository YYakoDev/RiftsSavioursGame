using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoystickButtonCheker : MonoBehaviour
{
    KeyCode[] _keyValues = new KeyCode[20];

    // Start is called before the first frame update
    void Start()
    {
        var values = System.Enum.GetValues(typeof(JoystickKeyCodes)) as int[];
        
        for (int i = 0; i < _keyValues.Length; i++)
        {
            _keyValues[i] = (KeyCode)values[i];
            Debug.Log(_keyValues[i].ToString());
        }
    }

    // Update is called once per frame
    void Update()
    {
        foreach(KeyCode code in _keyValues)
        {
            if(Input.GetKeyDown(code))
            {
                Debug.Log("JoystickInput!! " + code);
            }
        }
    }
}
