using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class MainHubCharacterSelector : ScrollSelectionUI<SOCharacterData>
{
    [SerializeField]RectTransform _roomBG, _uiBG;
    Animator _roomBGAnimator, _uiBGAnimator;
    [SerializeField] FadeImage _fadeImg;
    bool _fadeInAlreadyPlayed = false;
    float _roomBGFadeInDuration = 1f, _uiBGEnterDuration = 0.5f;

    //this is the start method ;)
    protected override void Initialize()
    {
        base.Initialize();
        onConfirm += SetSelectedCharacter;
        _roomBGAnimator = _roomBG.GetComponent<Animator>();
        _uiBGAnimator = _uiBG.GetComponent<Animator>();
        _uiBG.gameObject.SetActive(true);
        _roomBG.gameObject.SetActive(true);
        YYExtensions.i.GetAnimatorStateLength(_roomBGAnimator, "FadeIn", SetFadeInDuration);
        YYExtensions.i.GetAnimatorStateLength(_uiBGAnimator, "EnterAnimation", SetEnterDuration);

        void SetFadeInDuration(float duration)
        {
            _roomBGFadeInDuration = duration;
            _roomBG.gameObject.SetActive(false);
        }
        void SetEnterDuration(float duration)
        {
            _uiBGEnterDuration = duration;
            _uiBG.gameObject.SetActive(false);
        }
    }

    public void Open(bool ignoreFadeIn = false)
    {
        if(!_initialized)Initialize();
        YYInputManager.StopInput();
        PauseMenuManager.DisablePauseBehaviour(true);
        _eventSys.SetSelectedGameObject(_confirmButton.gameObject);
        PlayOpeningAnimation(ignoreFadeIn);
    }
    protected override void Scroll(int dir)
    {
        base.Scroll(dir);
        var character = _selectableDatalist[CurrentIndex];
        _characterIcon.sprite = character.Sprite;
    }

    void PlayOpeningAnimation(bool ignoreFadeIn)
    {
        //if the animation has already been played just go directly with the background animation
        if(_fadeInAlreadyPlayed || ignoreFadeIn)
        {
            YYExtensions.i.ExecuteMethodAfterFrame(() =>
            {
                _uiBG.gameObject.SetActive(true);
                _uiBGAnimator.enabled = true;
                PlayBackgroundExitAnimation();
            }, 3);
            
            return;
        }
        _fadeInAlreadyPlayed = true;
        _roomBGAnimator.enabled = true;
        _uiBGAnimator.enabled = true;
        _fadeImg.FadeIn(FadeOut, 0.2f);
        void FadeOut()
        {
            Debug.Log("Fading out opening anim");
            _fadeImg.FadeOut(duration: 0.4f);
            _roomBG.gameObject.SetActive(true);
            YYExtensions.i.PlayAnimationWithEvent(_roomBGAnimator, "FadeIn", PlayBackgroundExitAnimation);
        }
    }

    void PlayBackgroundExitAnimation()
    {
        Debug.Log("Playing BackgroundExitAnimation");
        _uiBG.gameObject.SetActive(true);
        YYExtensions.i.ExecuteMethodAfterFrame(() =>
        {
            _menuParent.SetActive(true);
            YYExtensions.i.PlayAnimationWithEvent(_uiBGAnimator, "EnterAnimation", _uiBGEnterDuration, () => 
            {
                base.Open();
                _uiBGAnimator.enabled = false;
            });
            YYExtensions.i.SetAnimatorTriggerWithEvent(_roomBGAnimator, "Exit", _roomBGFadeInDuration, () =>
            {
                _roomBGAnimator.enabled = false;
            });
        });
        
        var character = _selectableDatalist[CurrentIndex];
        _characterIcon.sprite = character.Sprite;
        if(PlayerManager.SelectedChara != null)  _closeButton.gameObject.SetActive(true);
        else
            // this means you can only exit the menu once you have created a character!
        _closeButton.gameObject.SetActive(false);



    }
    void SetSelectedCharacter(SOCharacterData character) => PlayerManager.ChangeSelectedCharacter(character);

    public override void Close(bool skipAudio = false)
    {
        _uiBG.gameObject.SetActive(false);
        _roomBG.gameObject.SetActive(false);
        base.Close(skipAudio);
        //maybe do a closing animation?
    }

    private void OnDestroy() {
        onConfirm -= SetSelectedCharacter;
    }
}

