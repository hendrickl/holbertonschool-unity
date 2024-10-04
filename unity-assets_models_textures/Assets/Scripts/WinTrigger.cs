using UnityEngine;
using TMPro;

public class WinTrigger : MonoBehaviour
{
    [SerializeField] private Timer _timer;
    [SerializeField] private TMP_Text _timerText;
    [SerializeField] private float _increasedFontSize = 60f;
    [SerializeField] private Color _winColor = Color.green;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Stop_timer();
            IncreaseTextSize();
            ChangeTextColor();
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
}