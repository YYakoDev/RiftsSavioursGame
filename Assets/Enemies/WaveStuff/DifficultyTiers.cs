using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DifficultyTiers : MonoBehaviour
{
    [SerializeField] DifficultyScaler _difficultyScaler;
    [SerializeField] DifficultyTier[] _tiers;
    int _currentIndex = -1, _endlessSeriesIndex = 1;
    DifficultyTier _currentTier, _endlessTier;
    public DifficultyTier CurrentTier => _currentTier;


    private void Awake() {
        _difficultyScaler.OnDifficultyIncrease += ChangeCurrentTier;
    }

    void ChangeCurrentTier()
    {
        if(_tiers == null || _tiers.Length == 0) return;
        _currentIndex++;
        if(_currentIndex >= _tiers.Length)
        {
            _endlessSeriesIndex++;
            if(_endlessTier == null) _endlessTier = new();
            _endlessTier.SetName(_tiers[_tiers.Length - 1].Name + " Level " + _endlessSeriesIndex);
            _endlessTier.SetSprite(_tiers[_tiers.Length - 1].Icon);
            _currentTier = _endlessTier;
            Debug.Log(_currentTier.Name);
            return;
        }
        _currentTier = _tiers[_currentIndex];
        Debug.Log(_currentTier.Name);
    }

    private void OnDestroy() {
        _difficultyScaler.OnDifficultyIncrease -= ChangeCurrentTier;
    }
}

[System.Serializable]
public class DifficultyTier
{
    [SerializeField] string _name;
    [SerializeField] Sprite _icon;

    public Sprite Icon => _icon;
    public string Name => _name;

    public void SetName(string name) => _name = name;
    public void SetSprite(Sprite sprite) => _icon = sprite;
}
