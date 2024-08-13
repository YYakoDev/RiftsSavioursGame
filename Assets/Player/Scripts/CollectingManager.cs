using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectingManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField]PlayerManager _player;
    [SerializeField]SpriteRenderer _rangeRenderer;


    [Header("Stats")]
    float _range = 1f;
    int _damage = 1;
    [Range(0,5)]float _interactCooldown = 0.5f;
    [Range(1, 100)]int _maxResourceInteractions = 1; // a cap to the amount of resource you can interact with
    float _nextTimeToInteract; // the time for when you can interact with a resource
    [SerializeField]LayerMask _resourceLayer;
    Vector3 _resourcePosition;
    Collider2D[] _detectedResources = new Collider2D[100];

    public event Action<IResources> onResourceInteraction;
    Vector3 _dmgPopupOffset = Vector3.up / 1.5f;

    //properties
    public Vector3 ResourcePosition => _resourcePosition;
    public int PlayerFacingDirection => _player.MovementScript.FacingDirection;


    private void Awake() {
       if(_player == null) _player = this.GetComponentInParent<PlayerManager>();
       gameObject.CheckComponent<SpriteRenderer>(ref _rangeRenderer);
    }
    // Start is called before the first frame update
    void Start()
    {
        _player.Stats.onStatsChange += SetValues;
        SetValues();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(Time.time < _nextTimeToInteract)return;
        DetectResource();
    }

    void DetectResource()
    {
        _nextTimeToInteract = Time.time + _interactCooldown;
        int numberOfDetections = Physics2D.OverlapCircleNonAlloc(_player.Position, _range, _detectedResources , _resourceLayer);
        List<Resource> sortedResources = new();

        for(int i = 0; i < numberOfDetections; i++)
        {
            if(_detectedResources[i].TryGetComponent<Resource>(out var resourceDetected))
            {
                sortedResources.Add(resourceDetected);
            }
        }

        sortedResources.Sort();
        for(int y = 0; y < sortedResources.Count; y++)
        {
            if(y >= _maxResourceInteractions)break;
            if(sortedResources[y] == null)continue;
            GrabResource(sortedResources[y]);
        }
    }

    public void GrabResource(IResources resource)
    {
        //Debug.Log("Interacting With Resource");
        _resourcePosition = resource.ResourcePosition;
        onResourceInteraction?.Invoke(resource);
        resource.TakeDamage(_damage);
        PopupsManager.CreateDamagePopup(_resourcePosition + _dmgPopupOffset, _damage);
    }

    void SetValues()
    {
        //SOLO HACER CADA CAMBIO SI EL VALOR ES DIFERENTE, ESPECIALMENTE PARA EL METODO DE SET RANGE VISUALLY (creo que es costoso aplicarlo por cada cambio de stat)
        var newRange = _player.Stats.CollectingRange;
        if(_range != newRange)
        {
            _range = newRange;
            SetRangeVisually();
        }
        _damage = Mathf.RoundToInt(_player.Stats.CollectingDamage);
        _interactCooldown = _player.Stats.InteractCooldown;
        _maxResourceInteractions = Mathf.RoundToInt(_player.Stats.MaxResourceInteractions);
        //aca tendrias que chequear si el maxnumer of interactions cambio y es mayor que el resources detected length y agrandar el array de detecciones con Array.resize
        

    }

    void SetRangeVisually()
    {
        _rangeRenderer.size = Vector2.one * _range;
    }


    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(_player.Position, _range);
    }
    
    private void OnDestroy() {
        _player.Stats.onStatsChange -= SetValues;
    }
   
}
