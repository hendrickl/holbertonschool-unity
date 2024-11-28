using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class WinMenu : MonoBehaviour
{
    [SerializeField] private Button _menuButton;

    private void Start()
    {
        if (_menuButton != null)
            _menuButton.onClick.AddListener(MainMenu);
    }

    private void OnDestroy() 
    {
        if (_menuButton != null)
            _menuButton.onClick.RemoveListener(MainMenu);    
    }

    public void MainMenu()
    {
        SceneManager.LoadScene(0);
    }
}
