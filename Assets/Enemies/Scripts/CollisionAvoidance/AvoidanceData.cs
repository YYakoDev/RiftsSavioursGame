using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvoidanceData : MonoBehaviour, ITargetPositionProvider
{
    //Movement Behaviour
    private Transform _target = null;
    private List<Collider2D> _obstacles = new List<Collider2D>();

    //properties
    public List<Collider2D> Obstacles => _obstacles;
    public Transform TargetTransform { get => _target; set => _target = value; }

    // Start is called before the first frame update
    void Start()
    {
        if(_target == null)
        {
            _target = GameObject.FindGameObjectWithTag("Player").transform;
            Debug.Log(gameObject.name + " has no target, setting player as target");
        }
    }
}
