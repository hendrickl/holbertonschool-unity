using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenesManager : MonoBehaviour
{
    public static int PreviousSceneIndex;

    private void Start()
    {
        Debug.Log($"ScenesManager called");
    }

    public static string GetCurrentScene()
    {
        Debug.Log($"ScenesManager - The current scene is {SceneManager.GetActiveScene().name}");
        return SceneManager.GetActiveScene().name;
    }

    public static void SaveCurrentScene()
    {
        PlayerPrefs.SetInt("PreviousScene", SceneManager.GetActiveScene().buildIndex);
    }

    public static void LoadScene(int levelIndex)
    {
        SceneManager.LoadScene(levelIndex);
    }
    
    public static void LevelSelect(int levelIndex)
    {
        SaveCurrentScene();
        LoadScene(levelIndex);
    }
}
