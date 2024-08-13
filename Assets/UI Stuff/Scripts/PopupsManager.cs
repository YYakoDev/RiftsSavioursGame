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
    private static float TextSize = 32; //you should be chaning this

    static PopupProperties Default = new(0.9f, UIColor.None, FontStyles.Normal), 
    Bold = new(1.25f, UIColor.None, FontStyles.Bold), 
    CriticalYellow = new(1.3f, UIColor.Orange, FontStyles.Bold), //where the text is you could put an emoji if you like
    CriticalRed = new(1.65f, UIColor.Red, FontStyles.Bold, "!");

    public static void CreateDamagePopup(Vector3 position, int damageAmount, DamagePopupTypes styleType = DamagePopupTypes.Normal)
    {
        if(!Initialized) LoadPrefab();
        if(!PoolCreated) InitializePool();
        GameObject damagePopup = ObjectPool.GetObjectFromPool();
        damagePopup.transform.position = position;
        var currentStyle = styleType switch
        {
            DamagePopupTypes.Normal => Default,
            DamagePopupTypes.Bold => Bold,
            DamagePopupTypes.CriticalYellow => CriticalYellow,
            DamagePopupTypes.CriticalRed => CriticalRed,
            _ => Default
        };
        TextMeshPro text = damagePopup.GetComponent<TextMeshPro>();
        float sizeOffset = Random.Range(0.9f, 1.1f);
        string damageText = damageAmount.ToString() + currentStyle.AppendixText;
        text.color = currentStyle.Color;
        text.fontSize = TextSize * currentStyle.SizeMultiplier * sizeOffset;
        text.SetText(damageText);

        damagePopup.SetActive(true);
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

    struct PopupProperties
    {
        float _sizeMultiplier;
        Color _color;
        TMPro.FontStyles _fontStyle;
        string _appendixText;

        public float SizeMultiplier => _sizeMultiplier;
        public Color Color => _color;
        public TMPro.FontStyles FontStlye => _fontStyle;
        public string AppendixText => _appendixText;
        public PopupProperties(float sizeMultiplier, UIColor color, TMPro.FontStyles fontStyle, string appendixText = "")
        {
            _sizeMultiplier = sizeMultiplier;
            _color = UIColors.GetColor(color);
            _fontStyle = fontStyle;
            _appendixText = appendixText;
        }
    }
}
