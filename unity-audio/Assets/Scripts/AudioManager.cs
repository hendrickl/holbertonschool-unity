using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [SerializeField] private AudioMixer _audioMixer;

    [SerializeField] private AudioSource _bgmAudioSource;
    [SerializeField] private AudioSource _winAudioSource;

    [SerializeField] private AudioClip _cheerryMondayClip;
    [SerializeField] private AudioClip _victoryClip;
    [SerializeField] private AudioClip _wallPaperClip;

    [SerializeField] private string _defaultSnapshotName = "Default";
    [SerializeField] private string _pausedSnapshotName = "Paused";

    private bool _musicStarted = false;
    private string _currentScene;

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
        InitializeLevelBackgroundMusic();
    }

    public void InitializeLevelBackgroundMusic()
    {
        if (_musicStarted) return;


        if (_currentScene == "Level01")
        {
            _bgmAudioSource.clip = _cheerryMondayClip;
            _bgmAudioSource.Play();
            _musicStarted = true;
        }

        if (_currentScene == "Options")
        {
            _bgmAudioSource.clip = _wallPaperClip;
            _bgmAudioSource.Play();
            _musicStarted = true;
        }

        if (_currentScene == "MainMenu")
        {
            _bgmAudioSource.clip = _wallPaperClip;
            _bgmAudioSource.Play();
            _musicStarted = true;
        }
    }

    public void StopLevelBackgroundMusic()
    {
        if (_bgmAudioSource.isPlaying)
        {
            _bgmAudioSource.Stop();
            _musicStarted = false;
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
}
