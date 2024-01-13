using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public static class PopupsManager
{
    private static bool Initialized = false;
    private static bool PoolCreated = false;
    private static GameObject PopupPrefab;
    private static Transform Parent;
    private static NotMonoObjectPool ObjectPool;
    public static void Create(Vector3 position, int damageAmount)
    {
        if(!Initialized) LoadPrefab();
        if(!PoolCreated) InitializePool();
        GameObject damagePopup = ObjectPool.GetObjectFromPool();
        damagePopup.transform.position = position;
        damagePopup.SetActive(true);

        TextMeshPro text = damagePopup.GetComponent<TextMeshPro>();
        text.SetText(damageAmount.ToString());
    }

    static void LoadPrefab()
    {
        PopupPrefab = Resources.Load<TextMeshPro>("DamagePopUp").gameObject;
        Parent = GameObject.FindGameObjectWithTag("PopupCanvas").transform;
        InitializePool();
        Initialized = true;
    }

    static void InitializePool()
    {
        if(Parent == null) Parent = GameObject.FindGameObjectWithTag("PopupCanvas").transform;
        ObjectPool = new(55, PopupPrefab, Parent, true);
        PoolCreated = true;
    }

    public static void RecreatePool()
    {
        PoolCreated = false;
    }

}
