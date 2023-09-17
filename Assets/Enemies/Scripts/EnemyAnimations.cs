using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class EnemyAnimations : MonoBehaviour
{
    [SerializeField]private Animator _animator;
    private static readonly int Iddle = Animator.StringToHash("Iddle");
    private static readonly int Move = Animator.StringToHash("Move");
    private static readonly int Attack = Animator.StringToHash("Attack");
    private static readonly int Death = Animator.StringToHash("Death");

    int _currentAnimation;
    float _lockedTill = 0f;

    //Properties
    public Animator Animator => _animator;

    private void Awake()
    {
        gameObject.CheckComponent<Animator>(ref _animator);
    }

    private void OnEnable() {
        PlayIddle();
    }
    public void PlayMove()
    {
        PlayStated(Move);
    }
    public void PlayIddle()
    {
        PlayStated(Iddle);
    }
    public void PlayAttack()
    {
        PlayStated(Attack);
    }
    public void PlayDeath()
    {
        PlayStated(Death);
    }

    void PlayStated(int animHash)
    {
        if(Time.time <= _lockedTill) return;
        if(animHash == _currentAnimation)return;
        
        _currentAnimation = animHash;
        if(animHash == Attack)
        {
            LockState(0.3f);
        }
        else if(animHash == Death)
        {
            LockState(1f);
        }
        _animator.Play(animHash);
//        Debug.Log($"Playing enemy's <b>{animHash.GetHashCode()}</b> animation");
        void LockState( float time)
        {
            _lockedTill = Time.time + time;
        }
    }
    
    private void OnDisable() {
        _animator.StopPlayback();
        _currentAnimation = 0;
        _lockedTill = 0f;
    }
}
