using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWavePooler : MonoBehaviour
{
    Dictionary<GameObject, EnemySignature> _spawnedEnemies = new Dictionary<GameObject, EnemySignature>();
    //List<GameObject> pooledObjects;
    GameObject objectToPool;
    EnemySignature objectToPoolSignature;
    [SerializeField]int amountToPool;
    [SerializeField]bool resizeable = true;

    World _currentWorld;
    EnemySignature[] _currentSignatures => _currentWorld.CurrentWave.EnemiesSignatures;

    void Start()
    {
        //pooledObjects = new List<GameObject>();
        for(int i = 0;i<amountToPool;i++)
        {
            CreateNewObject();
        }
    }

    public GameObject GetPooledObject()
    {
        //debug
        /*for(int i = 0; i < _currentSignatures.Length; i++)
        {
            Debug.Log("Signature: " + _currentSignatures[i]);
        }*/

        foreach(KeyValuePair<GameObject,EnemySignature> enemy in _spawnedEnemies)
        {
            if(enemy.Key.activeInHierarchy) continue;

            //if you reach this loop it means the enemy is not active on the scene
            for(int i = 0; i < _currentSignatures.Length; i++)
            {
                if(enemy.Value != _currentSignatures[i]) continue;

                //enemy.Key.SetActive(true);
                return enemy.Key;
            }
        }

        if(resizeable)return CreateNewObject();
        else return null;

    }

    private GameObject CreateNewObject()
    {
        SelectEnemyToPool();

        if(objectToPoolSignature == EnemySignature.None)return null;

        GameObject newObject = Instantiate(objectToPool);
        newObject.transform.SetParent(this.transform);
        newObject.SetActive(false);

        _spawnedEnemies.Add(newObject, objectToPoolSignature);

        return newObject;

    }

    void SelectEnemyToPool()
    {
        int index = RandomIndexFromList(_currentSignatures.Length);
        for(int i = 0; i < _currentWorld.EnemyPrefabs.Length; i++)
        {
            if(_currentWorld.EnemyPrefabs[i].SignatureMatch(_currentSignatures[index]))
            {
                objectToPoolSignature = _currentWorld.EnemyPrefabs[i].Signature;
                objectToPool = _currentWorld.EnemyPrefabs[i].gameObject;
                return;
            }
        }
        Debug.Log("<b> No enemies found with the signature: " + _currentSignatures[index] + "</b>");
        objectToPoolSignature = EnemySignature.None;

        int RandomIndexFromList(int listLength)
        {
            return Random.Range(0, listLength);
        }
    }

    public void SetCurrentWorld(World world)
    {
        _currentWorld = world;
    }

}