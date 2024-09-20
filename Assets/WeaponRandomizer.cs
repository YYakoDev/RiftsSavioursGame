using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponRandomizer : MonoBehaviour
{
    [SerializeField] WeaponBase[] _weapons;
    [SerializeField] WeaponSwapper _swapper;

    private void Awake() {
        _swapper.SetWeapon(_weapons[Random.Range(0, _weapons.Length)]);
    }
}
