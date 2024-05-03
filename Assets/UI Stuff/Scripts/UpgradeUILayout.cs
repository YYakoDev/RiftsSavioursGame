using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UpgradeUILayout : MonoBehaviour
{
    bool _initialized = false;
    bool _stop = false;
    Timer _stopTimer;
    EventSystem _eventSystem;


    [Header("Navigation Stuff")]
    [SerializeField] Vector2 _behindScale;
    [SerializeField] RectTransform[] _upgradeElements = new RectTransform[3];
    [SerializeField] RectTransform[] _bottomButtons = new RectTransform[0];
    RectTransform[] _currentGroup;
    [SerializeField] Vector3[] _upgradesBackPositions = new Vector3[2];
    Vector3[] _positions;
    float _inputDirection;
    float _verticalDirection;
    UISelectionIndex[] _indexes = new UISelectionIndex[2];

    [Header("Animation Stuff")]
    [SerializeField] float _movementAnimDuration = 0.5f;
    [SerializeField] float _scaleAnimDuration = 0.5f;
    TweenAnimatorMultiple _animator;





    private void Awake()
    {
        Initialize();
    }

    private void Start()
    {
        _eventSystem = EventSystem.current;
    }

    void Initialize()
    {
        if (_initialized) return;
        _initialized = true;

        _animator = this.CheckOrAddComponent<TweenAnimatorMultiple>();
        _positions = new Vector3[3]
        {
            _upgradesBackPositions[0],
            Vector3.zero,
            _upgradesBackPositions[1]
        };

        for (int i = 0; i < _indexes.Length; i++)
        {
            int maxLength = (i == 0) ? _upgradeElements.Length : _bottomButtons.Length;
            _indexes[i] = new(maxLength);
        }

        _stopTimer = new(0.2f, useUnscaledTime: true);
        _stopTimer.Stop();
        _stopTimer.onStart += Stop;
        _stopTimer.onEnd += Resume;
        Resume();
    }

    public void SetElements(UpgradeItemPrefab[] elements)
    {
        Initialize();
        //THE ARGUMENT ELEMENTS SHOULD HAVE ONLY 3 ELEMENTS
        RectTransform[] rectComponents = new RectTransform[3];
        for (int i = 0; i < rectComponents.Length; i++)
        {
            rectComponents[i] = elements[i].GetComponent<RectTransform>();
            rectComponents[i].localPosition = _positions[i];
            rectComponents[i].localScale = (i == 1) ? Vector3.one : _behindScale;
        }
        _upgradeElements = rectComponents;
        _currentGroup = _upgradeElements;
        //SwitchFocus(_upgradeElements[1].gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        _stopTimer.UpdateTime();
        if (_stop) return;
        _verticalDirection = Input.GetAxisRaw("Vertical");
        _inputDirection = Input.GetAxisRaw("Horizontal");
        if (_inputDirection != 0) SwitchUIElement();
        if (_verticalDirection != 0) SwitchGroup();
    }

    void SwitchUIElement()
    {
        int dir = (_inputDirection < -0.01f) ? 1 : -1;
        int groupIndex = (_currentGroup == _upgradeElements) ? 0 : 1;
        _indexes[groupIndex].CurrentIndex += dir;

        SetStopTimer(_movementAnimDuration + 0.05f);
        GameObject selectedElement = null;

        if (_currentGroup == _upgradeElements)
        {
            for (int i = 0; i < _upgradeElements.Length; i++)
            {
                var element = _upgradeElements[i];
                var position = _positions[_indexes[groupIndex].CurrentIndex];
                Vector3 scale = (_indexes[groupIndex].CurrentIndex == 1) ? Vector3.one : _behindScale;
                PlayMovementAnimation(element, position);
                PlayScaleAnimation(element, scale);
                if (_indexes[groupIndex].CurrentIndex == 1) selectedElement = element.gameObject;
                _indexes[groupIndex].CurrentIndex++;
            }
        }
        else if (_currentGroup == _bottomButtons)
        {
            var rect = _bottomButtons[_indexes[groupIndex].CurrentIndex];
            selectedElement = rect.gameObject;
            var startingScale = selectedElement.transform.localScale;
            _animator.Scale(rect, startingScale * 1.3f, _scaleAnimDuration / 2f, onComplete: () =>
            {
                _animator.Scale(rect, startingScale, _scaleAnimDuration / 2f);
            });
        }

        if (selectedElement != null) SwitchFocus(selectedElement);
    }

    public void SwitchFocus(GameObject selectedElement)
    {
        _eventSystem.SetSelectedGameObject(selectedElement);
    }

    void SwitchGroup()
    {
        var group = (_verticalDirection > 0.1f) ? _upgradeElements : _bottomButtons;
        SetStopTimer(_scaleAnimDuration + 0.05f);
        _currentGroup = group;
        var groupIndex = (_currentGroup == _upgradeElements) ? 0 : 1;
        for (int i = 0; i < _currentGroup.Length; i++)
        {
            RectTransform element = _currentGroup[i];
            Vector3 currentScale = element.localScale;
            _animator?.Scale(element, currentScale * 1.15f, _scaleAnimDuration / 2.5f, onComplete: () =>
            {
                _animator?.Scale(element, currentScale, _scaleAnimDuration / 4f);
            });
        }
        SwitchFocus(_currentGroup[_indexes[groupIndex].CurrentIndex].gameObject);
    }

    void PlayMovementAnimation(RectTransform element, Vector3 endPos)
    {
        _animator?.TweenMoveToBouncy(element, endPos, Vector3.right * 10f, _movementAnimDuration, 0, 4);
    }
    void PlayScaleAnimation(RectTransform element, Vector3 endScale)
    {
        _animator?.Scale(element, endScale, _scaleAnimDuration);
    }

    public void SetStopTimer(float time)
    {
        _stopTimer.ChangeTime(time);
        _stopTimer.Start();
    }

    public void ResumeInput()
    {
        _stopTimer.Stop();
        Resume();
    }

    void Stop() => _stop = true;
    void Resume() => _stop = false;

    private void OnValidate() {
        if(_upgradeElements.Length > 3) System.Array.Resize<RectTransform>(ref _upgradeElements, 3);
        if(_upgradesBackPositions.Length > 2) System.Array.Resize<Vector3>(ref _upgradesBackPositions, 2);
    }

    private void OnDestroy() {
        _stopTimer.onStart -= Stop;
        _stopTimer.onEnd -= Resume;
    }
}

public struct UISelectionIndex
{
    int _currentIndex;
    int _maxLength;
    public UISelectionIndex(int maxLength, int startingIndex = 0)
    {
        _currentIndex = startingIndex;
        _maxLength = maxLength;
    }
    public int CurrentIndex
    {
        get => _currentIndex;
        set
        {
            _currentIndex = value;
            if (value < 0) _currentIndex = _maxLength - 1;
            if (value >= _maxLength) _currentIndex = 0;
        }
    }
}

