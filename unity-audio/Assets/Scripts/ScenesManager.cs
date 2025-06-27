using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenesManager : MonoBehaviour
{
    public static int PreviousSceneIndex;
    public static int CurrentSceneIndex;

    public static string GetCurrentSceneName()
    {
        Debug.Log($"ScenesManager - The current scene name is {SceneManager.GetActiveScene().name}");
        return SceneManager.GetActiveScene().name;
    }

    public static int GetCurrentSceneIndex()
    {
        Debug.Log($"ScenesManager - The current scene index is {SceneManager.GetActiveScene().buildIndex}");
        return SceneManager.GetActiveScene().buildIndex;
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
