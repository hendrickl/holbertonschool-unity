using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [SerializeField] private AudioSource _bgmAudioSource;
    [SerializeField] private AudioSource _winAudioSource;
    [SerializeField] private AudioClip _cheerryMondayClip;
    [SerializeField] private AudioClip _victoryClip;

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

    private void Start()
    {
        InitializeLevelBackgroundMusic();
    }

    public void InitializeLevelBackgroundMusic()
    {
        if (_musicStarted) return;

        _bgmAudioSource.clip = _cheerryMondayClip;
        _bgmAudioSource.loop = true;
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
}
