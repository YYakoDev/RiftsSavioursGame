using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScreenResolutionTest : MonoBehaviour
{
    [SerializeField]TextMeshProUGUI _text;
    // Start is called before the first frame update
    void Start()
    {
        //Debug.Log(Display.main.renderingWidth + "x" + Display.main.renderingHeight);   
    }

    private void OnValidate() {
        _text.text = Display.main.renderingWidth + "x" + Display.main.renderingHeight;
    }
}
