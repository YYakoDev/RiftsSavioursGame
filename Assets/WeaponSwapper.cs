using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSwapper : MonoBehaviour, IInteractable
{
    [SerializeField] WaveSystem _waveSys;
    [SerializeField] bool _disableRift = true;
    [SerializeField] PrototypeRift _riftStarter;
    [SerializeField] UIWeaponSelector _weaponSelector;
    [SerializeField] SpriteRenderer _spriteOnPedestal;
    [SerializeField] Sprite _emptySprite;
    WeaponBase _weaponOnPedestal;
    [SerializeField] Vector3 _offset;
    private bool _alreadyInteracted;
    [SerializeField] AudioClip _interactSfx;
    public bool AlreadyInteracted { get => _alreadyInteracted; set => _alreadyInteracted = value; }
    public Vector3 Offset => _offset;
    public AudioClip InteractSfx => _interactSfx;

    private void Start() {
        _waveSys.OnWaveChange += Deactivate;
        _riftStarter.AlreadyInteracted = true;
    }

    public void Interact()
    {
        SwapWeapon();
    }

    void SwapWeapon()
    {
        if(_weaponOnPedestal == null) return;
        _weaponSelector.OpenMenu(_weaponOnPedestal, this);
        _riftStarter.AlreadyInteracted = false;
        _alreadyInteracted = false;
    }

    public void SetWeapon(WeaponBase weapon)
    {
        _weaponOnPedestal = weapon;
        if(weapon != null) _spriteOnPedestal.sprite = weapon.SpriteAndAnimationData.Sprite;
        else _spriteOnPedestal.sprite = _emptySprite;
    }

    void Deactivate(SOEnemyWave wave)
    {
        gameObject.SetActive(false);
    }

    private void OnDestroy() {
        _waveSys.OnWaveChange -= Deactivate;
    }
}
