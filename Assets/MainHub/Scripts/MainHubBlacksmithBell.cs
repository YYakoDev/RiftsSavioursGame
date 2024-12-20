using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainHubBlacksmithBell : MonoBehaviour, IInteractable
{
    [SerializeField]Vector3 _buttonOffset;
    [SerializeField] Animator _bellAnimator, _vulcanusAnimator, _shadowAnimator;
    [SerializeField] VulcanusInteraction _vulcanusInteractionLogic;
    bool _alreadyInteracted;
    public Vector3 Offset => _buttonOffset;
    public bool AlreadyInteracted { get => _alreadyInteracted; set => _alreadyInteracted = value; }
    float _animLength;
    Timer _animTimer, _bellTimer;

    //audio stuff
    [SerializeField] AudioClip _bellSfx;
    public AudioClip InteractSfx => _bellSfx;

    private void Awake() {
        _bellTimer = new(0.5f);
        _bellTimer.Stop();
        _bellTimer.onEnd += PlayVulcanusAnimation;
    }

    private void Start() {
        var clips = _vulcanusAnimator.runtimeAnimatorController.animationClips;
        foreach(var clip in clips)
        {
            if(clip.name == "VulcanusAnimation")
            {
                _animLength = clip.averageDuration;
                break;
            }
        }
        _animTimer = new(_animLength - 0.15f);
        _animTimer.Stop();
        _animTimer.onEnd += AnimationEnd;
        _vulcanusAnimator.gameObject.SetActive(false);
        _vulcanusInteractionLogic.AlreadyInteracted = true;
    }

    private void Update() {
        _bellTimer.UpdateTime();
        _animTimer.UpdateTime();
    }

    public void Interact()
    {
        _bellAnimator.Play("Animation");
        _bellTimer.Start();
        YYInputManager.StopInput();
    }

    void PlayVulcanusAnimation()
    {
        _vulcanusAnimator.gameObject.SetActive(true);
        _vulcanusAnimator.Play("Animation");
        _animTimer.Start();
        _shadowAnimator.gameObject.SetActive(false);
    }

    void AnimationEnd()
    {
        YYInputManager.ResumeInput();
        _shadowAnimator.gameObject.SetActive(true);
        _shadowAnimator.Play("Animation");
        _vulcanusInteractionLogic.AlreadyInteracted = false;
    }

    private void OnDestroy() {
        _bellTimer.onEnd -= PlayVulcanusAnimation;
        _animTimer.onEnd -= AnimationEnd;
    }


}
