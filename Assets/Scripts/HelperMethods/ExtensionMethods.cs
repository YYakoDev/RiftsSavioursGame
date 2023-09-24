using System;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public static class ExtensionMethods
{
    //ANIMATOR STUFF
    public static void PlayWithEndTime(this Animator animator,string animationName)
    {
        if(animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.98f) animator.Play(animationName);
    }
    public static void ForcePlay(this Animator animator, string animationName)
    {
        animator.StopPlayback();
        animator.Play(animationName);
    }

    //TRY TO GET COMPONENT IF ITS NOT NULL
    public static void CheckComponent<T>(this GameObject gameObj, ref T component) where T: Component
    {
        if(component != null)return;

        if(gameObj.TryGetComponent<T>(out T desiredComponent))
        {
            component = desiredComponent;
        }
        else
        {
            Debug.LogError($" Couldn't get the component {typeof(T)}  of  {gameObj.name} ");
        }
    }

    //AUDIO STUFF
    public static void PlayWithVaryingPitch(this AudioSource audio, AudioClip clip)
    {
        float pitchOffset = Random.Range(-0.02f, 0.035f);
        audio.pitch = 1 + pitchOffset;
        audio.PlayOneShot(clip);
    }


    // BUTTONS
    public static void AddEventListener(this Button button, Action OnClick)
    {
        button.onClick.AddListener(() => {OnClick(); });
    }
    public static void RemoveAllEvents(this Button button)
    {
        button.onClick.RemoveAllListeners();
    }
}
