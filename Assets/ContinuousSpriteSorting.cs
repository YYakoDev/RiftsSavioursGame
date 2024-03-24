using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContinuousSpriteSorting : MonoBehaviour
{
    SpriteRenderer _renderer;
    SortingOrderController _sorting;
    [SerializeField] float _sortingOffset;
    const float SortingInterval = 0.033f;
    float _triggerTime;
    private void Awake() {
        _renderer = GetComponent<SpriteRenderer>();
        if(_renderer == null) gameObject.SetActive(false);
        _sorting = new(transform, _renderer, _sortingOffset);
    }

    // Update is called once per frame
    void Update()
    {
        if(_triggerTime >= 0)
        {
            _triggerTime -= Time.deltaTime;
        }
        else
        {
            _triggerTime = SortingInterval;
            _sorting.SortOrder();
        }
    }
}
