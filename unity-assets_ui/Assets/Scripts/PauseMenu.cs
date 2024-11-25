using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject _pauseCanvas;
    [SerializeField] private TimerTrigger _timer;

    private bool _isPaused = false; // Track if the game is currently paused

    private void Update() 
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Pause();
        }    
    }

    public void Pause()
    {
        _isPaused = !_isPaused; // Toggle pause state
        _pauseCanvas.SetActive(_isPaused);

        // Set time scale (0 = frozen, 1 = normal)
        Time.timeScale = _isPaused ? 0f : 1f;

        if (_timer != null)
        {
            _timer.enabled = !_isPaused; 
        }
    }
}
