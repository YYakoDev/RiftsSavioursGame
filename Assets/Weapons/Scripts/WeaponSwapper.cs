using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSwapper : MonoBehaviour, IInteractable
{
    [SerializeField] SOPlayerStats _playerStats;
    [SerializeField] WeaponManager _weaponManager;
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

    bool PlayerHasWeapons()
    {
        var weapons = _playerStats.Weapons;
        bool result = false;
        for (int i = 0; i < weapons.Length; i++)
        {
            if(weapons[i] != null)
            {
                result = true;
                break;
            }
        }
        return result;
    }
    void SwapWeapon()
    {
        if(_weaponOnPedestal == null) return;
        if(!PlayerHasWeapons())
        {
            _weaponManager.SetWeaponAtIndex(0, _weaponOnPedestal);
            _playerStats.Weapons[0] = _weaponOnPedestal;
            SetPedestalWeapon(null);
            _riftStarter.AlreadyInteracted = false;
            return;
        }
        _weaponSelector.OpenMenu(_weaponOnPedestal, this);
        _alreadyInteracted = false;
        _riftStarter.AlreadyInteracted = false;
    }

    public void SetPedestalWeapon(WeaponBase weapon)
    {
        _weaponOnPedestal = weapon;
        if(weapon != null) _spriteOnPedestal.sprite = weapon.SpriteAndAnimationData.Sprite;
        else _spriteOnPedestal.sprite = _emptySprite;
    }

    void Deactivate(SOEnemyWave wave)
    {
        return;
        gameObject.SetActive(false);
    }

    private void OnDestroy() {
        _waveSys.OnWaveChange -= Deactivate;
    }
}
