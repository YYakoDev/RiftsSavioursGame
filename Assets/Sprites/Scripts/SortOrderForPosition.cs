using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SortOrderForPosition : MonoBehaviour
{
    SpriteRenderer _spriteRenderer;
    SortingOrderController _sortOrderController;
    [SerializeField]float _offsetPosition = 0;

    void Awake()
    {
        if(_spriteRenderer == null) _spriteRenderer = this.GetComponent<SpriteRenderer>();
        if(_sortOrderController == null) _sortOrderController = new SortingOrderController(transform, _spriteRenderer, _offsetPosition);
    }

    // Start is called before the first frame update
    void Start()
    {
        _sortOrderController.SortOrder();
    }
}
