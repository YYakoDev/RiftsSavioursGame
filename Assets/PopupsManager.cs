using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PopupsManager : MonoBehaviour
{
    private static GameObject PopupPrefab;
    public static void Create(Vector3 position, int damageAmount)
    {
        if(PopupPrefab == null) LoadPrefab();
        GameObject damagePopup = Instantiate(PopupPrefab, position, Quaternion.identity);
        TextMeshPro text = damagePopup.GetComponent<TextMeshPro>();
        text.SetText(damageAmount.ToString());
    }

    static void LoadPrefab()
    {
        Debug.Log("Loading Prefab");
        PopupPrefab = Resources.Load<TextMeshPro>("DamagePopUp").gameObject;
    }
}
