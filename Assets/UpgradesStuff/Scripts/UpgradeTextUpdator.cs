using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class UpgradeTextUpdator 
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
}
