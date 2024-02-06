using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
public class FeedbackMenu : MonoBehaviour, ISelectHandler
{

    [SerializeField] TMP_InputField _inputField;
    [SerializeField] Button _sendButton;

    string _body;

    private void Start() {
        _inputField.onEndEdit.AddListener(GetUserInput);
    }
    void GetUserInput(string inputFieldText)
    {
        _body = inputFieldText;
        EventSystem.current.SetSelectedGameObject(_sendButton.gameObject);
    }

    public void SendEmail()
    {
        string path = $"https://mail.google.com/mail/?view=cm&fs=1&to=riftsaviours@gmail.com&su=Feedback&body={_body}";
        Application.OpenURL(path);
    }

    private void OnDestroy() {
        _inputField.onEndEdit.RemoveListener(GetUserInput);
    }

    public void OnSelect(BaseEventData eventData)
    {
        StartCoroutine(SelectInputField());
    }

    IEnumerator SelectInputField()
    {
        yield return null;
        yield return null;
        yield return null;
        EventSystem.current.SetSelectedGameObject(_inputField.gameObject);
    }
}
