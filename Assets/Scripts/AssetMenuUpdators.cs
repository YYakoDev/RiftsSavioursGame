using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class AssetMenuUpdators 
{
    [MenuItem("Upgrades/UpgradesTryReadText")]
    public static void UpgradesTryReadText()
    {
        var type = typeof(StatChangingUpgrade);
        var data = AssetDatabase.FindAssets("t: StatChangingUpgrade", new[] {"Assets/UpgradesStuff/Upgrades/UpgradeTypes"});
        foreach(var item in data)
        {
            var loadedAsset = AssetDatabase.LoadAssetAtPath(AssetDatabase.GUIDToAssetPath(item), type);
            var assetOfType = loadedAsset as StatChangingUpgrade;
            assetOfType.TryReadText();
        }
    }
    [MenuItem("Upgrades/SaveTextFromUpgrades")]
    public static void UpgradesSaveText()
    {
        var type = typeof(StatChangingUpgrade);
        var data = AssetDatabase.FindAssets("t: StatChangingUpgrade", new[] {"Assets/UpgradesStuff/Upgrades/UpgradeTypes"});
        foreach(var item in data)
        {
            var loadedAsset = AssetDatabase.LoadAssetAtPath(AssetDatabase.GUIDToAssetPath(item), type);
            var assetOfType = loadedAsset as StatChangingUpgrade;
            assetOfType.SaveTextAndRead();
        }
    }
    [MenuItem("Drops/UpdateCraftingMaterialIcons")]
    public static void UpdateCraftingMaterialIcons()
    {
        var type = typeof(ResourceDrop);
        var data = AssetDatabase.FindAssets("t: ResourceDrop", new[] {"Assets/Drops/Scripts/DropTypes"});
        foreach(var item in data)
        {
            var loadedAsset = AssetDatabase.LoadAssetAtPath(AssetDatabase.GUIDToAssetPath(item), type);
            var assetOfType = loadedAsset as ResourceDrop;
            assetOfType.SetSprite();
        }
    }
}
