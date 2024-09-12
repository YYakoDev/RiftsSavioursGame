using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
public class FeedbackMenu : MonoBehaviour, ISelectHandler
{
    [SerializeField] MenuQuitter _menuQuitter;
    [SerializeField] RectTransform _menuVisuals;
    [SerializeField] TMP_InputField _inputField;
    [SerializeField] Button _sendButton;

    string _body;
    private EventSystem eventSys;

    private void Start() {
        eventSys = EventSystem.current;
        _inputField.onSelect.AddListener(DisableReturnKey);
        _inputField.onEndEdit.AddListener(GetUserInput);
    }

    private void OnEnable() {
        
    }

    public void Enable()
    {
        _menuVisuals.gameObject.SetActive(true);
        _menuQuitter.SetCurrentMenu(null);
    }

    void DisableReturnKey(string text)
    {
        _menuQuitter.SetCurrentMenu(null);
    }

    public void Disable()
    {
        _menuVisuals.gameObject.SetActive(false);
    }

    void GetUserInput(string inputFieldText)
    {
        _menuQuitter.SetCurrentMenu(_menuVisuals.gameObject);
        _body = inputFieldText;
        if(!eventSys.alreadySelecting) eventSys.SetSelectedGameObject(_sendButton.gameObject);
    }

    public void SendEmail()
    {
        string path = $"https://mail.google.com/mail/?view=cm&fs=1&to=riftsaviours@gmail.com&su=Feedback&body={_body}";
        Application.OpenURL(path);
    }

    private void OnDestroy() {
        _inputField.onSelect.RemoveListener(DisableReturnKey);
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
