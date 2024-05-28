using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MultipleObjectPooler))]
[RequireComponent(typeof(AudioSource))]
public class ToolFX : MonoBehaviour
{
    [Header("References")]
    [SerializeField]CollectingManager _collectingManager;
    MultipleObjectPooler _objectsPool;
    AudioSource _audio;

    [Header("Pickaxe")]
    [SerializeField]private GameObject _pickaxePrefab;
    [SerializeField]private AudioClip[] _pickaxeSounds;
    private AudioClip _pickaxeSFX => _pickaxeSounds[Random.Range(0, _pickaxeSounds.Length)];

    [Header("Axe")]
    [SerializeField]private GameObject _axePrefab;
    [SerializeField]private AudioClip[] _axeSounds;
    private AudioClip _axeSFX => _axeSounds[Random.Range(0, _axeSounds.Length)];

    [Header("Hoe")]
    [SerializeField]private GameObject _hoePrefab;
    [SerializeField]private AudioClip[] _hoeSounds;
    private AudioClip _hoeSFX => _hoeSounds[Random.Range(0, _hoeSounds.Length)];

    [Header("Hand")]
    [SerializeField]private GameObject _handPrefab;

    private void Awake() 
    {
        if(_collectingManager == null) _collectingManager = this.GetComponentInParent<CollectingManager>();
        gameObject.CheckComponent<MultipleObjectPooler>(ref _objectsPool);
        gameObject.CheckComponent<AudioSource>(ref _audio); _audio.playOnAwake = false;
    }
    // Start is called before the first frame update
    void Start()
    {
        _collectingManager.onResourceInteraction += SelectToolFx;
    }

    void SelectToolFx(IResources resource)
    {
        var resourceType = resource.ResourceType;
        var offset = (Vector3.up * 0.7f);
        offset.x = -0.5f * _collectingManager.PlayerFacingDirection;
        Vector3 resourcePostion = _collectingManager.ResourcePosition + offset;
        //resourcePostion.x *= _collectingManager.PlayerFacingDirection;
        switch(resourceType)
        {
            case ResourcesTypes.Ore:
            {
                //Debug.Log("Spawning <b>pickaxe</b>!");
                InstantiateTool(_pickaxePrefab, resourcePostion);
                _audio.PlayOneShot(_pickaxeSFX);
            }break;
            case ResourcesTypes.Wood:
            {
                //Debug.Log("Spawning <b>Axe!!</b>");
                InstantiateTool(_axePrefab, resourcePostion);
                _audio.PlayWithVaryingPitch(_axeSFX);
            }break;
            case ResourcesTypes.Herb:
            {
                //Debug.Log("Spawning <b>Hand!</b>");
                InstantiateTool(_hoePrefab, resourcePostion);
                _audio.PlayWithVaryingPitch(_hoeSFX);
            }break;
            default:
            {
                //Debug.Log("Spawning <b>DEFAULT tool!</b>");
                InstantiateTool(_handPrefab, _collectingManager.ResourcePosition);
                _audio.PlayWithVaryingPitch(_hoeSFX);
            }break;
        }
    }

    void InstantiateTool(GameObject toolPrefab, Vector3 position)
    {
        GameObject tool = _objectsPool.GetPooledObjectMatch(toolPrefab); //the match succeeds when the prefab has the same name as the object(tool) you are giving as a reference
        tool.transform.parent = null;
        tool.transform.position = position;

        Vector3 facingScale = tool.transform.localScale;
        if(facingScale.x != _collectingManager.PlayerFacingDirection)
        {
            facingScale.x = _collectingManager.PlayerFacingDirection;
        } 
        tool.transform.localScale = facingScale;
        
    }

    private void OnDestroy()
    {
        _collectingManager.onResourceInteraction -= SelectToolFx;    
    }

    
}
