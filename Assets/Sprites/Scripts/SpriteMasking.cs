using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteMasking : MonoBehaviour
{
    [Header("References")]
    [SerializeField]SpriteRenderer _playerSpriteRenderer;
    [SerializeField]Transform _playerTransform;
    [SerializeField]SpriteMask _spriteMask;
    [SerializeField]CircleCollider2D _collider;
    [SerializeField]float _yOffset;
    //[SerializeField]LayerMask _exceptionLayer;

    private List<SpriteRenderer> _otherRenderers = new List<SpriteRenderer>();
    private bool _checking = false;
    private bool _canDisableMask = true;
    private int _playerSortingLayerID;

    // Start is called before the first frame update
    void Awake()
    {
        if(_collider == null) _collider = this.GetComponent<CircleCollider2D>();
        if(_playerSpriteRenderer == null) _playerSpriteRenderer = this.GetComponentInParent<SpriteRenderer>();
        if(_spriteMask == null) _spriteMask = this.GetComponent<SpriteMask>();
        if(_playerTransform == null) _playerTransform = _playerSpriteRenderer.transform;
        _collider.isTrigger = true;

        _playerSpriteRenderer.maskInteraction = SpriteMaskInteraction.None;
        _playerSortingLayerID = _playerSpriteRenderer.sortingLayerID;
    }

    void Start()
    {
       
    }

    // Update is called once per frame
    void LateUpdate()
    {
        CheckMask();
    }

    void CheckMask()
    {
        if(_checking)
        {   
            float playerYPos = _playerTransform.position.y + _yOffset;
            for(int i = 0; i < _otherRenderers.Count; i++)
            {
                if(_otherRenderers[i] == null)
                {
                    continue;
                }
                if(_playerSortingLayerID != _otherRenderers[i].sortingLayerID)
                {
                    DisableDetectedMask(_otherRenderers[i]);
                    continue;
                }
                if(_playerSpriteRenderer.sortingOrder > _otherRenderers[i].sortingOrder){
                    DisableDetectedMask(_otherRenderers[i]);
                    continue;
                }
                if(playerYPos < _otherRenderers[i].transform.position.y){
                    DisableDetectedMask(_otherRenderers[i]);
                    continue;
                }
                
                _spriteMask.enabled = true;
                _playerSpriteRenderer.maskInteraction = SpriteMaskInteraction.VisibleInsideMask;
                _otherRenderers[i].maskInteraction = SpriteMaskInteraction.VisibleOutsideMask;
                _canDisableMask = false;
            }
        }else
        {
            DisableMask();
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        //if(other.gameObject.layer == _exceptionLayer)return;

        if(other.TryGetComponent<SpriteRenderer>(out var collRenderer))
        {
            _otherRenderers.Add(collRenderer);
            _checking = true;
        }
    }
    private void OnTriggerExit2D(Collider2D other) {
        //if(other.gameObject.layer == _exceptionLayer)return;
        
        if(other.TryGetComponent<SpriteRenderer>(out var collRenderer))
        {
            collRenderer.maskInteraction = SpriteMaskInteraction.None;
            _otherRenderers.Remove(collRenderer);
            _checking = true;
            if(_otherRenderers.Count <= 0)
            {
                _checking = false;
                _canDisableMask = true;
                DisableMask();
            }
        }
    }

    void DisableMask()
    {
        if(!_canDisableMask) return;
        _spriteMask.enabled = false;
        _playerSpriteRenderer.maskInteraction = SpriteMaskInteraction.None;
    }

    void DisableDetectedMask(SpriteRenderer renderer)
    {
        renderer.maskInteraction = SpriteMaskInteraction.None;
        DisableMask();
    }
}

