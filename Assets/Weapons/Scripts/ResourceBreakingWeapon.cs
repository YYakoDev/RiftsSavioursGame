using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = MenuPath + "ResourceBreakingWeapon")]
public class ResourceBreakingWeapon : MeleeWeapon
{
    [SerializeField] LayerMask _resourcesLayer;
    LayerMask _targetsLayer;
    public override void Initialize(WeaponManager weaponManager, Transform prefabTransform)
    {
        base.Initialize(weaponManager, prefabTransform);
        _targetsLayer = _resourcesLayer | _enemyLayer;
    }
    protected override bool DetectEnemies(float atkRange)
    {
        Collider2D[] hittedEnemies =  Physics2D.OverlapCircleAll(_attackPoint, atkRange, _targetsLayer);
        if(hittedEnemies.Length == 0) return false;

        _hittedEnemiesGO.Clear();
        for(int i = 0; i < hittedEnemies.Length; i++)
        {
            if(_hittedEnemiesGO.Contains(hittedEnemies[i].gameObject)) continue;
            _hittedEnemiesGO.Add(hittedEnemies[i].gameObject);
        }
        return true;
    }
    protected override void DoAttackLogic()
    {
        base.DoAttackLogic();
        AttackResources();
    }

    void AttackResources()
    {
        for (int i = 0; i < _hittedEnemiesGO.Count; i++)
        {
            if(_hittedEnemiesGO[i] == null || !_hittedEnemiesGO[i].activeSelf)continue;

            if(_hittedEnemiesGO[i].TryGetComponent<Resource>(out Resource resource))
            {
                resource.Interact(_modifiedStats._atkDmg);
                //PopupsManager.Create(_hittedEnemiesGO[i].transform.position + Vector3.up * 0.75f, _modifiedStats._atkDmg);
                InvokeOnEnemyHit(_hittedEnemiesGO[i].transform.position);
            }
        }
    }
}
