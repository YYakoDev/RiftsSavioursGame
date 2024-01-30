using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(TweenAnimator))]
public class UpgradeMenuAnimations : MonoBehaviour
{
    [Header("References")]
    RectTransform[] _upgradeItemsInstance;

    public void SetElements()
    {
        
    }
    private void OnDrawGizmosSelected() {
    }
}
