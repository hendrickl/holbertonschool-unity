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

     // Animation parameter names for different levels
    private string _intro01AnimationParameter;
    private string _intro02AnimationParameter;
    private string _intro03AnimationParameter;

    // Animation state names for different levels
    private string _level01AnimationState;
    private string _level02AnimationState;
    private string _level03AnimationState;

    private string _currentLevelAnimationParameter; 
    private string _currentLevelAnimationState; 
    private string _currentScene;

    private void Start() 
    {
        _currentScene = SceneManager.GetActiveScene().name;
        Debug.Log($"CutsceneController: Current scene is {_currentScene}");

        _intro01AnimationParameter = "PlayIntro";
        _intro02AnimationParameter = "PlayIntro02";
        _intro03AnimationParameter = "PlayIntro03";

        _level01AnimationState = "Intro01";
        _level02AnimationState = "Intro02";
        _level03AnimationState = "Intro03";

        switch (_currentScene)
        {
            case "Level02":
                _currentLevelAnimationParameter = _intro02AnimationParameter;
                _currentLevelAnimationState = _level02AnimationState;
                Debug.Log($"CutsceneController: In {_currentScene} the animation paramater is {_currentLevelAnimationParameter}");
                Debug.Log($"CutsceneController: In {_currentScene} the animation state is {_currentLevelAnimationState}");
                break;

            case "Level03":
                _currentLevelAnimationParameter = _intro03AnimationParameter;
                _currentLevelAnimationState = _level03AnimationState;
                Debug.Log($"CutsceneController: In {_currentScene} the animation paramater is {_currentLevelAnimationParameter}");
                Debug.Log($"CutsceneController: In {_currentScene} the animation state is {_currentLevelAnimationState}");
                break;

            default:
                _currentLevelAnimationParameter = _intro01AnimationParameter;
                _currentLevelAnimationState = _level01AnimationState;
                Debug.Log($"CutsceneController: In {_currentScene} the animation paramater is {_currentLevelAnimationParameter}");
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
            Debug.Log($"CutsceneCobtroller: Triggered animation parameter {_currentLevelAnimationParameter}");

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
