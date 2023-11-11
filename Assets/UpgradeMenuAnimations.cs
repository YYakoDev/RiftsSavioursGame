using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(TweenAnimator))]
public class UpgradeMenuAnimations : MonoBehaviour
{
    [Header("References")]
    [SerializeField]Canvas _canvas;
    [SerializeField]RectTransform _upgradeMenu, _toolsLeft, _toolsRight, _openingIcon, _table, _background;
    [SerializeField]TweenAnimator _upgradeAnimator, _toolsLeftAnimator, _toolsRightAnimator, _tableAnimator;
    TweenAnimator _twAnimator;
    Image _bgImg;
    [Header("Animation Values")]
    [SerializeField]float _bgFadeInTime = 0.3f;
    [SerializeField]float _iconAnimDuration = 1f;
    Timer _timer;

    

    [SerializeField]Vector3 _upgradeMenuStartPos, _toolsLeftStartPos, _toolsRightStartPos;

    [SerializeField]Vector3 _upgradeMenuEndPos, _toolsLeftEndPos, _toolsRightEndPos;



    private void Awake() {
        _timer = new(_iconAnimDuration, false);
        gameObject.CheckComponent<Canvas>(ref _canvas);
        _twAnimator = GetComponent<TweenAnimator>();
        _bgImg = _background.GetComponent<Image>();
    }

    private void OnEnable() {
        _timer.onReset += PlayRestOfAnimations;
        //changing bg color to 0 just in case
        ResetUIState();

        _timer.SetActive(false);
    }

    private void Update() {
        _timer.UpdateTime();
    }
    public void PlayAnimations()
    {
        _twAnimator.TweenImageOpacity(_background, 175f, _bgFadeInTime, CurveTypes.EaseInOut,
                    onComplete: () => 
                    {
                        _timer.SetActive(true);
                        _openingIcon.gameObject.SetActive(true);
                    });
    }

    void PlayRestOfAnimations()
    {
        _upgradeAnimator.MoveTo(_upgradeMenu, _upgradeMenuEndPos, 1f);
        _tableAnimator.MoveTo(_table, _upgradeMenuEndPos, 1f);
        //_twAnimator.MoveTo(_openingIcon, _upgradeMenuStartPos, 0f);
        _toolsLeftAnimator.MoveTo(_toolsLeft, _toolsLeftEndPos, 1f);
        _toolsRightAnimator.MoveTo(_toolsRight, _toolsRightEndPos, 1f);
    }

    void ResetUIState()
    {
        //Background
        Color bgColor = _bgImg.color;
        bgColor.a = 0;
        _bgImg.color = bgColor;

        //opening icon
        _twAnimator.MoveTo(_upgradeMenu, _upgradeMenuStartPos, 0f);
        _twAnimator.MoveTo(_table, _upgradeMenuStartPos, 0f);
        //_twAnimator.MoveTo(_openingIcon, _upgradeMenuStartPos, 0f);
        _twAnimator.MoveTo(_toolsLeft, _toolsLeftStartPos, 0f);
        _twAnimator.MoveTo(_toolsRight, _toolsRightStartPos, 0f);

        _openingIcon.gameObject.SetActive(false);

    }

    private void OnDisable() {
        _timer.onReset -= PlayRestOfAnimations;
    }

    private void OnDrawGizmosSelected() {
        if(_canvas == null)
        {
            _canvas = _upgradeMenu.root.GetComponentInChildren<Canvas>();
        }
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(_canvas.TranslateUiToWorldPoint(_upgradeMenuStartPos), Vector3.one * 35);
        Gizmos.DrawWireCube(_canvas.TranslateUiToWorldPoint(_toolsLeftStartPos), Vector3.one * 35);
        Gizmos.DrawWireCube(_canvas.TranslateUiToWorldPoint(_toolsRightStartPos), Vector3.one * 35);

        Gizmos.color = Color.magenta;
        Gizmos.DrawWireCube(_canvas.TranslateUiToWorldPoint(_upgradeMenuEndPos), Vector3.one * 35);
        Gizmos.DrawWireCube(_canvas.TranslateUiToWorldPoint(_toolsLeftEndPos), Vector3.one * 35);
        Gizmos.DrawWireCube(_canvas.TranslateUiToWorldPoint(_toolsRightEndPos), Vector3.one * 35);
    }
}
