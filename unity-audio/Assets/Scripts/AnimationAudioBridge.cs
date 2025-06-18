using UnityEngine;

public class AnimationAudioBridge : MonoBehaviour
{
    public PlayerController player;

    public void PlayLandingSFX()
    {
        if (player != null)
        {
            player.PlayLandingSFX();
        }
    } 
}
