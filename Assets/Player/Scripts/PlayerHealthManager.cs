using UnityEngine;

[RequireComponent(typeof(WhiteBlinkEffect))]
public class PlayerHealthManager : MonoBehaviour, IDamageable
{
    [SerializeField]PlayerManager _player;
    [SerializeField]AudioSource _audio;
    WhiteBlinkEffect _blinkFX;
    
    [SerializeField]AudioClip[] _onHitSFXs;

    private AudioClip _onHitSfx => _onHitSFXs[Random.Range(0, _onHitSFXs.Length)];
    private void Awake()
    {   
        gameObject.CheckComponent<PlayerManager>(ref _player);
        gameObject.CheckComponent<WhiteBlinkEffect>(ref _blinkFX);
        SetHealth();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void SetHealth()
    {
        _player.Stats.CurrentHealth = _player.Stats.MaxHealth;
    }

    public void TakeDamage(int damage)
    {
        if(_player.Stats.CurrentHealth <= 0)
        {
            return;
        }
        _player.Stats.CurrentHealth -= damage;
        _audio.PlayOneShot(_onHitSfx);
        _blinkFX.Play();
        if(_player.Stats.CurrentHealth <= 0)
        {
            _blinkFX.Stop();
            Die();
        }
    }
    public void Die()
    {
        //Debug.Log("Player is dead lmao");
    }

}
