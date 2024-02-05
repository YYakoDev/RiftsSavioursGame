using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class AsyncLoader : MonoBehaviour
{
    [SerializeField] GameObject _loadingScreenCanvas;
    GameObject _canvasInstance;
    Coroutine _loadingRoutine;
    public void LoadGame()
    {
        LoadLevel(1);
    }

    public void LoadLevel(int sceneIndex)
    {
        if(_loadingRoutine != null) return;
        if(_canvasInstance == null) _canvasInstance = Instantiate(_loadingScreenCanvas);
        _canvasInstance.SetActive(true);
        _loadingRoutine = StartCoroutine(LoadLevelAsync(sceneIndex));
    }

    IEnumerator LoadLevelAsync(int sceneIndex)
    {
        AsyncOperation loadOperation = SceneManager.LoadSceneAsync(sceneIndex);
        while (!loadOperation.isDone)
        {
            yield return null;
        }
        _loadingRoutine = null;
    }
}
