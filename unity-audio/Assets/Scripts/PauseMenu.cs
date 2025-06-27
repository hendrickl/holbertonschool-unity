using UnityEngine;
using UnityEngine.UI;

// This script manages the functionality of the pause panel
public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject _pauseCanvas;
    [SerializeField] private GameObject _mainMenuCanvas;

    [SerializeField] private TimerTrigger _timer;

    [SerializeField] private Button _resumeButton;
    [SerializeField] private Button _restartButton;
    [SerializeField] private Button _mainMenuButton;
    [SerializeField] private Button _optionsButton;

    private bool _isPaused = false;

    private void Awake()
    {
        Time.timeScale = 1f; // Ensure time scale is reset at the start of the scene
    }

    private void Start()
    {
        if (_resumeButton != null)
        {
            _resumeButton.onClick.AddListener(Resume);
        }

        if (_restartButton != null)
        {
            _restartButton.onClick.AddListener(Restart);
        }

        if (_mainMenuButton != null)
        {
            _mainMenuButton.onClick.AddListener(MainMenu);
        }

        if (_optionsButton != null)
        {
            _optionsButton.onClick.AddListener(Options);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (_isPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    private void OnDestroy()
    {
        if (_resumeButton != null)
        {
            _resumeButton.onClick.RemoveListener(Resume);
        }

        if (_restartButton != null)
        {
            _restartButton.onClick.RemoveListener(Restart);
        }

        if (_mainMenuButton != null)
        {
            _mainMenuButton.onClick.RemoveListener(MainMenu);
        }

        if (_optionsButton != null)
        {
            _optionsButton.onClick.RemoveListener(Options);
        }
    }

    public void Pause()
    {
        _isPaused = true;
        _pauseCanvas.SetActive(true);
        Time.timeScale = 0f;

        if (_timer != null)
        {
            _timer.enabled = false;
        }

        if (AudioManager.Instance != null)
        {
            // AudioManager.Instance.PauseLevelbackgroundMusic();
            AudioManager.Instance.ApplyPausedSnapshot();
            Debug.Log("PauseMenu: Pause");
        }
    }

    public void Resume()
    {
        _isPaused = false;
        _pauseCanvas.SetActive(false);
        Time.timeScale = 1f;

        if (_timer != null)
        {
            _timer.enabled = true;
        }

        if (AudioManager.Instance != null)
        {
            // AudioManager.Instance.ResumeLevelbackgroundMusic();
            AudioManager.Instance.ApplyDefaultSnapshot();
            Debug.Log("PauseMenu: Resume");
        }
    }

    public void Restart()
    {
        ScenesManager.LoadScene(ScenesManager.GetCurrentSceneIndex());
    }

    public void MainMenu()
    {
        _pauseCanvas.SetActive(false);
        _mainMenuCanvas.SetActive(true);
    }

    public void Options()
    {
        ScenesManager.LevelSelect(1);
    }
}
