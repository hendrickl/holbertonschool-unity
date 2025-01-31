using System.Collections;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioSource _audioSource;

    [SerializeField] private AudioClip _audioOnClick;
    [SerializeField] private AudioClip _audioBackgroundMusic;

    private void Start()
    {
        TriggerAudio(_audioBackgroundMusic);
    }

    public void TriggerAudio(AudioClip p_audioClip)
    {
        if (p_audioClip == _audioBackgroundMusic)
        {
            _audioSource.loop = true;
        }
        else
        {
            _audioSource.loop = false;
        }
            
        _audioSource.clip = p_audioClip;
        _audioSource.Play();
    }
}
