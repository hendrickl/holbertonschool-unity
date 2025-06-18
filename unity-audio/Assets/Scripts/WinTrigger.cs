using UnityEngine;
using TMPro;

// This script manages win condition in the game
public class WinTrigger : MonoBehaviour
{
    [SerializeField] private Timer _timer;
    [SerializeField] private Canvas _winCanvas;
    [SerializeField] private TMP_Text _timerText;
    [SerializeField] private float _increasedFontSize = 60f;
    [SerializeField] private Color _winColor = Color.green;

    private void Start()
    {
        _winCanvas.gameObject.SetActive(false);    
    }

    private void OnTriggerEnter(Collider p_other)
    {
        if (p_other.CompareTag("Player"))
        {
            if (AudioManager.Instance != null)
            {
                AudioManager.Instance.StopLevelBackgroundMusic();
            }

            DisplayWinCanvas();
            Stop_timer();
            IncreaseTextSize();
            ChangeTextColor();
            _timerText.enabled = false;
        }
    }

    private void Stop_timer()
    {
        if (_timer != null)
        {
            _timer.enabled = false;
        }
    }

    private void IncreaseTextSize()
    {
        if (_timerText != null)
        {
            _timerText.fontSize = _increasedFontSize;
        }
    }

    private void ChangeTextColor()
    {
        if (_timerText != null)
        {
            _timerText.color = _winColor;
        }
    }

    private void DisplayWinCanvas()
    {
        if (_winCanvas != null)
        {
            _winCanvas.gameObject.SetActive(true);
            DisplayFinalTime();
        }
    }

    private void DisplayFinalTime()
    {
        if (_timer != null)
        {
            _timer.Win();
        }
    }
}