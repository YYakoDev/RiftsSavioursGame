using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueObject : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _textObj;
    [SerializeField] Canvas _canvas;
    [SerializeField] RectTransform _textContainer;
    [SerializeField] Vector2 _padding = new Vector2(20f, 14f);
    Timer _dialogueTimer;
    Action _onDialogueEnd;
    private void Awake() {
        _dialogueTimer = new(0f);
        _dialogueTimer.Stop();
        _dialogueTimer.onEnd += _onDialogueEnd;
    }

    private void Start() {
        _canvas.worldCamera = Camera.main;
    }

    public void SetDialogue(string text, float duration, Action onDialogueEnd = null)
    {
        _textObj.text = text;
        _dialogueTimer.ChangeTime(duration);
        _dialogueTimer.Start();
        _onDialogueEnd = onDialogueEnd;

        _canvas.gameObject.SetActive(true);
        _textObj.SetText(text);
        _textObj.ForceMeshUpdate();   
        Vector2 textSize = _textObj.GetRenderedValues(false);
        _textContainer.sizeDelta = textSize + _padding;
        _textContainer.localPosition = new Vector3(_textContainer.sizeDelta.x /20f,0f);
            //rt.localPosition = new Vector3(rt.sizeDelta.x/2f,0f) + offset;
        
    }

    public void Update()
    {
        _dialogueTimer.UpdateTime();
    }
    private void OnDestroy() {
        _dialogueTimer.onEnd -= _onDialogueEnd;
    }
}
