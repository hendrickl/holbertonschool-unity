using UnityEngine;
using UnityEngine.UI;

// This script manages win menu navigation
public class WinMenu : MonoBehaviour
{
    [SerializeField] private Button _menuButton;
    [SerializeField] private Button _nextButton;

    private const int MAIN_MENU_INDEX = 0;
    private const int LAST_LEVEL_INDEX = 4;

    private void Start()
    {
        if (_menuButton != null)
        {
            _menuButton.onClick.AddListener(MainMenu);
        }

        if (_nextButton != null)
        {
            _nextButton.onClick.AddListener(Next);
        }
    }

    private void OnDestroy() 
    {
        if (_menuButton != null)
        {
            _menuButton.onClick.RemoveListener(MainMenu);    
        }

        if (_nextButton != null)
        {
            _nextButton.onClick.RemoveListener(Next);
        }
    }

    public void MainMenu()
    {
        ScenesManager.LoadScene(0);
    }

    public void Next()
    {
        int currentSceneIndex = ScenesManager.GetCurrentSceneIndex();

        if (currentSceneIndex >= LAST_LEVEL_INDEX)
        {
            ScenesManager.LoadScene(MAIN_MENU_INDEX);
        }
        else
        {
            ScenesManager.LoadScene(currentSceneIndex + 1);
        }
    }
}
