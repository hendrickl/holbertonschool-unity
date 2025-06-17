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
        gameObject.SetActive(true);

        _currentScene = SceneManager.GetActiveScene().name;

        _level01AnimationState = "Intro01";
        _level02AnimationState = "Intro02";
        _level03AnimationState = "Intro03";

        switch (_currentScene)
        {
            case "Level02":
                _currentLevelAnimationState = _level02AnimationState;
                break;

            case "Level03":
                _currentLevelAnimationState = _level03AnimationState;
                break;

            default:
                _currentLevelAnimationState = _level01AnimationState;
                break;
        }

        if (_playerController != null)
        {
            _playerController.enabled = false;
        }
        
        if (_animator != null)
        {
            _animator.Play(_currentLevelAnimationState, 0);

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
