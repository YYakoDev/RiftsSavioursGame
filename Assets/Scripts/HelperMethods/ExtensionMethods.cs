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
    public static void ForcePlay(this Animator animator, int animationHash)
    {
        animator.StopPlayback();
        animator.Play(animationHash);
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

    public static T CheckOrAddComponent<T>(this Component component) where T : Component
    {
        T componentToCheck;
        
        if(!component.TryGetComponent<T>(out componentToCheck))
        {
            componentToCheck = component.gameObject.AddComponent<T>();
        }

        return componentToCheck;
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



    //UI POSITION TRANSLATION 
    public static Vector2 TranslateUiToWorldPoint(this Canvas canvas, Vector2 position)
    {
        float scaleFactor = canvas.scaleFactor;
        var canvasRect = canvas.GetComponent<RectTransform>();
        float canvasCenterX = canvasRect.position.x; //this two positions are already expressed as world points and also its the center of the canvas
        float canvasCenterY = canvasRect.position.y;

        //float percentX = position.x / canvasWidth;
        //float percentY = position.y / canvasHeight;

        Vector2 worldPoint = new Vector2
        (
            canvasCenterX + position.x * scaleFactor,
            canvasCenterY + position.y * scaleFactor            
        );
        return worldPoint;
    }

    public static Vector2 TranslateWorldPointToUI(this Canvas canvas, Vector2 worldPoint)
    {
        Camera camera = Camera.main;
        worldPoint = camera.WorldToScreenPoint(worldPoint);
        float screenCenterX = camera.scaledPixelWidth/2f;
        float screenCenterY = camera.scaledPixelHeight/2f;

        var canvasRect = canvas.GetComponent<RectTransform>();
        float canvasCenterX = canvasRect.position.x; //this two positions are already expressed as world points and also its the center of the canvas
        float canvasCenterY = canvasRect.position.y;

        Debug.Log($"Camera Center X:  {screenCenterX} \n Camera Center Y: {screenCenterY} ");
        float scaleFactor = canvas.scaleFactor;

        Vector2 test = new Vector2(screenCenterX, screenCenterY);
        //float percentX = position.x / canvasWidth;
        //float percentY = position.y / canvasHeight;

        Vector2 uiPoint = new Vector2
        (
            screenCenterX - worldPoint.x / scaleFactor,
            screenCenterY - worldPoint.y / scaleFactor
        );
        return uiPoint;
    }




}
