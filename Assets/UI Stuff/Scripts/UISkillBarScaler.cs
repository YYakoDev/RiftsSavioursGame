using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISkillBarScaler : MonoBehaviour
{
    RectTransform _barRect;
    [SerializeField] float _widthOffset;
    [SerializeField] RectTransform _skillsParent;
    [SerializeField] HorizontalLayoutGroup _layoutGroup;
    private void Awake() {
        _barRect = GetComponent<RectTransform>();
    }
    private IEnumerator Start() {
        _layoutGroup = _skillsParent.GetComponent<HorizontalLayoutGroup>();
        ScreenController.OnScreenChange += ResizeSkillsGroup;
        yield return null;
        yield return null;
        yield return null;
        var childCount = _skillsParent.childCount;
        var childWidth = _skillsParent.GetChild(0).GetComponent<RectTransform>().sizeDelta.x;
        var desiredWidth = (childWidth * childCount) + _widthOffset;
        var startingPos = _barRect.localPosition;
        startingPos.x = (startingPos.x - (_barRect.sizeDelta.x / 2f)) +(desiredWidth / 2f);
        _barRect.localPosition = startingPos;
        var scale = _barRect.sizeDelta;
        scale.x = desiredWidth;
        _barRect.sizeDelta = scale;

    }

    void ResizeSkillsGroup()
    {
        _layoutGroup.CalculateLayoutInputHorizontal();
    }

    private void OnDestroy() {
        ScreenController.OnScreenChange -= ResizeSkillsGroup;
    }
}
