
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeactivateOnBuild : MonoBehaviour
{
    
    void Start()
    {
        gameObject.SetActive((Application.installMode == ApplicationInstallMode.Editor));
    }
    
}
