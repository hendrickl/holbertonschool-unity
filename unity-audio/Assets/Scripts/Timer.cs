using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    [SerializeField] private TMP_Text _timerText;
    [SerializeField] private TMP_Text _finalTimeText;
    private float _elapsedTime = 0f;
    private bool _isRunning = false;

    private void Update()
    {
        if (_isRunning)
        {
            _elapsedTime += Time.deltaTime; // in seconde
            UpdateTimer();
        }
    }

    public void StartTimer()
    {
        if (!_isRunning)
        {
            _isRunning = true;
        }
    }

    private void UpdateTimer()
    {
        float l_minutes = Mathf.FloorToInt(_elapsedTime / 60); // Mathf.FloorToInt() is a Unity function that rounds a floating point number down to the nearest integer.
        float l_seconds = Mathf.FloorToInt(_elapsedTime % 60);
        float l_milliseconds = (_elapsedTime % 1) * 100;

        string l_timeFormat = string.Format("{0:00}:{1:00}.{2:00}", l_minutes, l_seconds, l_milliseconds);
        _timerText.text = l_timeFormat;
    }

    public void Win()
    {
        _isRunning = false;
        _finalTimeText.text = _timerText.text;
    }
}