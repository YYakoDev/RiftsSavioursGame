using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PopupsManager : MonoBehaviour
{
    private static GameObject PopupPrefab;
    private static Transform Parent;
    public static void Create(Vector3 position, int damageAmount)
    {
        if(PopupPrefab == null) LoadPrefab();
        GameObject damagePopup = Instantiate(PopupPrefab, position, Quaternion.identity);
        if(Parent != null) damagePopup.transform.SetParent(Parent);
        
        TextMeshPro text = damagePopup.GetComponent<TextMeshPro>();
        text.SetText(damageAmount.ToString());
    }

    static void LoadPrefab()
    {
        PopupPrefab = Resources.Load<TextMeshPro>("DamagePopUp").gameObject;
        Parent = GameObject.FindGameObjectWithTag("PopupCanvas").transform;
    }
}
