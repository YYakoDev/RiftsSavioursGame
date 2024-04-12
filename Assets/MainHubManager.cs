using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainHubManager : MonoBehaviour
{
    [SerializeField] GameObject _playerObj;
    [SerializeField] MainHubCharacterSelector _characterSelector;
    // Start is called before the first frame update
    void Start()
    {
        _characterSelector.onMenuOpen += DeactivatePlayer;
        _characterSelector.onMenuClose += ReactivatePlayer;
        if(PlayerManager.SelectedChara == null)
        {
            _characterSelector.Open();    
            TimeScaleManager.ForceTimeScale(1f);
        }
    }

    void DeactivatePlayer()
    {
        _playerObj.SetActive(false);
    }

    void ReactivatePlayer()
    {
        _playerObj.SetActive(true);
    }

    private void OnDestroy() {
        _characterSelector.onMenuOpen -= DeactivatePlayer;
        _characterSelector.onMenuClose -= ReactivatePlayer;
    }
}
