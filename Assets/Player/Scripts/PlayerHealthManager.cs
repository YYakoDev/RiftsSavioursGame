using UnityEngine;

[RequireComponent(typeof(WhiteBlinkEffect))]
public class PlayerHealthManager : MonoBehaviour, IDamageable
{
    [SerializeField]PlayerManager _player;
    WhiteBlinkEffect _blinkFX;
    bool _invulnerability = false;
    Timer _invulnerabilityTimer;
    [Header("Audio Stuff")]
    [SerializeField]AudioSource _audio;
    [SerializeField]AudioClip[] _onHitSFXs;
    private AudioClip _onHitSfx => _onHitSFXs[Random.Range(0, _onHitSFXs.Length)];
    public WhiteBlinkEffect BlinkFX => _blinkFX;
    private void Awake()
    {   
        gameObject.CheckComponent<PlayerManager>(ref _player);
        gameObject.CheckComponent<WhiteBlinkEffect>(ref _blinkFX);
        SetHealth();
        _invulnerabilityTimer = new(0.1f);
        _invulnerabilityTimer.Stop();
        _invulnerabilityTimer.onStart += MakePlayerInvulnerable;
        _invulnerabilityTimer.onEnd += MakePlayerVulnerable;
    }


    private void Update() {
        _invulnerabilityTimer.UpdateTime();
    }

    void SetHealth()
    {
        _player.Stats.CurrentHealth = _player.Stats.MaxHealth;
    }

    public void SetInvulnerabilityTime(float time = 0.1f)
    {
        _invulnerabilityTimer.ChangeTime(time);
        _invulnerabilityTimer.Start();
    }

    void MakePlayerInvulnerable() => _invulnerability = true;
    void MakePlayerVulnerable() => _invulnerability = false;

    public void TakeDamage(int damage)
    {
        if(_player.Stats.CurrentHealth <= 0 || _invulnerability) return;
        int realDamage = damage - Mathf.FloorToInt((damage * _player.Stats.DamageResistance) / 100f);
        _player.Stats.CurrentHealth -= realDamage;
        _audio.PlayOneShot(_onHitSfx);
        GameFreezer.FreezeGame(0.03f);
        _blinkFX.Play();
        if(_player.Stats.CurrentHealth <= 0)
        {
            _blinkFX.Stop();
            Die();
        }
    }
    public void Die()
    {
        _player.Stats.CurrentHealth = 0;
        //Debug.Log("Player is dead lmao");
    }

    private void OnDestroy() {
        _invulnerabilityTimer.onStart -= MakePlayerInvulnerable;
        _invulnerabilityTimer.onEnd -= MakePlayerVulnerable;
    }

}
