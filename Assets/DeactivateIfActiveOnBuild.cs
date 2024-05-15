using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeactivateIfActiveOnBuild : MonoBehaviour
{
    private void Awake() {
        if(gameObject.activeInHierarchy && (Application.installMode != ApplicationInstallMode.Editor)) gameObject.SetActive(false);
    }
}
