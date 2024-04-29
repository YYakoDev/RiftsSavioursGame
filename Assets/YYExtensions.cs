using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YYExtensions : MonoBehaviour
{
    //instance extensions for things not possible in static classes
    public static YYExtensions i;
    Timer[] _animationsTimers = new Timer[0];
    bool[] _timersAvailability = new bool[0];
    private void Awake() {
        if(i != null && i != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            i = this;
            i.transform.SetParent(null);
            DontDestroyOnLoad(i.gameObject);    
        }
    }

    void GetAnimatorInfoBase(Animator animator, string stateName, Action method)
    {
        var currentState = animator.GetCurrentAnimatorStateInfo(0).shortNameHash;
        animator.Play(stateName);
        StartCoroutine(ActionAfterFrame(1, ExecuteCallback));
        void ExecuteCallback()
        {
            animator.Play(currentState);
            method?.Invoke();
        }
    }
    public void GetAnimatorStateLength(Animator animator, string stateName, Action<float> callback)
    {
        GetAnimatorInfoBase(animator, stateName, GetLength);
        void GetLength()
        {
            var value = animator.GetCurrentAnimatorStateInfo(0).length;
            callback?.Invoke(value);
        }
    }
    public void GetAnimatorStateHash(Animator animator, string stateName, Action<int> callback)
    {
        GetAnimatorInfoBase(animator, stateName, GetHash);
        void GetHash()
        {
            var value = animator.GetCurrentAnimatorStateInfo(0).fullPathHash;
            callback?.Invoke(value);
        }
    }
    public void GetAnimatorStateInfo(Animator animator, string stateName, Action<AnimatorStateInfo> callback)
    {
        GetAnimatorInfoBase(animator, stateName, GetInfo);
        void GetInfo()
        {
            var value = animator.GetCurrentAnimatorStateInfo(0);
            callback?.Invoke(value);
        }
    }

    public void PlayAnimationWithEvent(Animator animator, string stateName, Action onComplete)
    {
        if(animator == null) return;
        GetAnimatorStateInfo(animator, stateName, Callback);
        void Callback(AnimatorStateInfo state)
        {
            AddEventToTimer(GetTimerIndex(state.length + Time.deltaTime), onComplete);
            ExecuteMethodAfterFrame(() =>
            {
                animator.Play(stateName);
            });

        }
    }
    public void PlayAnimationWithEvent(Animator animator, string stateName, float animDuration, Action onComplete)
    {
        if(animator == null) return;
        animator.Play(stateName);
        AddEventToTimer(GetTimerIndex(animDuration), onComplete);
    }
    public void SetAnimatorTriggerWithEvent(Animator animator, string triggerName, Action onComplete)
    {
        if(animator == null) return;
        GetAnimatorStateInfo(animator, triggerName, Callback);

        void Callback(AnimatorStateInfo state)
        {
            AddEventToTimer(GetTimerIndex(state.length + Time.deltaTime), onComplete);
            ExecuteMethodAfterFrame(() =>
            {
                animator.SetTrigger(triggerName);
            });
            
        }
    }
    public void SetAnimatorTriggerWithEvent(Animator animator, string triggerName, float animDuration, Action onComplete)
    {
        if(animator == null) return;
        animator.SetTrigger(triggerName);
        AddEventToTimer(GetTimerIndex(animDuration), onComplete);
    }

    public void ExecuteEventAfterTime(float time, Action onComplete)
    {
        AddEventToTimer(GetTimerIndex(time), onComplete);
    }

    private void Update() {
        for (int i = 0; i < _animationsTimers.Length; i++)
        {
            var availability = _timersAvailability[i];
            if(availability) continue;
            var timer = _animationsTimers[i];
            timer?.UpdateTime();
        }
    }

    void AddEventToTimer(int timerIndex, Action onComplete)
    {
        Action onTimerEndAction = onComplete + RemoveEvent;
        _animationsTimers[timerIndex].onEnd += onTimerEndAction;
        void RemoveEvent()
        {
            _timersAvailability[timerIndex] = false;
            _animationsTimers[timerIndex].ClearOnEndEvent();
        }
    }

    int CreateTimer(float time)
    {
        Timer timer = new(time);
        timer.Stop();
        var currentLength = _animationsTimers.Length;
        Array.Resize<Timer>(ref _animationsTimers, currentLength + 1);
        Array.Resize<bool>(ref _timersAvailability, currentLength + 1);
        _animationsTimers[currentLength] = timer;
        _timersAvailability[currentLength] = true;
        return currentLength;
    }

    int GetTimerIndex(float timeToSet)
    {
        int index = -1;
        for (int i = 0; i < _animationsTimers.Length; i++)
        {
            if(_timersAvailability[i])
            {
                var timer = _animationsTimers[i];
                timer.ChangeTime(timeToSet);
                timer.Start();
                _timersAvailability[i] = false;
                index = i;
                break;
            }
        }
        if(index == -1)
        {
            var newIndex = CreateTimer(timeToSet);
            _timersAvailability[newIndex] = false;
            _animationsTimers[newIndex].Start();
            return newIndex;
        }
        return index;
    }
    public void ExecuteMethodAfterFrame(Action method, int frames = 1)
    {
        StartCoroutine(ActionAfterFrame(frames, method));
    }

    IEnumerator ActionAfterFrame(int framesToSkip, Action method)
    {
        for (int i = 0; i < framesToSkip; i++)
        {
            yield return null;
        }
        method?.Invoke();                
    }

    private void OnDestroy() {
    }
}
