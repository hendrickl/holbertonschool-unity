using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private Canvas _pauseMenu;

    [SerializeField] private Button _restartButton;   
    [SerializeField] private Button _resumeButton;  

    private void Start()
    {
        if (_pauseMenu != null)
            _pauseMenu.gameObject.SetActive(false);
    }

    private void Update() 
    {
        if (Input.GetKeyDown(KeyCode.Escape) && _pauseMenu != null)
        {
            _pauseMenu.gameObject.SetActive(true);
        }    
    } 

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);    
    }
}
