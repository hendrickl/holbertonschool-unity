using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// This script manages navigation between scenes
public class MainMenu : MonoBehaviour
{
    [SerializeField] private Button _level01Button;
    [SerializeField] private Button _level02Button;
    [SerializeField] private Button _level03Button;
    [SerializeField] private Button _optionsButton;
    [SerializeField] private Button _exitButton;

    private void Start()
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.StopLevelBackgroundMusic();
        }

        if (_level01Button != null)
        {
            _level01Button.onClick.AddListener(() => LevelSelect(2));
        }

        if (_level02Button != null)
        {
            _level02Button.onClick.AddListener(() => LevelSelect(3));
        }

        if (_level03Button != null)
        {
            _level03Button.onClick.AddListener(() => LevelSelect(4));
        }

        if (_optionsButton != null)
        {
            _optionsButton.onClick.AddListener(() => LevelSelect(1));
        }

        if (_exitButton != null)
        {
            _exitButton.onClick.AddListener(() => Debug.Log($"Exited"));
            // _exitButton.onClick.AddListener(() => Application.Quit());
        }

    }

    public void LevelSelect(int level)
    {
        PlayerPrefs.SetInt("PreviousScene", SceneManager.GetActiveScene().buildIndex);
        SceneManager.LoadScene(level);
    }

    private void OnDestroy()
    {
        if (_level01Button != null)
        {
            _level01Button.onClick.RemoveAllListeners();
        }

        if (_level02Button != null)
        {
            _level02Button.onClick.RemoveAllListeners();
        }

        if (_level03Button != null)
        {
            _level03Button.onClick.RemoveAllListeners();
        }

        if (_optionsButton != null)
        {
            _optionsButton.onClick.RemoveAllListeners();
        }

        if (_exitButton != null)
        {
            _optionsButton.onClick.RemoveAllListeners();
        }
    }
}