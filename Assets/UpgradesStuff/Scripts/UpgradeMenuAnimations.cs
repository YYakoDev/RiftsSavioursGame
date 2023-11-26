using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(TweenAnimator))]
public class UpgradeMenuAnimations : MonoBehaviour
{
    [Header("References")]
    [SerializeField]Canvas _canvas;
    [SerializeField]UpgradeMenuSounds _menuSounds;
    [SerializeField]RectTransform _upgradeMenu, _toolsLeft, _toolsRight, _openingIcon, _table, _background;
    [SerializeField]TweenAnimator _upgradeAnimator, _toolsLeftAnimator, _toolsRightAnimator, _tableAnimator;
    TweenAnimator _twAnimator;
    Image _bgImg;
    [Header("Animation Values")]
    [SerializeField]float _bgFadeInTime = 0.3f;
    [SerializeField]float _iconAnimDuration = 1f;

    Timer _timerForIcon;

    

    [SerializeField]Vector3 _upgradeMenuStartPos, _toolsLeftStartPos, _toolsRightStartPos;
    [SerializeField]Vector3 _openingIconEndSize = Vector3.one;

    [SerializeField]Vector3 _upgradeMenuEndPos, _toolsLeftEndPos, _toolsRightEndPos, _tableEndPos;



    private void Awake() {
        gameObject.CheckComponent<Canvas>(ref _canvas);
        _twAnimator = GetComponent<TweenAnimator>();
        _bgImg = _background.GetComponent<Image>();

        //timer
        _timerForIcon = new(_iconAnimDuration, false, true);


    }

    private void OnEnable() {
        ResetUIState();
    }

    private void Update() {
        _timerForIcon.UpdateTime();
    }

    public void PlayAnimations()
    {
        _twAnimator.TweenImageOpacity(_background, 175f, _bgFadeInTime, CurveTypes.EaseInOut,
                    onComplete: () => 
                    {
                        _openingIcon.gameObject.SetActive(true);
                        _twAnimator.Scale(_openingIcon, _openingIconEndSize, _iconAnimDuration / 2f, CurveTypes.EaseInOut, 
                        onComplete: () => 
                        {
                            _timerForIcon.Start();
                        });
                    });
    }

    void ScaleDownIcon()
    {
         _twAnimator.Scale(_openingIcon, Vector3.zero, _iconAnimDuration, CurveTypes.EaseInOut, 
                            onComplete: () =>
                            {
                                PlayRestOfAnimations();
                                _openingIcon.gameObject.SetActive(false);
                            });
    }

    void PlayRestOfAnimations()
    {
        _tableAnimator.MoveTo(_table, _tableEndPos, 0.3f, CurveTypes.EaseInOut);
        _toolsRightAnimator.MoveTo(_toolsRight, _toolsRightEndPos, 0.4f, CurveTypes.EaseInOut);
        _toolsLeftAnimator.MoveTo(_toolsLeft, _toolsLeftEndPos, 0.4f, CurveTypes.EaseInOut); 
        _upgradeAnimator.MoveTo(_upgradeMenu, _upgradeMenuEndPos, 0.65f, CurveTypes.EaseInOut);
    }

    void ResetUIState()
    {
        //Background
        Color bgColor = _bgImg.color;
        bgColor.a = 0;
        _bgImg.color = bgColor;

        _upgradeMenu.localPosition = _upgradeMenuStartPos;
        _upgradeMenu.localScale = Vector3.one;
        _table.localPosition = _upgradeMenuStartPos;
        _toolsLeft.localPosition = _toolsLeftStartPos;
        _toolsRight.localPosition = _toolsRightStartPos;
        _openingIcon.localScale = Vector3.zero;

        _openingIcon.gameObject.SetActive(false);
        _timerForIcon.onEnd += ScaleDownIcon;
        _timerForIcon.Stop();
        //ClearAnimations();

    }

    public void PlayCloseAnimations()
    {
        _tableAnimator.MoveTo(_table, _upgradeMenuStartPos, 0.2f, CurveTypes.EaseInOut);
        _toolsLeftAnimator.MoveTo(_toolsLeft, _toolsLeftStartPos, 0.25f, CurveTypes.EaseInOut);
        _toolsRightAnimator.MoveTo(_toolsRight, _toolsRightStartPos, 0.25f, CurveTypes.EaseInOut);
        _twAnimator.TweenImageOpacity(_background, 0, 0.3f, CurveTypes.EaseInOut);
        _upgradeAnimator.Scale(_upgradeMenu, Vector3.zero, 0.45f, CurveTypes.EaseInOut,
            onComplete: () => 
            {
                gameObject.SetActive(false);
            });
    }

    public void ClearAnimations()
    {
        _twAnimator.Clear();
        _tableAnimator.Clear();
        _toolsLeftAnimator.Clear();
        _toolsRightAnimator.Clear();
        _upgradeAnimator.Clear();
    }

    private void OnDisable() {
        _timerForIcon.onEnd -= ScaleDownIcon;
    }


    private void OnDrawGizmosSelected() {
        if(_canvas == null)
        {
            _canvas = _upgradeMenu.root.GetComponentInChildren<Canvas>();
        }

        Gizmos.color = Color.magenta;
        Gizmos.DrawWireCube(_canvas.TranslateUiToWorldPoint(_upgradeMenuEndPos), Vector3.one * 55);
        Gizmos.DrawWireCube(_canvas.TranslateUiToWorldPoint(_toolsLeftEndPos), Vector3.one * 31);
        Gizmos.DrawWireCube(_canvas.TranslateUiToWorldPoint(_toolsRightEndPos), Vector3.one * 31);
        Gizmos.DrawWireCube(_canvas.TranslateUiToWorldPoint(_tableEndPos), Vector3.one * 45);
    }
}
