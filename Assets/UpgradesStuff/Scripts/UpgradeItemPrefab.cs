using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UpgradeItemPrefab : MonoBehaviour
{
    
    [SerializeField]private TextMeshProUGUI _upgradeName; 
    [SerializeField]private TextMeshProUGUI _upgradeEffect;
    [SerializeField]private Image _upgradeIcon;
    //this 3 fields above will inherit the properties of the upgrade in question such as the effect, the name and the sprite
    [SerializeField]private Button _craftUpgradeButton;

    //[SerializeField]private RectTransform _requirementsContainer;
    [SerializeField]RecipeItemPrefab _recipeItemPrefab;
    private UpgradeCost[] _upgradeCosts;
    
    //properties
    public Button CraftUpgradeButton => _craftUpgradeButton;

    void Initialize(SOUpgradeBase upgrade)
    {
        _upgradeName.text = upgrade.name;
        _upgradeEffect.text = upgrade.EffectDescription;
        _upgradeIcon.sprite = upgrade.Sprite;
        _upgradeCosts = upgrade.UpgradeCosts;
        //_craftingMaterials = upgrade.CraftingMaterials;
        foreach(UpgradeCost cost in _upgradeCosts)
        {
            GameObject recipeItemGO = Instantiate(_recipeItemPrefab.gameObject, transform.position, Quaternion.identity);
            recipeItemGO.transform.SetParent(this.transform);
            recipeItemGO.GetComponent<RecipeItemPrefab>().Initialize(cost);     
        }
    }

        
    
}
