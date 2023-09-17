using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponFX : MonoBehaviour
{
    private float _attackDuration;

    public void Initialize(float attackDuration)
    {
        _attackDuration = attackDuration;
        StartCoroutine(DisableAfterAttack());
    }
    
    IEnumerator DisableAfterAttack()
    {
        yield return new WaitForSeconds(_attackDuration);
        gameObject.SetActive(false);
    }
}
