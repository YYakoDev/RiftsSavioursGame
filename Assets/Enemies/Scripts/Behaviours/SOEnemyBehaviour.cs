using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SOEnemyBehaviour : ScriptableObject
{
    protected const string MenuPath = "ScriptableObjects/Enemy/";
    protected EnemyBrain _brain;

    public virtual void Initialize(EnemyBrain brain) => _brain = brain;

    public virtual void Action(){}

}
