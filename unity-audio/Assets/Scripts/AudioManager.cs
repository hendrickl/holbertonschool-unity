using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [SerializeField] private AudioMixer _audioMixer;

    [SerializeField] private AudioSource _bgmAudioSource;
    [SerializeField] private AudioSource _winAudioSource;

    [SerializeField] private AudioClip _cheerryMondayClip;
    [SerializeField] private AudioClip _victoryClip;
    [SerializeField] private AudioClip _wallPaperClip;
    [SerializeField] private AudioClip _porchSwingDaysClip;
    [SerializeField] private AudioClip _brittleRilleClip;

    [SerializeField] private string _defaultSnapshotName = "Default";
    [SerializeField] private string _pausedSnapshotName = "Paused";

    public Slider bgmSlider;
    private bool _musicStarted = false;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        PlayLevelBackgroundMusic();
    }

    // start - Code snippet to handle BGM resolution
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        _musicStarted = false;
        PlayLevelBackgroundMusic();
    }
    // end

    public void PlayLevelBackgroundMusic()
    {
        if (_musicStarted) return;

        if (ScenesManager.GetCurrentSceneName() == "Level01")
        {
            _bgmAudioSource.clip = _cheerryMondayClip;
        }
        else if (ScenesManager.GetCurrentSceneName() == "Level02")
        {
            _bgmAudioSource.clip = _porchSwingDaysClip;
        }
        else if (ScenesManager.GetCurrentSceneName() == "Level03")
        {
            _bgmAudioSource.clip = _brittleRilleClip;
        }
        else
        {
            _bgmAudioSource.clip = _wallPaperClip;
        }

        _bgmAudioSource.Play();
        _musicStarted = true;
        Debug.Log($"AudioManager - For scene: {ScenesManager.GetCurrentSceneName()}, Current BGM is: {_bgmAudioSource.clip}");
    }

    public void StopLevelBackgroundMusic()
    {
        if (_bgmAudioSource.isPlaying)
        {
            _bgmAudioSource.Stop();
            _musicStarted = false;
            Debug.Log($"AudioManager - Stopped BGM & Reset _musicStarted flag to {_musicStarted}");
        }
    }

    public void PauseLevelbackgroundMusic()
    {
        if (_bgmAudioSource.isPlaying)
        {
            _bgmAudioSource.Pause();
        }
    }

    public void ResumeLevelbackgroundMusic()
    {
        _bgmAudioSource.UnPause();
    }

    public void PlayVictorySting()
    {
        StopLevelBackgroundMusic();

        if (_winAudioSource != null && _victoryClip != null)
        {
            _winAudioSource.clip = _victoryClip;
            _winAudioSource.Play();
        }
        else
        {
            Debug.Log($"AudioManager - Missing AudioSource or Clip for VictorySting");
        }
    }

    public void ApplyDefaultSnapshot()
    {
        _audioMixer.FindSnapshot(_defaultSnapshotName).TransitionTo(0.5f);
        Debug.Log($"AudioManager - Default snapshot applied");
    }

    public void ApplyPausedSnapshot()
    {
        _audioMixer.FindSnapshot(_pausedSnapshotName).TransitionTo(0.5f);
        Debug.Log($"AudioManager - Paused snapshot applied");
    }
    
    public void OnBGMValueChanged()
    {
        _bgmAudioSource.volume = bgmSlider.value;
        Debug.Log($"OptionsMenu - BGM Value : {bgmSlider.value}");
    }
}
