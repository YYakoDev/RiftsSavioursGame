using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(TweenAnimatorMultiple))]
public class UISkill : MonoBehaviour
{
    InputActionReference _inputType;
    bool _cooldownBehaviour, _skillOnCooldown, _skillBlinking;
    float _cooldown = 0f;
    Timer _cooldownTimer, _iconBlinkTimer;
    [SerializeField] RectTransform _objRect;
    [SerializeField] Slider _cooldownSlider;
    [SerializeField] Image _skillIcon;
    [SerializeField] TextMeshProUGUI _inputText;

    //animations
    [Header("Animation")]
    [SerializeField] Vector2 _endObjScale, _endObjScaleOffset;
    Vector3 _objStartScale;
    Vector2 _objStartSizeDelta;
    [SerializeField] float _scaleDuration = 0.3f, _blinkDuration;
    TweenAnimatorMultiple _animator;
    Material _startingMaterial;
    [SerializeField] Material _whiteBlinkUIMat;

    public InputActionReference InputType => _inputType;

    private void Awake()
    {
        _animator = GetComponent<TweenAnimatorMultiple>();
    }

    public void Initialize(InputActionReference inputType, Sprite skillIcon)
    {
        _inputType = inputType;
        //var hotKey = YYInputManager.GetKey(inputType);
        //_hotKey.OnKeyPressed += Interact;
        _cooldownBehaviour = false;
        _startingMaterial = _skillIcon.material;
        _objStartScale = _objRect.localScale;
        _objStartSizeDelta = _objRect.sizeDelta;
        UpdateInputText(inputType);
        UpdateSkillIcon(skillIcon);
        //_inputText.text = hotKey.GetInputKeyName();
        name = inputType.action.name + "_inputItem";
    }
    public void Initialize(InputActionReference inputType, Sprite skillIcon, float cooldown)
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

        UpdateCooldown(cooldown);

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

    public void Interact()
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

    public void UpdateSkillIcon(Sprite icon)
    {
        _skillIcon.sprite = icon;
    }
    public void UpdateCooldown(float cooldown)
    {
        _cooldownTimer.ChangeTime(cooldown);
        _cooldownSlider.maxValue = cooldown;
        _scaleDuration = Mathf.Clamp(_scaleDuration, 0.05f, cooldown / 1.1f);
    }
    public void UpdateInteractionTrigger(ref Action previousTrigger, ref Action newTrigger)
    {
        previousTrigger -= Interact;
        newTrigger += Interact;
    }
    public void UpdateInputText(InputActionReference inputType)
    {
        var text = inputType.action.GetBindingDisplayString();
        _inputText.SetText(text);
        _inputText.ForceMeshUpdate();
        var textSize = _inputText.GetRenderedValues(true);
        var sizeDiff = new Vector2
        (
            _objStartSizeDelta.x - textSize.x,
            _objStartSizeDelta.y - textSize.y
        );
        if(sizeDiff.x > 0) sizeDiff.x = 0;
        if(sizeDiff.y > 0) sizeDiff.y = 0;
        _objRect.sizeDelta = _objStartSizeDelta + sizeDiff + new Vector2(50f, 35f);
        _inputText.rectTransform.sizeDelta = textSize / 1.3f;
        if(textSize.x > _objRect.sizeDelta.x)
        {
            _inputText.enableWordWrapping = true;
        }
    }

    public Vector2 GetItemSize()
    {
        return _objRect.sizeDelta;
    }
    public void SetItemSize(Vector2 sizeDelta)
    {
        _objRect.sizeDelta = sizeDelta;
    }

    void StartCooldownTimer()
    {
        _cooldownTimer.Start();
        _skillOnCooldown = true;
    }

    private void OnDestroy() {
        //if(_hotKey != null)_hotKey.OnKeyPressed -= HotkeyPressed;
        if(_cooldownBehaviour)
        {
            _iconBlinkTimer.onEnd -= ResetMaterial;
            _cooldownTimer.onEnd -= ReadySkill;
        }
    }

}
