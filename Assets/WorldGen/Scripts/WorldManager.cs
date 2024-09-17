using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldManager : MonoBehaviour
{
    //public static WorldManager instance;
    [SerializeField]private World[] _worlds = new World[4];
    [SerializeField]private World _currentWorld;
    private static int _currentWorldIndex = 0; 

    [SerializeField] ChunkGenerator _chunkGenerator;
    [SerializeField] ResourcePool _resourcePool;
 
    //properties
    //public World CurrentWorld => _currentWorld;
    public static event Action<World> onWorldChange;

    private void Awake() 
    {
        _currentWorld.Initialize(_worlds[_currentWorldIndex]);
    }
    void Start()
    {
        //subscribe to the onRiftTimerEnd event of the timer and make the world advance and load a new scene etc
        _resourcePool.gameObject.SetActive(true);
        _chunkGenerator.gameObject.SetActive(true);
        //_chunkGenerator.StartCreation();
    }


    private void AdvanceWorld()
    {
        _currentWorldIndex++;
        if(_worlds.Length >= _currentWorldIndex)
        {
            Debug.Log("<b><color = red>No more worlds to advance to </color = red> </b>");
            //this indicates the end of the game or i messed up the list holding the worlds
            return;
        }
        _currentWorld = _worlds[_currentWorldIndex];
        onWorldChange?.Invoke(_currentWorld);
    }


    private void OnValidate() 
    {
        //set the worlds array max length
        if(_worlds.Length > 4)
        {
            Array.Resize<World>(ref _worlds, 4);
        }
    }
}
