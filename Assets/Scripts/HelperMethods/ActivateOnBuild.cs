using UnityEngine;

public class ActivateOnBuild : MonoBehaviour
{
    [SerializeField] private GameObject[] _objectsToActivate;

    void Awake()
    {
        if(Application.installMode == ApplicationInstallMode.Editor) return;
        if (_objectsToActivate == null || _objectsToActivate.Length == 0) return;
        foreach (var item in _objectsToActivate)
        {
            item.SetActive(true);
        }
    }
}
