using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    EnemyBrain _brain;
    EnemyAttackLogic _atkLogic;
    SOEnemyAttackBehaviour _behaviour;

    public void Init(EnemyBrain brain, EnemyAttackLogic baseLogic, SOEnemyAttackBehaviour attackBehaviour)
    {
        _brain = brain;
        _atkLogic = baseLogic;
        _behaviour = attackBehaviour;
    }

    private void Update() {
        _behaviour?.UpdateLogic();
    }

    public void DoAttack()
    {
        _behaviour?.Action();
    }



}
