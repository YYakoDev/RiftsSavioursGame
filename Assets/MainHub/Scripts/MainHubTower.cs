using UnityEngine;
using UnityEngine.SceneManagement;
public class MainHubTower : MonoBehaviour, IInteractable
{
    [SerializeField]Vector3 _buttonOffset;
    bool _alreadyInteracted;
    [SerializeField]Animator _towerAnimator;
    float _animLength = 1f;
    Timer _animationTimer;
    public Vector3 Offset => _buttonOffset;
    public bool AlreadyInteracted { get => _alreadyInteracted; set => _alreadyInteracted = value; }

    public AudioClip InteractSfx => null;

    private void Awake() {
        var clips = _towerAnimator.runtimeAnimatorController.animationClips;
        foreach(var clip in clips)
        {
            if(clip.name == "TowerAnimation")
            {
                _animLength = clip.averageDuration;
                break;
            }
        }
        _animationTimer = new(_animLength);
        _animationTimer.Stop();
        _animationTimer.onEnd += LoadGame;
    }

    private void Update() {
        _animationTimer.UpdateTime();
    }

    public void Interact()
    {
        _towerAnimator.Play("Animation");
        _animationTimer.Start();
    }

    void LoadGame()
    {
        SceneManager.LoadScene("Game");
    }

    private void OnDestroy() {
        _animationTimer.onEnd -= LoadGame;
    }
}
