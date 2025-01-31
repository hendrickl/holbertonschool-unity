using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public AudioManager AudioManager;
    public PauseMenu PauseMenu;
    
    [SerializeField] private AudioClip _audioOnClick;

    [SerializeField] private Button _startButton;
    [SerializeField] private Button _quitButton;

    [SerializeField] private GameObject _logoBigFormat;
    [SerializeField] private GameObject _logoSmallFormat;

    private void Start()
    {
        if (_startButton != null)
            _startButton.gameObject.SetActive(false);
        
        if (_quitButton != null)
            _quitButton.gameObject.SetActive(false);
        
        if (_logoSmallFormat != null)
            _logoSmallFormat.SetActive(false);

        StartCoroutine("AnimLogo");
        StartCoroutine("AnimButton");
    }

    private IEnumerator AnimLogo()
    {
        yield return new WaitForSeconds(2f);

        if (_logoBigFormat != null)
            _logoBigFormat.SetActive(false);

        yield return new WaitForSeconds(2f);

        
        if (_logoSmallFormat != null)
            _logoSmallFormat.SetActive(true);
    }

    private IEnumerator AnimButton()
    {
        yield return new WaitForSeconds(2.5f);

        if (_startButton != null)
            _startButton.gameObject.SetActive(true);

        yield return new WaitForSeconds(1f);

        if (_quitButton != null)
            _quitButton.gameObject.SetActive(true);
    }

    private IEnumerator LoadSceneAfterOnClickSoundCoroutine(int index, float time)
    {
        yield return new WaitForSeconds(time);
        LoadScene(index);
    }

    private void LoadScene(int index)
    {
        SceneManager.LoadScene(index);
    }

    public void LoadSceneWithOnClickSound(int index)
    {
        AudioManager.TriggerAudio(_audioOnClick);
        StartCoroutine(LoadSceneAfterOnClickSoundCoroutine(index, 0.35f));
    }

    public void RestartGame()
    {
        PauseMenu.Restart();   
    }

    public void ExitGame()
    {
        Debug.Log("Exit button clicked");
        // Application.Quit();
    }
}
