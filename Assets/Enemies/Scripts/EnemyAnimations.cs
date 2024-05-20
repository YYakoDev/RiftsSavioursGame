using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimations : MonoBehaviour
{
    [SerializeField]private Animator _animator;
    private static readonly int SpawnAnim = Animator.StringToHash("Spawn");
    private static readonly int Iddle = Animator.StringToHash("Iddle");
    private static readonly int Move = Animator.StringToHash("Move");
    private static readonly int Hit = Animator.StringToHash("Hit");
    private static readonly int Attack = Animator.StringToHash("Attack");
    private static readonly int Death = Animator.StringToHash("Death");

    int _currentAnimation;
    float _lockedTill = 0f;
    float _initialAnimatorSpeed = 1;
    Timer _speedChangeTimer;
    //Properties
    //public Animator Animator => _animator;
    public float InitialAnimatorSpeed => _initialAnimatorSpeed;
    public Animator Animator => _animator;
    private void Awake()
    {
        gameObject.CheckComponent<Animator>(ref _animator);
        _initialAnimatorSpeed = 1f + Random.Range(-0.1f, 0.15f);
        _speedChangeTimer = new(0.1f);
        _speedChangeTimer.onEnd += ResetSpeed;
        _speedChangeTimer.Stop();
        ChangeAnimatorSpeed(_initialAnimatorSpeed);
    }

    private void OnEnable() {
        //PlayIddle();
        PlaySpawnAnim();
    }
    private void Update() {
        _speedChangeTimer.UpdateTime();
    }
    public void PlaySpawnAnim() => PlayStated(SpawnAnim, 0.275f);
    public void PlayMove() => PlayStated(Move);
    
    public void PlayIddle() => PlayStated(Iddle);
    
    public void PlayHit() => PlayStated(Hit);
    public void PlayAttack() => PlayStated(Attack);
    
    public void PlayDeath(float animDuration) => PlayStated(Death, animDuration);
    

    void PlayStated(int animHash, float lockDuration = 0f)
    {
        if(Time.time <= _lockedTill) return;
        if(animHash == _currentAnimation)return;
        
        _currentAnimation = animHash;
        LockState(lockDuration);
        _animator.Play(animHash);
//        Debug.Log($"Playing enemy's <b>{animHash.GetHashCode()}</b> animation");
        void LockState( float time)
        {
            _lockedTill = Time.time + time;
        }
    }
    
    public void ChangeAnimatorSpeed(float newSpeed)
    {
        _animator.speed = newSpeed;
    }
    public void ChangeAnimatorSpeed(float newSpeed, float duration)
    {
        _animator.speed = newSpeed;
        _speedChangeTimer.ChangeTime(duration);
        _speedChangeTimer.Start();
    }
    public void ResetSpeed()
    {
        _animator.speed = InitialAnimatorSpeed;
    }

    private void OnDisable() {
        _animator.StopPlayback();
        _currentAnimation = 0;
        _lockedTill = 0f;
    }
    private void OnDestroy() {
        _speedChangeTimer.onEnd -= ResetSpeed;
    }
}
