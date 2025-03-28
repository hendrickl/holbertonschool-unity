using UnityEngine;
using System.Collections;

// This script manages cutscene behavior
public class CutsceneController : MonoBehaviour
{
    [SerializeField] private Camera _mainCamera;
    [SerializeField] private PlayerController _playerController;
    [SerializeField] private Canvas _timerCanvas;
    [SerializeField] private Animator _animator;

    private string _intro01AnimationParameter;

    private void Start() 
    {
        _intro01AnimationParameter = "PlayIntro";
       
       _playerController.enabled = false;
        
        if (_animator != null)
        {
            _animator.SetTrigger(_intro01AnimationParameter);
            StartCoroutine(WaitForAnimationEnd());
        }
        else
        {
            Debug.Log($"CutsceneController: Animator component not assigned");
            EndCutscene();
        }
    }

    private IEnumerator WaitForAnimationEnd()
    {
        while (_animator.GetCurrentAnimatorStateInfo(0).IsName("Intro01") && _animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
        {
            yield return null;
        }

        EndCutscene();
    }

    private void EndCutscene()
    {
        if (_mainCamera != null)
            _mainCamera.gameObject.SetActive(true);

        if (_playerController != null)
            _playerController.enabled = true;

        if (_timerCanvas != null)
            _timerCanvas.gameObject.SetActive(true);

        gameObject.SetActive(false);
    }
}
