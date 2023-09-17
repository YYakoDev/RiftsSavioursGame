using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OffScreenDeactivator : MonoBehaviour
{
    GameObject _objectToDeactivate;
    Collider2D[] _collidersToDeactivate;
    SpriteRenderer _parentRenderer;
    [SerializeField]float _timeUntilDeactivation = 10f;
    float _deactivationCountdown;

    bool isActive = true;
    private void Awake() 
    {
    }

    // Start is called before the first frame update
    void Start()
    {
        if(_objectToDeactivate == null) _objectToDeactivate = transform.parent.gameObject;    
        if(_collidersToDeactivate == null) _collidersToDeactivate = _objectToDeactivate.GetComponentsInChildren<Collider2D>();
        if(_parentRenderer == null) _parentRenderer = _objectToDeactivate.GetComponent<SpriteRenderer>();


        _timeUntilDeactivation += Random.Range(-0.5f, 0.5f);
        ResetCountdown();

    }

    // Update is called once per frame
    void LateUpdate()
    {
        if(!_parentRenderer.isVisible)
        {
            //Not visible
            if(!isActive)
            {
                SetTimeTrigger();
                return;
            }
            isActive = false;
            SetCollidersState(isActive);
        }
        else
        {
            //Visible
            if(isActive)
            {
                ResetCountdown();
                return;
            }
            isActive = true;
            SetCollidersState(isActive);
        }
    }

    void SetCollidersState(bool state)
    {
        for(int i = 0; i < _collidersToDeactivate.Length;i++)
        {
            if(_collidersToDeactivate[i] == null)continue;
            _collidersToDeactivate[i].enabled = state;
        }
    }
    void SetObjectState(bool state)
    {
        _objectToDeactivate.SetActive(state);
    }

    void SetTimeTrigger()
    {
        if(_deactivationCountdown <= 0)
        {
            SetObjectState(false);
            ResetCountdown();
        }
        else
        {
            _deactivationCountdown -= 1 * Time.deltaTime;
        }
    }

    void ResetCountdown()
    {
        _deactivationCountdown = _timeUntilDeactivation;
    }
}
