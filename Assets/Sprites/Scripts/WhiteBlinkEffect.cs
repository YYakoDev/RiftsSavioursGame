using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhiteBlinkEffect : MonoBehaviour
{
    [SerializeField] Material _blinkMat;
    private Material _originalMat;
    [SerializeField]private float _duration = 0.15f;
    [SerializeField]private SpriteRenderer _spriteRenderer;
    Timer _blinkTimer;

    void Awake()
    {
        if(_spriteRenderer == null) _spriteRenderer = this.GetComponent<SpriteRenderer>();
        _originalMat = _spriteRenderer.material;
        _blinkTimer = new(_duration);
        _blinkTimer.Stop();
        _blinkTimer.onStart += BlinkEffect;
        _blinkTimer.onEnd += Stop;
    }

    // Start is called before the first frame update
    void Start()
    {
        if(_blinkMat == null) _blinkMat = Resources.Load<Material>("WhiteBlinkMat");
        if(_originalMat == _blinkMat)
        {
            _originalMat = _spriteRenderer.material;
        }
    }

    private void Update() {
        _blinkTimer.UpdateTime();
    }

    public void Play()
    {
        _blinkTimer.ChangeTime(_duration);
        _blinkTimer.Start();
    }
    public void Play(float duration)
    {
        _blinkTimer.ChangeTime(duration);
        _blinkTimer.Start();
    }

    public void Stop()
    {
        _spriteRenderer.material = _originalMat;
    }

    void BlinkEffect()
    {
        _spriteRenderer.material = _blinkMat;
    }

    private void OnDestroy() {
        _blinkTimer.onStart -= BlinkEffect;
        _blinkTimer.onEnd -= Stop;
    }
}
