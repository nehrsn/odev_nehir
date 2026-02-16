using System;
using UnityEngine;

public class LanderAudio : MonoBehaviour
{
    [SerializeField] private AudioSource thrusterAudioSource;

    private LanderMovement landerMovement;

    private void Awake()
    {
        landerMovement = GetComponent<LanderMovement>();
    }

    private void Start()
    {
        landerMovement.OnBeforeForce += LanderMovement_OnBeforeForce;
        landerMovement.OnLeftForce += LanderMovement_OnLeftForce;
        landerMovement.OnUpForce += LanderMovement_OnUpForce;
        landerMovement.OnRightForce += LanderMovement_OnRightForce;

        SoundManager.Instance.OnSoundVolumeChanged += SoundManager_OnSoundVolumeChanged;
        thrusterAudioSource.Pause();

    }

    private void SoundManager_OnSoundVolumeChanged(object sender, EventArgs e)
    {
        thrusterAudioSource.volume = SoundManager.Instance.GetSoundVolumeNormalized();
    }

    private void LanderMovement_OnRightForce(object sender, EventArgs e)
    {
        if(!thrusterAudioSource.isPlaying){
            thrusterAudioSource.Play();
        }
    }

    private void LanderMovement_OnUpForce(object sender, EventArgs e)
    {
        if(!thrusterAudioSource.isPlaying){
            thrusterAudioSource.Play();
        }
    }

    private void LanderMovement_OnLeftForce(object sender, EventArgs e)
    {
        if(!thrusterAudioSource.isPlaying){
            thrusterAudioSource.Play();
        }
    }

    private void LanderMovement_OnBeforeForce(object sender, EventArgs e)
    {
        thrusterAudioSource.Pause();
    }
}
