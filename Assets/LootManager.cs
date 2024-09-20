using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootManager : MonoBehaviour
{
    [SerializeField] Chest _chest;
    [SerializeField] WaveSystem _waveSys;
    SOEnemyWave _previousWave;

    private void Awake() {
        _waveSys.OnWaveChange += UpdateWave;
    }

    private void Start() {
        _chest.gameObject.SetActive(false);
        _chest.MakeIntangible();
    }

    void UpdateWave(SOEnemyWave wave)
    {
        if(_previousWave == null)
        {
            _previousWave = wave;
            return;
        }
        RewardLoot();
        _previousWave = wave;
    }

    public void AddLoot(Drop drop) => _chest.AddDropToChest(drop);

    void RewardLoot()
    {
        _chest.gameObject.SetActive(true);
        //make chest tangible so it can be destroyed?
        _chest.MakeTangible();
    }

    private void OnDestroy() {
        _waveSys.OnWaveChange -= UpdateWave;
    }
}
