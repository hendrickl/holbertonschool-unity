using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    // [SerializeField] private Canvas _pauseCanvas;
    [SerializeField] private GameObject _pauseCanvas;

    private bool _isPaused = false;

    private void Update() 
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseGame();
        }    
    }

    private void PauseGame()
    {
        Debug.Log($"PauseGame called");
        _isPaused = !_isPaused;
        _pauseCanvas.SetActive(_isPaused);
    }
}
