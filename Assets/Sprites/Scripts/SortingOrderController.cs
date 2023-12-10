using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SortingOrderController
{
    Transform _currentPosition;
    SpriteRenderer _spriteRenderer;
    float _offset;
    int _extraOrder;
    public SortingOrderController(Transform currentTransform, SpriteRenderer spriteRenderer, float offset = 0, int extraOrder = 0)
    {
        _spriteRenderer = spriteRenderer;
        _currentPosition = currentTransform;
        _offset = offset;
        _extraOrder = extraOrder;
    }
    
    public void SortOrder()
    {
        int sortOrder = _extraOrder -(int)((_currentPosition.position.y - _offset) *35);
        _spriteRenderer.sortingOrder = sortOrder;
    }

    public void ChangeOffset(float newOffset)
    {
        _offset = newOffset;
        SortOrder();
    }
}
