using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

[System.Serializable]
public class RewardItem
{
    [SerializeField] string _name, _desctiption;
    [SerializeField] RewardType _rewardType;
    [SerializeField] Sprite _sprite;
    [SerializeField] AudioClip _sfx;
    bool _availability;

    public string Name => _name;
    public string Description => _desctiption;
    public RewardType Type => _rewardType;
    public bool Available { get => _availability; set => _availability = value; }
    public Sprite Sprite => _sprite;
    public AudioClip Sfx => _sfx;
}
