using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LaunchMenu : MonoBehaviour
{
    [SerializeField] private Button _startButton; 
    [SerializeField] private Button _quitButton;

    [SerializeField] private GameObject _logoBigFormat;
    [SerializeField] private GameObject _logoSmallFormat;

    private void Start()
    {
        _startButton.gameObject.SetActive(false);
        _quitButton.gameObject.SetActive(false);
        _logoSmallFormat.SetActive(false);

        StartCoroutine("AnimLogo");
    }

    private IEnumerator AnimLogo()
    {
        yield return new WaitForSeconds(2.5f);
        _logoBigFormat.SetActive(false);
        _logoSmallFormat.SetActive(true);

        _startButton.gameObject.SetActive(true);
        _quitButton.gameObject.SetActive(true);
    }

    public void LevelSelect(int level)
    {
        SceneManager.LoadScene(level);
    }

    public void ExitGame()
    {
        Debug.Log("Exit button clicked");
        // Application.Quit();
    }
}
