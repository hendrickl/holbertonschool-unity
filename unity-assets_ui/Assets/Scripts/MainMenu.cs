using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private Button _level01Button;
    [SerializeField] private Button _level02Button;
    [SerializeField] private Button _level03Button;

    private void Start()
    {
        if (_level01Button != null)
            _level01Button.onClick.AddListener(() => LevelSelect(1));
        
        if (_level02Button != null)
            _level02Button.onClick.AddListener(() => LevelSelect(2));
        
        if (_level03Button != null)
            _level03Button.onClick.AddListener(() => LevelSelect(3));
    }

    public void LevelSelect(int p_level)
    {
        string sceneName = $"Level0{p_level}";
        Debug.Log($"Loading the level: {sceneName}");
        SceneManager.LoadScene(sceneName);
    }

    private void OnDestroy()
    {
        if (_level01Button != null)
            _level01Button.onClick.RemoveAllListeners();
        
        if (_level02Button != null)
            _level02Button.onClick.RemoveAllListeners();
        
        if (_level03Button != null)
            _level03Button.onClick.RemoveAllListeners();
    }
}