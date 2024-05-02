using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(TweenAnimatorMultiple))]
public class UISkill : MonoBehaviour
{
    KeyInputTypes _inputType;
    KeyInput _hotKey;
    bool _cooldownBehaviour, _skillOnCooldown, _skillBlinking;
    float _cooldown = 0f;
    Timer _cooldownTimer, _iconBlinkTimer;
    [SerializeField] RectTransform _objRect;
    [SerializeField] Slider _cooldownSlider;
    [SerializeField] Image _skillIcon;
    [SerializeField] TextMeshProUGUI _inputText;
    RectTransform _iconRect;

    //animations
    [Header("Animation")]
    [SerializeField] Vector2 _endObjScale, _endObjScaleOffset;
    [SerializeField] Vector2 _endIconScale, _endIconScaleOffset;
    Vector3 _objStartScale, _iconStartScale;
    [SerializeField] float _scaleDuration = 0.3f, _blinkDuration;
    TweenAnimatorMultiple _animator;
    Material _startingMaterial;
    [SerializeField] Material _whiteBlinkUIMat;

    public KeyInputTypes InputType => _inputType;

    private void Awake() {
        _animator = GetComponent<TweenAnimatorMultiple>();
    }

    public void Initialize(KeyInputTypes inputType, Sprite skillIcon)
    {
        _inputType = inputType;
        _hotKey = YYInputManager.GetKey(inputType);
        _hotKey.OnKeyPressed += Interact;
        _cooldownBehaviour = false;
        _startingMaterial = _skillIcon.material;
        _iconRect = _skillIcon.rectTransform;
        _objStartScale = _objRect.localScale;
        _iconStartScale = _iconRect.localScale;
        SetSkillIcon(skillIcon);
        _inputText.text = _hotKey.GetInputKeyName();
        name = inputType.ToString() + "_inputItem";
    }
    public void Initialize(KeyInputTypes inputType, Sprite skillIcon, float cooldown)
    {
        Initialize(inputType, skillIcon);
        _cooldownBehaviour = true;
        _cooldown = cooldown;
        _iconBlinkTimer = new(_blinkDuration);
        _iconBlinkTimer.Stop();
        _iconBlinkTimer.onEnd += ResetMaterial;
        _cooldownTimer = new(cooldown);
        _cooldownTimer.Stop();
        _cooldownTimer.onEnd += ReadySkill;
        _cooldownSlider.maxValue = cooldown;
        _cooldownSlider.value = 0f;
        SetCooldown(cooldown);
    }

    private void Update() {
        if(!_cooldownBehaviour) return;
        _cooldownTimer.UpdateTime();
        _iconBlinkTimer.UpdateTime();
        var value = (_skillOnCooldown) ? _cooldownTimer.CurrentTime : 0f;
        _cooldownSlider.value = value;
        if(_skillOnCooldown && value < _blinkDuration - (_cooldown * 0.1f))
        {
            DoBlinkAnimation();
        }
    }

    void Interact()
    {
        if(_skillOnCooldown) return;
        PlayAnimation(_objRect, _objStartScale, _endObjScale, _endObjScaleOffset);
        if(_cooldownBehaviour)StartCooldownTimer();
    }

    void PlayAnimation(RectTransform rect, Vector3 startingScale, Vector3 endScale, Vector3 scaleOffset)
    {
        _animator.TweenScaleBouncy(rect, endScale, scaleOffset, _scaleDuration / 2f, 0, 4,
        onBounceComplete: () =>
        {
            _animator.Scale(rect, startingScale, _scaleDuration / 2f);
        });
    }

    void ReadySkill()
    {
        _skillOnCooldown = false;
    }
    void DoBlinkAnimation()
    {
        if(_skillBlinking) return;
        _skillBlinking = true;
        var startingMat = _skillIcon.material;
        _skillIcon.material = _whiteBlinkUIMat;
        _iconBlinkTimer.Start();
    }

    void ResetMaterial()
    {
        _skillIcon.material = _startingMaterial;
        _skillBlinking = false;
    }

    public void SetSkillIcon(Sprite icon)
    {
        _skillIcon.sprite = icon;
    }
    public void SetCooldown(float cooldown)
    {
        _cooldownTimer.ChangeTime(cooldown);
        _cooldownSlider.maxValue = cooldown;
        _scaleDuration = Mathf.Clamp(_scaleDuration, 0.05f, cooldown / 1.1f);
    }


    void StartCooldownTimer()
    {
        _cooldownTimer.Start();
        _skillOnCooldown = true;
    }

    private void OnDestroy() {
        if(_hotKey != null)_hotKey.OnKeyPressed -= Interact;
        if(_cooldownBehaviour)
        {
            _iconBlinkTimer.onEnd -= ResetMaterial;
            _cooldownTimer.onEnd -= ReadySkill;
        }
    }

}
