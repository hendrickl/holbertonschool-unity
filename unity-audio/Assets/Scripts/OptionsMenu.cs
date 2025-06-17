using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class OptionsMenu : MonoBehaviour
{
    [SerializeField] private Button _applyButton;
    [SerializeField] private Button _backButton;
    [SerializeField] private Toggle _invertYButton;

    private int _previousSceneIndex;

    private void Start()
    {
        bool isYaxisInverted = PlayerPrefs.GetInt("Y-axisIsInverted", 0) == 1;
        _previousSceneIndex = PlayerPrefs.GetInt("PreviousScene");

        if (_invertYButton != null)
        {
            _invertYButton.isOn = isYaxisInverted;
        }

        _applyButton.onClick.AddListener(Apply);
        _backButton.onClick.AddListener(Back);
    }

    public void Apply()
    {
        PlayerPrefs.SetInt("Y-axisIsInverted", _invertYButton.isOn ? 1 : 0);
        PlayerPrefs.Save();
        SceneManager.LoadScene(_previousSceneIndex);
    }

    public void Back()
    {
        SceneManager.LoadScene(_previousSceneIndex);
    }

    private void OnDestroy() 
    {
        _applyButton.onClick.RemoveListener(Apply);
        _backButton.onClick.RemoveListener(Back);
    }
}
