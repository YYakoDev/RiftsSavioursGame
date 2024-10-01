using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrototypeServant : MonoBehaviour
{
    [SerializeField] float _detectionRadius = 1f, _detectionRate = 1f, _speed;
    //[SerializeField] SOPlayerStats _playerStats;

    [SerializeField] LayerMask _resourceLayer;
    Transform _objective, _player;
    Vector3 _offset;
    float _atkCooldown = 0.6f, _nextAtkTime;
    // Start is called before the first frame update
    void Start()
    {
        _speed += Random.Range(-0.2f, 0.2f);
        _offset = new Vector3
        (
            Random.Range(-1f, 1f),
            Random.Range(-1f, 1f),
            0
        );
        _player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        MoveToObjective();
        if(_detectionRate >= 0)
        {
            _detectionRate -= Time.deltaTime;
            return;
        }
        var results = Physics2D.OverlapCircleAll(transform.position, _detectionRadius, _resourceLayer);
        if(results == null || results.Length == 0) return;
        _detectionRate = 1f;
        _objective = results[0].transform;
    }

    void MoveToPlayer()
    {
        transform.position = Vector3.MoveTowards(transform.position, _player.position - _offset, _speed * 2f * Time.fixedDeltaTime);
    }

    void MoveToObjective()
    {
        if(_objective == null)
        {
            MoveToPlayer();
            return;
        }
        transform.position = Vector3.MoveTowards(transform.position, _objective.position, _speed * Time.fixedDeltaTime);
        if(_nextAtkTime > Time.time) return;
        if(Vector3.Distance(transform.position, _objective.position) < 0.3f)
        {
            var resourceLogic = _objective.GetComponent<Resource>();
            if(resourceLogic != null)
            {
                _nextAtkTime = _atkCooldown + Time.time;
                resourceLogic.TakeDamage(1);
                if(resourceLogic.CurrentHealth <= 0)
                {
                    _objective = null;
                }
            }
        }
    }



    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, _detectionRadius);
    }
}
