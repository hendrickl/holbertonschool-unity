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
        if (ScenesManager.GetCurrentSceneName() == "Level01")
        {
            PlayLevelBackgroundMusic();
        }
    }

    public void PlayLevelBackgroundMusic()
    {
        if (_musicStarted) return;
 
        string sceneName = ScenesManager.GetCurrentSceneName();

        if (sceneName == "Level01" || sceneName == "Level02" || sceneName == "Level03")
        {
            _bgmAudioSource.clip = _cheerryMondayClip;
        }
        else if (sceneName == "Options" || sceneName == "MainMenu")
        {
            _bgmAudioSource.clip = _cheerryMondayClip;
        }

        _bgmAudioSource.Play();
        _musicStarted = true;
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
