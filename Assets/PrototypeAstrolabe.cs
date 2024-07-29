using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class PrototypeAstrolabe : MonoBehaviour, IInteractable
{
    [SerializeField] PrototypeChunkSpawner _chunkManager;
    [SerializeField] EnemyWaveSpawner _waveSpawner;
    [SerializeField] StoreMenu _store;
    [SerializeField] FadeImage _fadeImage;
    [SerializeField] Transform _player;
    Camera _camera;
    [SerializeField] float _fightDuration = 25f;
    Timer _fightTimer;
    [SerializeField] Vector3 _offset;
    bool _alreadyInteracted;
    int _lastRandomNumber = 4;

    public bool AlreadyInteracted { get => _alreadyInteracted; set => _alreadyInteracted = value; }
    public Vector3 Offset => _offset;
    public AudioClip InteractSfx => null;

    private void Awake() {
        _fightTimer = new(_fightDuration);
        _fightTimer.Stop();
        _fightTimer.onEnd += StopFight;
    }

    private void Start() {
        _camera = Camera.main;
    }

    private void Update() {
        _fightTimer.UpdateTime();
    }

    void StartFight()
    {
        _fightTimer.Start();
        _waveSpawner.gameObject.SetActive(true);
    }

    void StopFight()
    {
        _waveSpawner.gameObject.SetActive(false);
        _alreadyInteracted = false;
    }

    public void Interact()
    {
        //go to random room and do stuff there.
        int rndm = HelperMethods.RandomRangeExcept(0, 4, _lastRandomNumber);
        if(_lastRandomNumber >= 2) rndm = Random.Range(0, 4);
        if(rndm >= 2)
        {
            var pos = _chunkManager.GetFightPosition();
            TeleportPlayer(pos, () => {StartFight();});
            
        }else if(rndm == 1)
        {
            var pos = _chunkManager.GetStorePosition();
            _alreadyInteracted = false;
            TeleportPlayer(pos, () => {_store.OpenMenu();});
            
        }else
        {
            var pos = _chunkManager.GetCraftingPosition();
            _alreadyInteracted = false;
            TeleportPlayer(pos, null);
        }
        _lastRandomNumber = rndm;
    }

    void TeleportPlayer(Vector3 position, Action onComplete)
    {        
        transform.position = position;
        _player.position = position;
        position.z = -10;
        _camera.transform.position = position;
        CameraShake.Shake(2f, 1f);
        _fadeImage.FadeIn(() =>
        {
            _fadeImage.FadeOut();
            onComplete?.Invoke();
        });
    }

    private void OnDestroy() {
        _fightTimer.onEnd -= StopFight;
    }
}
