using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhiteBlinkEffect : MonoBehaviour
{
    private static Material _blinkMat;
    [SerializeField]private float _duration = 0.15f;
    [SerializeField]private SpriteRenderer _spriteRenderer;
    private Material _originalMat;
    private Coroutine _blinkRoutine;
    bool isRoutineRunning;

    void Awake()
    {
        if(_spriteRenderer == null) _spriteRenderer = this.GetComponent<SpriteRenderer>();
        _originalMat = _spriteRenderer.material;
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
    public void Play()
    {
        //this checks if the blink effect coroutine is currently running
        if(isRoutineRunning)
        {
            //if so stop it so we dont have any errors
//            Debug.Log("Stopping blink routine");
            Stop();
        }

        _blinkRoutine = StartCoroutine(BlinkEffect());
//        Debug.Log("Starting blink routine");

    }

    public void Stop()
    {
        if(_blinkRoutine != null)StopCoroutine(_blinkRoutine);
        isRoutineRunning = false;
        _spriteRenderer.material = _originalMat;
    }

    private IEnumerator BlinkEffect()
    {
        isRoutineRunning = true;
        _spriteRenderer.material = _blinkMat;
        yield return new WaitForSecondsRealtime(_duration);
        _spriteRenderer.material = _originalMat;

        isRoutineRunning = false;
    }
}
