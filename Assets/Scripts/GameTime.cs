using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameTime : MonoBehaviour
{
    public static float RunTime = 0f;

    // Start is called before the first frame update
    void Start()
    {
        RunTime = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        RunTime += Time.deltaTime;
    }
}
