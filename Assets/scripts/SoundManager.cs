using System;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    private const int SOUND_VOLUME_MAX = 10;

    public static SoundManager Instance {get; private set;}
    private static int soundVolume = 6;

    public event EventHandler OnSoundVolumeChanged;

    [SerializeField] private AudioClip fuelPickUpAudioClip;
    [SerializeField] private AudioClip coinPickUpAudioClip;
    [SerializeField] private AudioClip landingSuccessAudioClip;
    [SerializeField] private AudioClip crashAudioClip;

    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        LanderMovement.Instance.OnFuelPickUp += LanderMovement_OnFuelPickUp;
        LanderMovement.Instance.OnCoinPickUp += LanderMovement_OnCoinPickUp;
        LanderMovement.Instance.OnLanded += LanderMovement_OnLanded;
    }

    private void LanderMovement_OnLanded(object sender, LanderMovement.OnLandedEventArgs e)
    {
        switch (e.landingType)
        {
            case LanderMovement.LandingType.Success:
                AudioSource.PlayClipAtPoint(landingSuccessAudioClip, Camera.main.transform.position, GetSoundVolumeNormalized());
                break;
            default:
                AudioSource.PlayClipAtPoint(crashAudioClip, Camera.main.transform.position, GetSoundVolumeNormalized());
                break;

        }
    }

    private void LanderMovement_OnCoinPickUp(object sender, EventArgs e)
    {
        AudioSource.PlayClipAtPoint(coinPickUpAudioClip, Camera.main.transform.position, GetSoundVolumeNormalized());
    }

    private void LanderMovement_OnFuelPickUp(object sender, EventArgs e)
    {
        AudioSource.PlayClipAtPoint(fuelPickUpAudioClip, Camera.main.transform.position, GetSoundVolumeNormalized());
    }

    public void ChangeSoundVolume()
    {
        soundVolume = (soundVolume + 1) % SOUND_VOLUME_MAX;
        OnSoundVolumeChanged?.Invoke(this, EventArgs.Empty);
    }

    public int GetSoundVolume()
    {
        return soundVolume;
    }
    public float GetSoundVolumeNormalized()
    {
        return ((float)soundVolume) / SOUND_VOLUME_MAX;
    }
}
