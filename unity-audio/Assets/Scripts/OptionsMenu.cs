using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class OptionsMenu : MonoBehaviour
{
    [SerializeField] private Button _applyButton;
    [SerializeField] private Button _backButton;
    [SerializeField] private Toggle _invertYButton;

    private float _bgmValue;

    private void Start()
    {
        bool isYaxisInverted = PlayerPrefs.GetInt("Y-axisIsInverted", 0) == 1;
        ScenesManager.PreviousSceneIndex = PlayerPrefs.GetInt("PreviousScene");

        if (AudioManager.Instance != null && AudioManager.Instance.bgmSlider != null)
        {
            _bgmValue = PlayerPrefs.GetFloat("BGMValue", AudioManager.Instance.bgmSlider.value);
        }
        else
        {
            _bgmValue = PlayerPrefs.GetFloat("BGMValue", 0.5f); 
        }

        if (_invertYButton != null)
        {
            _invertYButton.isOn = isYaxisInverted;
        }

        if (_applyButton != null)
        {
            _applyButton.onClick.AddListener(Apply);
        }

        if (_backButton != null)
        {
            _backButton.onClick.AddListener(Back);
        }

        if (AudioManager.Instance != null && AudioManager.Instance.bgmSlider != null)
        {
            AudioManager.Instance.bgmSlider.onValueChanged.AddListener(delegate { AudioManager.Instance.OnBGMValueChanged(); });
        }
    }

    private void OnDestroy()
    {
        if (_applyButton != null)
        {
            _applyButton.onClick.RemoveListener(Apply);
        }

        if (_backButton != null)
        {
            _backButton.onClick.RemoveListener(Back);
        }
    }

    public void Apply()
    {
        PlayerPrefs.SetInt("Y-axisIsInverted", _invertYButton.isOn ? 1 : 0);

        if (AudioManager.Instance != null && AudioManager.Instance.bgmSlider != null)
        {
            PlayerPrefs.SetFloat("BGMValue", AudioManager.Instance.bgmSlider.value);
            AudioManager.Instance.OnBGMValueChanged();
        }

        PlayerPrefs.Save();
        SceneManager.LoadScene(ScenesManager.PreviousSceneIndex);
    }

    public void Back()
    {
        SceneManager.LoadScene(ScenesManager.PreviousSceneIndex);
    }
}
