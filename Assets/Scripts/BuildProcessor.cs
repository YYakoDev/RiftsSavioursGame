
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;

public class BuildProcessor : IPreprocessBuildWithReport
{
    public int callbackOrder { get { return 0; } }
    public void OnPreprocessBuild(BuildReport report)
    {
        AssetMenuUpdators.UpgradesSaveText();
        AssetMenuUpdators.UpdateCraftingMaterialIcons();
    }
}
