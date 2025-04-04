using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

// This script manages cutscene behavior
public class CutsceneController : MonoBehaviour
{
    [SerializeField] private Camera _mainCamera;
    [SerializeField] private PlayerController _playerController;
    [SerializeField] private Canvas _timerCanvas;
    [SerializeField] private Animator _animator;
    [SerializeField] private float _animationTransitionTime;

    // Animation state names for different levels
    private string _level01AnimationState;
    private string _level02AnimationState;
    private string _level03AnimationState;

    private string _currentLevelAnimationState; 
    private string _currentScene;

    private void Start() 
    {
        _currentScene = SceneManager.GetActiveScene().name;
        Debug.Log($"CutsceneController: Current scene is {_currentScene}");

        _level01AnimationState = "Intro01";
        _level02AnimationState = "Intro02";
        _level03AnimationState = "Intro03";

        switch (_currentScene)
        {
            case "Level02":
                _currentLevelAnimationState = _level02AnimationState;
                Debug.Log($"CutsceneController: In {_currentScene} the animation state is {_currentLevelAnimationState}");
                break;

            case "Level03":
                _currentLevelAnimationState = _level03AnimationState;
                Debug.Log($"CutsceneController: In {_currentScene} the animation state is {_currentLevelAnimationState}");
                break;

            default:
                _currentLevelAnimationState = _level01AnimationState;
                Debug.Log($"CutsceneController: In {_currentScene} the animation state is {_currentLevelAnimationState}");
                break;
        }

        if (_playerController != null)
        {
            _playerController.enabled = false;
        }
        
        if (_animator != null)
        {
            _animator.Play(_currentLevelAnimationState);
            Debug.Log($"CutsceneController: Triggered animation state {_currentLevelAnimationState}");

            StartCoroutine(WaitForAnimationEnd());
        }
        else
        {
            Debug.Log($"CutsceneController: Animator is not assigned");
            EndCutscene();
        }
    }

    private IEnumerator WaitForAnimationEnd()
    {
        yield return new WaitForSeconds(_animationTransitionTime);

        // Wait until the animation completes
        while (_animator.GetCurrentAnimatorStateInfo(0).IsName(_currentLevelAnimationState) && _animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
        {
            yield return null;
        }

        EndCutscene();              
    }

    private void EndCutscene()
    {
        if (_mainCamera != null)
        {
            _mainCamera.gameObject.SetActive(true);
        }

        if (_playerController != null)
        {
            _playerController.enabled = true;
        }

        if (_timerCanvas != null)
        {
            _timerCanvas.gameObject.SetActive(true);
        }

        gameObject.SetActive(false);
    }
}
