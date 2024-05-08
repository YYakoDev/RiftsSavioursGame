using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class PickUpsController : MonoBehaviour
{
    [Header("References")]
    [SerializeField]PlayerManager _playerManager;
    [SerializeField] ResourcesUIPopupManager _pickupsPopupManager;
    CircleCollider2D _collider;

    [Header("Pick Up Properties")]
    float _pickUpRange;

    //audio stuff
    [SerializeField] AudioSource _audio;
    [SerializeField] float _pitchIncrements = 0.05f, _pitchResetTime = 1f;
    float _previousPitch , _minPitch = 0.9f, _maxPitch = 2f;
    Timer _pitchTimer;

    //properties 
    public float PickUpRange => _pickUpRange;

    
    private void Awake() {
        if(_playerManager == null) _playerManager = GetComponentInParent<PlayerManager>();
        gameObject.CheckComponent<CircleCollider2D>(ref _collider);
        _audio.pitch = _minPitch;
        _pitchTimer = new(_pitchResetTime);
        _pitchTimer.Stop();
        _pitchTimer.onEnd += ResetPitch;
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


    private void Update() {
        _pitchTimer.UpdateTime();
    }


    //pickupEffects
    public void AddXP(int xpAmount)
    {
        _playerManager.LevelManager.AddXP(xpAmount);
    }

    public void AddMaterial(CraftingMaterial material)
    {
        _playerManager.Inventory.AddMaterial(material);
        _pickupsPopupManager?.SpawnMaterialPopup(material);
    }

    public void PlayAudioClip(AudioClip sound)
    {
        _previousPitch = _audio.pitch;
        var newPitch = _previousPitch + _pitchIncrements;
        newPitch = Mathf.Clamp(newPitch, _minPitch, _maxPitch);
        _audio.pitch = newPitch;
        _pitchTimer.Start();
        _audio.PlayOneShot(sound);
        
    }

    void ResetPitch() => _audio.pitch = _minPitch;

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
