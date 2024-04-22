using System;
using UnityEngine;

public class MainHubTransitionTrigger : MonoBehaviour
{
    public event Action<Transform> _onPlayerEnter;
    private void OnTriggerEnter2D(Collider2D other) {
        if(other.tag == "Player")
            _onPlayerEnter?.Invoke(other.transform);
    }
}
