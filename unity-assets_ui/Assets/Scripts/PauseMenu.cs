using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject _pauseCanvas;
    [SerializeField] private TimerTrigger _timer;
    [SerializeField] private Button _resumeButton;

    private bool _isPaused = false;

    private void Awake()
    {
        Time.timeScale = 1f; // Ensure time scale is reset at the start of the scene
    }

    private void Start()
    {
        _pauseCanvas.SetActive(false);
        _resumeButton.onClick.AddListener(Resume);
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
        _resumeButton.onClick.RemoveListener(Resume);
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
    }
}
