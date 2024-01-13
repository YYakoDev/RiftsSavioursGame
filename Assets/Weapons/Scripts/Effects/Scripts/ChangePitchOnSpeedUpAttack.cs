using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct ChangePitchOnSpeedUpAttack
{
    float _pitchChange;
    PlayerAttackEffects _effects;
    public ChangePitchOnSpeedUpAttack(float pitchChange, PlayerAttackEffects effects)
    {
        _pitchChange = pitchChange;
        _effects = effects;
    }

    //public void Play(float duration) => _effects.ChangePitch(_pitchChange, duration);
    

}
