using System.Collections;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioSource _audioSource;

    [SerializeField] private AudioClip _audioOnClick;
    [SerializeField] private AudioClip _audioBackgroundMusic;

    private void Start()
    {
        // PlaySound(_audioBackgroundMusic);
    }

    public void TriggerAudio(AudioClip p_audioClip)
    {
        _audioSource.clip = p_audioClip;
        _audioSource.Play();
    }
}
