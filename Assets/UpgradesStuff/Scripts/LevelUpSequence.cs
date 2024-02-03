using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(TweenAnimatorMultiple))]
public class LevelUpSequence : MonoBehaviour
{
    //DEBUG
    [SerializeField]bool _playOnStart = false;

    [Header("References")]
    [SerializeField] GameObject _visualsParent;
    [SerializeField] Image _levelUpImg;
    [SerializeField] Animator _levelUpTxtAnimator;
    [SerializeField] Image _levelUpBG;
    [SerializeField] Image _anvilImg;
    [SerializeField] Animator _anvilAnimator;
    [SerializeField] Image _anvilFX;
    [SerializeField] Animator _anvilFxAnimator;
    [SerializeField] Transform _playerTransform;
    [SerializeField] LayerMask _enemyLayer;
    TweenAnimatorMultiple _animator;

    [Header("Animations")]
    [SerializeField] float _anvilFallDuration = 0.75f;
    [SerializeField] Vector3 _anvilOffset;
    [SerializeField] float _levelTxtFadeInDuration = 0.25f;
    [SerializeField, Range(0, 255)] int _flashOpacity;
    [SerializeField] float _flashInterval = 0.3f;
    [SerializeField] float _flashDuration = 0.1f;
    Timer _flashTimer;
    int _flashAttempts = 0, _maxFlashAttempts = 2;


    [Space]
    [SerializeField] AudioSource _audioSource;
    [SerializeField] AudioClip _anvilFallSfx, _anvilFallingSfx, _levelUpSfx, _flashSfx;

    bool _hasLeveledUp = false;

    private void Awake()
    {
        _animator = GetComponent<TweenAnimatorMultiple>();
        _animator.ChangeTimeScalingUsage(TweenAnimator.TimeUsage.UnscaledTime);
        _audioSource = GetComponent<AudioSource>();
        PlayerLevelManager.onLevelUp += PlayerHasLeveledUp;
        if (_levelUpTxtAnimator == null) _levelUpTxtAnimator = _levelUpImg.GetComponent<Animator>();

        _flashTimer = new(_flashInterval, true, true);
        _flashTimer.Stop();
        _flashTimer.onStart += StartFlash;
        _flashTimer.onEnd += StopFlash;
    }

    private void Start() {
        if(_playerTransform == null) _playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        if(_playOnStart) Play(null);
        else DisableVisuals();
    }

    void SetInitialStates()
    {
        _animator.Clear();
        _visualsParent.SetActive(true);
        var newColor = _levelUpImg.color;
        newColor.a = 0;
        _levelUpImg.color = newColor;
        _anvilImg.rectTransform.localPosition = new Vector3(0, 2000, 0);
    }

    private void Update() {
        _flashTimer?.UpdateTime();
    }

    public void Play(Action onComplete)
    {
        TimeScaleManager.ForceTimeScale(0f);
        SetInitialStates();
        AnvilFall(CheckLevelUpAnim);
        void CheckLevelUpAnim() => LevelUpImageAnimation(onComplete);

        //onComplete?.Invoke();
    }

    void LevelUpImageAnimation(Action onComplete)
    {
        if(!_hasLeveledUp)
        {
            onComplete?.Invoke();
            //_visualsParent.SetActive(false);
            return;
        }
        PlayAudio(_levelUpSfx);
        _animator?.TweenImageOpacity(_levelUpImg.rectTransform, 255, _levelTxtFadeInDuration, onComplete: () => 
        {
            //this animation is only being played because i need the oncomplete to happen after a certain time, is a fake animation if you will
            _animator?.TweenImageOpacity(_levelUpImg.rectTransform, 255, 0.1f + _flashDuration * 2 + _flashInterval, onComplete: () =>
            {
                onComplete?.Invoke();
                _hasLeveledUp = false;
                //_visualsParent.SetActive(false);
            });
        });
        _levelUpTxtAnimator.enabled = true;
        _levelUpTxtAnimator.Play("Animation");
        _flashTimer.Start();
        
    }

    void StartFlash()
    {
        //do the flash sfxs here
        PlayAudio(_flashSfx);
        _animator?.TweenImageOpacity(_levelUpBG.rectTransform, _flashOpacity, _flashDuration, onComplete: () =>
        {
            _animator?.TweenImageOpacity(_levelUpBG.rectTransform, 0, _flashDuration / 3f);
        });
    }

    void StopFlash()
    {
        _flashAttempts++;
        if(_flashAttempts >= _maxFlashAttempts)
        {
            _flashTimer.Stop();
            _flashAttempts = 0;
        }
    }

    void AnvilFall(Action onComplete)
    {
        _anvilAnimator.enabled = true;
        _anvilAnimator.Play("Animation");
        _anvilFxAnimator.enabled = true;
        _anvilFxAnimator.Play("Animation");
        PlayAudio(_anvilFallingSfx);
        StartCoroutine(PlayFallSFX(_anvilFallDuration / 1.3f));
        _animator?.TweenMoveToBouncy(_anvilImg.rectTransform, Vector3.zero + _anvilOffset, Vector3.down * 25f, _anvilFallDuration, 0, 3, curve: CurveTypes.Linear, onBounceComplete: () => 
        {

            onComplete?.Invoke();
            AffectSurroundingEntities();
        });
    }

    IEnumerator PlayFallSFX(float waitTime)
    {
        yield return new WaitForSecondsRealtime(waitTime);
        PlayAudio(_anvilFallSfx);
    }
    void AffectSurroundingEntities()
    {
        var results = Physics2D.OverlapCircleAll(_playerTransform.position, 4f, _enemyLayer);
        if(results.Length == 0) return;
        foreach(Collider2D enemyColl in results)
        {
            if(enemyColl.TryGetComponent<IDamageable>(out var damageable))
            {
                damageable.TakeDamage(2);
                if(enemyColl.TryGetComponent<IKnockback>(out var knockbackable))
                {
                    knockbackable.KnockbackLogic.SetKnockbackData(_playerTransform, 5f, ignoreResistance: true);
                }
            }
        }
    }
    public void DisableVisuals() => _visualsParent.gameObject.SetActive(false);
    void PlayAudio(AudioClip clip) => _audioSource?.PlayWithVaryingPitch(clip);
    void PlayerHasLeveledUp() => _hasLeveledUp = true;

    private void OnDestroy() {
        PlayerLevelManager.onLevelUp -= PlayerHasLeveledUp;
        _flashTimer.onStart -= StartFlash;
        _flashTimer.onEnd -= StopFlash;
    }
}
