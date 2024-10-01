using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneMessenger : MonoBehaviour
{
    public event Action OnSceneStart, OnSceneEnd;

    private IEnumerator Start() {
        yield return null;
        OnSceneStart?.Invoke();
    }

    private void OnDestroy() {
        OnSceneEnd?.Invoke();
    }
}
