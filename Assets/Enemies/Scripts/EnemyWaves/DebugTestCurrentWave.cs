using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DebugTestCurrentWave : MonoBehaviour
{
    [SerializeField]bool _debug = true;
    [SerializeField]EnemyWaveSpawner _spawner;
    [SerializeField]TextMeshProUGUI _debugText;
    SOEnemyWave _currentWave;

    #if UNITY_EDITOR
    // Start is called before the first frame update
    void Start()
    {
        if(!_debug) this.gameObject.SetActive(false);
        SetDebugText();
    }

    // Update is called once per frame
    void Update()
    {
        SetDebugText();
    }

    void SetDebugText()
    {
        if(!_debug)return;
        /*if(_currentWave == _spawner._currentWave)return;
        _currentWave = _spawner._currentWave;
        _debugText.text = "Current Wave: " + _currentWave.name + "\n";*/
    }
    #endif
    
    #if !UNITY_EDITOR
        void Start()
        {
            this.gameObject.SetActive(false);
        }
    #endif
}
