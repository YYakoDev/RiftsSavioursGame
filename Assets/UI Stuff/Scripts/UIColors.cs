using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class UIColors
{
    static Color[] colors = new Color[]
    {
        new Color(1,1,1,1),
        new Color(0.72f,0.21f,0.23f,1f),
        new Color(0.25f,0.755f,0.25f,1f),
        new Color(0.098f,0.081f,0.1f,1f),
        new Color(0,0,0,0f),
        new Color(0.239f,0.211f,0.639f,1f),
        new Color(0.772f,0.592f,0.227f,1f),
        new Color(0.596f,0.239f,0.701f,1f),
        new Color(0.59f,0.545f,0.474f,1f),
    };
    static string[] HexColors = new string[]
    {
        "#D6CFCB",
        "#B83A3B",
        "#40C14C",
        "#191421",
        "#000000",
        "#3D36A3",
        "#C67A3A",
        "#983db3",
        "#968B79",
    };

    public static Color GetColor(UIColor color = UIColor.None)
    {
        return colors[(int)color];
    }

    public static string GetHexColor(UIColor color)
    {
        return HexColors[(int)color];
    }

    public static int GetRarityColorIndex(UpgradeRarity rarity) => rarity switch
    {
        UpgradeRarity.Broken => 8,
        UpgradeRarity.Common => 0,
        UpgradeRarity.Uncommon => 2,
        UpgradeRarity.Rare => 5,
        UpgradeRarity.Epic => 7,
        UpgradeRarity.Legendary => 6,
        _ => 0
    };


}
public enum UIColor
{
    None = 0, Red = 1, Green = 2, Black = 3, Transparent = 4, Blue = 5, Orange = 6, Purple = 7, Grey = 8,
}

