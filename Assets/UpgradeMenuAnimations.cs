using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeMenuAnimations : MonoBehaviour
{
    [SerializeField]RectTransform _upgradeMenu, _toolsLeft, _toolsRight;
    [SerializeField]Image _background;
    [SerializeField]float _iconAnimDuration = 1f;
    Timer _timer;

    private void Awake() {
        _timer = new(_iconAnimDuration, false);
    }

    private void OnEnable() {
        _timer.onReset += PlayRestOfAnimations;
    }

    private void Update() {
        _timer.UpdateTime();
    }
    public void PlayAnimations()
    {
        _timer.SetActive(true);
    }

    void PlayRestOfAnimations()
    {

    }

    private void OnDisable() {
        _timer.onReset -= PlayRestOfAnimations;
    }
}
