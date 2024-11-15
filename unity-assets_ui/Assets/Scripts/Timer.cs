using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    public TMP_Text timerText;
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
        float minutes = Mathf.FloorToInt(_elapsedTime / 60); // Mathf.FloorToInt() is a Unity function that rounds a floating point number down to the nearest integer.
        float seconds = Mathf.FloorToInt(_elapsedTime % 60);
        float milliseconds = (_elapsedTime % 1) * 100;

        string timeFormat = string.Format("{0:00}:{1:00}.{2:00}", minutes, seconds, milliseconds);
        timerText.text = timeFormat;
    }
}