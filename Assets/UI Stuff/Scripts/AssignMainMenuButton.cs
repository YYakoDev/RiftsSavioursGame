using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AssignMainMenuButton : MonoBehaviour
{
    public void PlayButton()
    {
        Debug.Log("You should be using a loading screen for this, use the prefab asyncloader");
        SceneManager.LoadScene(1);
    }
    public void QuitButton()
    {
        Application.Quit();
    }
}
