using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldManager : MonoBehaviour
{
    //public static WorldManager instance;
    [SerializeField]private World[] _worlds = new World[4];
    [SerializeField]private World _currentWorld;
    private int _currentWorldIndex = 0; 

    //properties
    public World CurrentWorld => _currentWorld;

    private void Awake() 
    {
        DontDestroyOnLoad(gameObject);
        _currentWorld.Initialize(_worlds[_currentWorldIndex]);

    }
    // Start is called before the first frame update
    void Start()
    {
        //subscribe to the onRiftTimerEnd event of the timer and make the world advance and load a new scene etc
        
    }

    // Update is called once per frame
    void Update()
    {
        
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
    }


    private void OnValidate() 
    {
        //set the worlds array max length

    }
}
