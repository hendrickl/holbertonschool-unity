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
        Debug.Log($"Y-axis is inverted: {isYaxisInverted}");

        _previousSceneIndex = PlayerPrefs.GetInt("PreviousScene");
        Debug.Log($"OptionsMenu - Previous scene index is {_previousSceneIndex}");

        if (_invertYButton != null)
            _invertYButton.isOn = isYaxisInverted;

        Debug.Log($"Invert button value: {isYaxisInverted}");

        _applyButton.onClick.AddListener(Apply);
        _backButton.onClick.AddListener(Back);
    }

    public void Apply()
    {
        PlayerPrefs.SetInt("Y-axisIsInverted", _invertYButton.isOn ? 1 : 0);
        PlayerPrefs.Save();

        Debug.Log($"OptionsMenu - Invert button value:{_invertYButton.isOn}");
        Debug.Log($"OptionsMenu - Previous scene index is {_previousSceneIndex}");

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
