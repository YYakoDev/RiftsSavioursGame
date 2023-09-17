using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SortingOrderController
{
    Transform _currentPosition;
    SpriteRenderer _spriteRenderer;
    [Range(0,10)]float _offset;
    int _extraOrder;
    public SortingOrderController(Transform currentPosition, SpriteRenderer spriteRenderer, float offset = 0, int extraOrder = 0)
    {
        _spriteRenderer = spriteRenderer;
        _currentPosition = currentPosition;
        _offset = offset;
        _extraOrder = extraOrder;
    }
    
    public void SortOrder()
    {
        int sortOrder = _extraOrder -(int)((_currentPosition.position.y - _offset) *30);
        _spriteRenderer.sortingOrder = sortOrder;
    }

}
