using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class PickUpsController : MonoBehaviour
{
    [Header("References")]
    [SerializeField]PlayerManager _playerManager;
    CircleCollider2D _collider;

    [Header("Pick Up Properties")]
    float _pickUpRange;

    //properties 
    public float PickUpRange => _pickUpRange;

    
    private void Awake() {
        if(_playerManager == null) _playerManager = GetComponentInParent<PlayerManager>();
        gameObject.CheckComponent<CircleCollider2D>(ref _collider);
    }
    
    void Start()
    {
        _playerManager.Stats.onStatsChange += SetPickUpRange;
        SetPickUpRange();
    }

    void SetPickUpRange()
    {
        if(_playerManager.Stats.PickUpRange == _pickUpRange)return;
        _pickUpRange = _playerManager.Stats.PickUpRange;
        _collider.radius = _pickUpRange;
    }



    //pickupEffects
    public void AddXP(int xpAmount)
    {
        _playerManager.LevelManager.AddXP(xpAmount);
    }

    public void AddMaterial(CraftingMaterial material)
    {
        _playerManager.Inventory.AddMaterial(material);
    }



    private void OnTriggerEnter2D(Collider2D other){
        if(other.TryGetComponent<DropPrefab>(out DropPrefab drop))
        {
            drop.PickUp(this);
        }
    }

    private void OnDestroy() {
        _playerManager.Stats.onStatsChange -= SetPickUpRange;
    }


    
}
