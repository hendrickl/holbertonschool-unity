using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip _cherryMondayClip;

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
        _audioSource.clip = _cherryMondayClip;
        _audioSource.loop = true;
        _audioSource.Play();
    }

    public void StopLevelBackgroundMusic()
    {
        if (_audioSource.isPlaying)
        {
            _audioSource.Stop();
        }
    }

    public void PauseLevelbackgroundMusic()
    {
        if (_audioSource.isPlaying)
        {
            _audioSource.Pause();
        }
    }

    public void ResumeLevelbackgroundMusic()
    {
        _audioSource.UnPause();
    }
}
