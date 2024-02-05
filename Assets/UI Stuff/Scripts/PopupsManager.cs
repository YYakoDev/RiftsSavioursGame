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
    private static float TextSize = 36; //you should be chaning this

    public static void CreateDamagePopup(Vector3 position, int damageAmount, bool criticalHit)
    {
        if(!Initialized) LoadPrefab();
        if(!PoolCreated) InitializePool();
        GameObject damagePopup = ObjectPool.GetObjectFromPool();
        damagePopup.transform.position = position;
        damagePopup.SetActive(true);
        float sizeOffset = Random.Range(0.85f, 1.22f);
        TextMeshPro text = damagePopup.GetComponent<TextMeshPro>();
        string damageText = (criticalHit) ? damageAmount.ToString() + "!" : damageAmount.ToString();
        text.color = (criticalHit) ? UIColors.GetColor(UIColor.Red) : UIColors.GetColor(UIColor.None);
        text.fontSize = (criticalHit) ? TextSize * 1.5f : TextSize * sizeOffset;
        text.SetText(damageText);
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
