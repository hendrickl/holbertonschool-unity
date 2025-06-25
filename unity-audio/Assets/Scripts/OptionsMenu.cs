using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class OptionsMenu : MonoBehaviour
{
    [SerializeField] private Button _applyButton;
    [SerializeField] private Button _backButton;
    [SerializeField] private Toggle _invertYButton;
    [SerializeField] private Slider _bgmSlider;
    [SerializeField] private AudioSource _bgmAudiosource;

    private int _previousSceneIndex;
    private float _bgmValue;

    private void Start()
    {
        bool isYaxisInverted = PlayerPrefs.GetInt("Y-axisIsInverted", 0) == 1;
        _previousSceneIndex = PlayerPrefs.GetInt("PreviousScene");
        _bgmValue = PlayerPrefs.GetFloat("BGMValue", _bgmSlider.value);

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

        if (_bgmSlider != null)
        {
            _bgmSlider.onValueChanged.AddListener(delegate { OnBGMValueChanged(); });
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
        PlayerPrefs.SetFloat("BGMValue", _bgmSlider.value);
        PlayerPrefs.Save();
        SceneManager.LoadScene(_previousSceneIndex);
    }

    public void Back()
    {
        SceneManager.LoadScene(_previousSceneIndex);
    }

    public void OnBGMValueChanged()
    {
        _bgmAudiosource.volume = _bgmSlider.value;
        Debug.Log($"OptionsMenu - BGM Value : {_bgmSlider.value}");
    }
}
