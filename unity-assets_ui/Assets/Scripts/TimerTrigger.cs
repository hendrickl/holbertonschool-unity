using UnityEngine;

public class TimerTrigger : MonoBehaviour
{
    [SerializeField] private Timer _timer;

    private void Start()
    {
        if (_timer != null)
        {
            _timer.enabled = false;
        }
    }

    private void OnTriggerExit(Collider p_other)
    {
        if (p_other.CompareTag("Player"))
        {
            StartTimer();
        }
    }

    private void StartTimer()
    {
        if (_timer != null && !_timer.enabled)
        {
            _timer.enabled = true;
            _timer.StartTimer();
        }
    }
}
