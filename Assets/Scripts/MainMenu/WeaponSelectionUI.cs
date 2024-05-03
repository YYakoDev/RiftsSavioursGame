using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[RequireComponent(typeof(TweenAnimatorMultiple))]
public class WeaponSelectionUI : MonoBehaviour
{
    [SerializeField] WeaponBase[] _availableWeapons = new WeaponBase[0];
    [SerializeField] Image _weaponIcon;
    int _currentIndex = 0;
    public static WeaponBase SelectedWeapon;
    float _inputCooldown = 0.25f;
    float _nextInputTime = 0;
    int CurrentIndex
    {
        get => _currentIndex;
        set 
        {
            if(value >= _availableWeapons.Length) _currentIndex = 0;
            else if(value < 0) _currentIndex = _availableWeapons.Length - 1;
            else _currentIndex = value;
        }
    }

    [Space][SerializeField] AudioSource _audio;
    [SerializeField] AudioClip _moveSFX;
    //animations
    TweenAnimatorMultiple _animator;
    [Space]
    [SerializeField]float _animDuration = 0.7f;
    [SerializeField] Vector3 _endScale = Vector3.one * 2;
    [SerializeField] Vector3 _scaleAnimOffset = Vector3.up;
    [SerializeField] int _scaleBounces = 3;

    private void Awake() {
        _animator = GetComponent<TweenAnimatorMultiple>();
        _animator.ChangeTimeScalingUsage(TweenAnimator.TimeUsage.UnscaledTime);
    }

    private void Start() {
        SetSelectedWeapon();
    }

    private void Update() 
    {
        if(_nextInputTime >= Time.time) return;
        float input = Input.GetAxisRaw("Vertical");
        
        if(input != 0)
        {
            int move = (input > 0.1f) ? 1 : -1;
            MoveIndex(move);
        }
    }

    public void MoveIndex(int move)
    {
        _audio?.PlayWithVaryingPitch(_moveSFX);
        move = Mathf.Clamp(move, -1, 1);
        _nextInputTime = _inputCooldown + Time.time;
        CurrentIndex += move;
        SetSelectedWeapon();
        
    }

    void SetSelectedWeapon()
    {
        SelectedWeapon = _availableWeapons[_currentIndex];
        PlayAnimation();
        _weaponIcon.sprite = SelectedWeapon.SpriteAndAnimationData.Sprite;
    }

    void PlayAnimation()
    {
        _animator.Clear();
        Vector3 startingSize = _weaponIcon.rectTransform.localScale;
        startingSize.y = 0;
        _weaponIcon.rectTransform.localScale = startingSize;
        _animator.TweenScaleBouncy(_weaponIcon.rectTransform, _endScale, _scaleAnimOffset, _animDuration, 0, _scaleBounces);
    }
}
