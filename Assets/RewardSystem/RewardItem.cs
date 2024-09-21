using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class RewardItem : MonoBehaviour, IInteractable
{
    [SerializeField] SOPlayerInventory _inventory;
    [SerializeField] RewardType _rewardType;
    protected bool _alreadyInteracted;
    [SerializeField] Vector3 _offset;
    protected bool _available = true;
    [SerializeField] AudioClip _interactSfx;

    public bool AlreadyInteracted { get => _alreadyInteracted; set => _alreadyInteracted = value; }
    public Vector3 Offset => _offset;
    public AudioClip InteractSfx => _interactSfx;
    public bool Availabe { get => _available; set => _available = value; }
    public RewardType RewardType => _rewardType;

    private void OnEnable() {
        _available = true;
    }

    public virtual void Interact()
    {
        _inventory.AddToken(this);
        gameObject.SetActive(false);
    }
}
