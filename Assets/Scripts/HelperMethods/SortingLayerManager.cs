using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SortingLayerManager
{
    public static int GetLayer(LayerType type)
    {
        int value = type switch
        {
            LayerType.Below => -1,
            LayerType.Default => 0,
            LayerType.Above => 1,
            _ => 0
        };

        var layers = SortingLayer.layers;
        foreach(var layer in layers)
        {
            if(layer.value == value) return layer.id;
        }
        return 0;
    }
}

public enum LayerType
{
    Below, Default, Above
}