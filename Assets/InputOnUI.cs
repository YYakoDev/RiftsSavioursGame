using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InputOnUI : InputItem
{
    [SerializeField] Image _icon;
    [SerializeField] TextMeshProUGUI t_bindings;
    private void Start() {
        GetInputData();
    }

    public override void GetInputData()
    {
        base.GetInputData();
        t_bindings.SetText(_bindings);
    }

    public void SetIcon()
    {
        
    }



    
}
