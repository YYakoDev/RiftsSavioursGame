using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(TweenAnimator))]
public class UpgradeMenuAnimations : MonoBehaviour
{
    TweenAnimator _twAnimator;
    CanvasScaler _canvasScaler;
    [SerializeField]private Vector2 _screenSize;
    [SerializeField]RectTransform _upgradeMenu, _toolsLeft, _toolsRight, _openingIcon;
    [SerializeField]Image _background;
    [SerializeField]float _bgFadeInTime = 0.3f;
    [SerializeField]float _iconAnimDuration = 1f;
    Timer _timer;

    [SerializeField]Vector3 _upgradeMenuEndPos, _toolsLeftEndPos, _toolsRightEndPos;



    private void Awake() {
        _timer = new(_iconAnimDuration, false);
        _canvasScaler = _upgradeMenu.root.GetComponentInChildren<CanvasScaler>();
        _twAnimator = GetComponent<TweenAnimator>();
    }

    private void OnEnable() {
        _timer.onReset += PlayRestOfAnimations;
        //changing bg color to 0 just in case
        Color bgColor = _background.color;
        bgColor.a = 0;
        _background.color = bgColor;

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

    }

    private void OnDisable() {
        _timer.onReset -= PlayRestOfAnimations;
    }

    private void OnValidate() {
        _screenSize = new Vector2(Display.main.renderingWidth, Display.main.renderingHeight);
    }

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.magenta;

        Gizmos.DrawWireCube(_upgradeMenuEndPos, Vector3.one * 35);
        Gizmos.DrawWireCube(_toolsLeftEndPos, Vector3.one * 35);
        Gizmos.DrawWireCube(_toolsRightEndPos, Vector3.one * 35);
    }
}
