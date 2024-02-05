using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(TweenAnimatorMultiple))]
public class ResourcesUIPopupManager : MonoBehaviour
{
    TweenAnimatorMultiple _animator;
    ResourceUIPopup[] _popupsInstances = new ResourceUIPopup[3];
    CraftingMaterial[] _assignedMaterials = new CraftingMaterial[3];
    [SerializeField] ResourceUIPopup _popupPrefab;
    [SerializeField] float _animDuration = 0.5f, _scaleOutDuration = 1f;
    [SerializeField] Vector3 _startingScale = Vector3.one;
    bool _lockChanges = false;

    private void Awake() {
        _animator = GetComponent<TweenAnimatorMultiple>();
        CreateItems();
    }

    void CreateItems()
    {
        for (int i = 0; i < _popupsInstances.Length; i++)
        {
            _popupsInstances[i] = Instantiate(_popupPrefab);
            _popupsInstances[i].transform.SetParent(transform);
            _popupsInstances[i].transform.localScale = Vector3.zero;
            _popupsInstances[i].gameObject.SetActive(false);
        }
    }

    public void SpawnMaterialPopup(CraftingMaterial material)
    {
        CheckCurrentItems();
        for (int i = 0; i < _popupsInstances.Length; i++)
        {
            var item = _popupsInstances[i];
            var mat = _assignedMaterials[i];
            if(mat == material)
            {
                item.AddToMaterialCount(1);
                SetActivePopup(item, i);
                break;
            }else if(mat == null && !_lockChanges)
            {
                _assignedMaterials[i] = material;
                item.Initialize(1, material.Sprite);
                SetActivePopup(item, i);
                break;
            }
        }
    }

    void SetActivePopup(ResourceUIPopup popup, int matIndex)
    {;
        popup.gameObject.SetActive(true);
        popup.GetAnimator().Clear();
        popup.GetAnimator().Scale(popup.GetRect(), _startingScale, _animDuration, onComplete: () =>
        {
            _animator.Scale(popup.GetRect(), _startingScale * 1.05f, _animDuration / 2.5f, onComplete: () =>
            {
                _animator.Scale(popup.GetRect(), Vector3.zero, _scaleOutDuration, onComplete: () =>
                {
                    _assignedMaterials[matIndex] = null;
                });
            });
        });
    }

    void CheckCurrentItems()
    {
        bool anyNullElement = false;
        foreach(var mat in _assignedMaterials)
        {
            if(mat == null)
            {
                anyNullElement = true;
                break;
            }
        }
        _lockChanges = !anyNullElement;
    }

    void PlayPopup()
    {
        //_animator.MoveTo()
    }


    private void OnDestroy() {
    }
}
