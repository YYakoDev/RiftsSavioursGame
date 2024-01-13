using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class UIColors
{
    static Color[] colors = new Color[5]
    {
        new Color(1,1,1,1),
        new Color(0.72f,0.21f,0.23f,1),
        new Color(0.31f,0.63f,0.24f,1f),
        new Color(0,0,0,1),
        new Color(0,0,0,0)
    };

    public static Color GetColor(UIColor color = UIColor.None)
    {
        return colors[(int)color];
    }
}
public enum UIColor
{
    None = 0, Red = 1, Green = 2, Black = 3, Transparent = 4
}

