using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(TweenAnimatorMultiple))]
public class GameStatePopup : MonoBehaviour
{
    GameStateBase _currentState;
    TweenAnimatorMultiple _animator;
    Canvas _canvas;
    [SerializeField] Image _icon, _bg, _border;
    [SerializeField]GameStateUI[] _states = new GameStateUI[3];
    [SerializeField] TextMeshProUGUI _popupText;
    RectTransform _popupTextRect;
    [SerializeField] AudioSource _audio;

    [Header("Animation")]
    [SerializeField] RectTransform _popupParent;
    [SerializeField] float _animDuration = 0.5f;
    [SerializeField] Vector3 _endScale = Vector3.one, _scaleOffset;
    [SerializeField] int _maxBounces = 4;
    [SerializeField] float _popupTextScaleDuration = 0.5f, _popupTextMoveDuration = 0.3f;
    [SerializeField]
    Vector3 _popupTextStartPosition = Vector3.zero, _popupTextEndPosition, _popupTextPositionOffset, _popupTextEndScale = Vector3.one * 3f, _popupTextEndScaleOffset;

    private void Awake()
    {
        _animator = GetComponent<TweenAnimatorMultiple>();
        _audio = GetComponent<AudioSource>();
        _popupTextRect = _popupText.GetComponent<RectTransform>();
        GameStateManager.OnStateSwitch += SwitchedState;
    }

    private void Start() {
        _canvas = _bg.canvas;
    }

    void SwitchedState(GameStateBase newState)
    {
        _currentState = newState;
        System.Type type = _currentState.GetType();
        int stateIndex = 0;
        if (type == typeof(ConvergenceState)) stateIndex = 0;
        else if (type == typeof(RestState)) stateIndex = 1;
        else if (type == typeof(CraftState)) stateIndex = 2;
        
        SetSprites(_states[stateIndex]);
        PlaySound((_states[stateIndex].changeSfx));
        PlayPopupTextAnimation(_states[stateIndex]);
    }

    void SetSprites(GameStateUI state)
    {
        PlayAnimation();
        _icon.sprite = state.icon;
        _bg.sprite = state.bg;
        _border.sprite = state.border;
    }

    void PlayAnimation()
    {
        _popupParent.localScale = Vector3.up;
        //do the switch animation here
        _animator.TweenScaleBouncy(_popupParent, _endScale, _scaleOffset, _animDuration, 0, _maxBounces);
    }

    void PlayPopupTextAnimation(GameStateUI state)
    {
        _popupText.text = state.PopupAlertName;
        _popupTextRect.localPosition = _popupTextStartPosition;
        var newColor = _popupText.color;
        newColor.a = 0;
        _popupText.color = newColor;
        _animator.TweenTextOpacity(_popupText, 255, _popupTextScaleDuration / 3f);
        _animator.TweenScaleBouncy(_popupTextRect, _popupTextEndScale, _popupTextEndScaleOffset, _popupTextScaleDuration, 0, 3,
        onBounceComplete: () =>
        {
            _animator.Scale(_popupTextRect, Vector3.one, _popupTextScaleDuration / 3f);
            _animator.TweenMoveToBouncy(_popupTextRect, _popupTextEndPosition, _popupTextPositionOffset, _popupTextMoveDuration, 0, 3);
        });
    }

    void PlaySound(AudioClip clip)
    {
        if(_audio == null) return;
        _audio.PlayWithVaryingPitch(clip);
    }

    private void OnDestroy() {
        GameStateManager.OnStateSwitch -= SwitchedState;
    }

    private void OnValidate() {
        if(_states.Length >= 3) System.Array.Resize<GameStateUI>(ref _states, 3);
        for (int i = 0; i < _states.Length; i++)
        {
            var state = _states[i];
            switch(i)
            {
                case 0: state.Name = "RiftState";
                    break;
                case 1: state.Name = "RestState";
                    break;
                case 2: state.Name = "CraftState";
                    break;
            }
        }
    }

    private void OnDrawGizmosSelected() {
        if(Application.isPlaying) return;
        if(_canvas == null) _canvas = _bg.canvas;
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireCube(_canvas.TranslateUiToWorldPoint(_popupTextEndPosition), Vector3.one * 35);
    }
    
}


[System.Serializable]
public class GameStateUI
{
    [HideInInspector]public string Name = string.Empty;
    public Sprite icon, bg, border;
    public AudioClip changeSfx;
    public string PopupAlertName = string.Empty;
}
