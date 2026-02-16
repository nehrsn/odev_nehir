using System;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField] private AudioClip fuelPickUpAudioClip;
    [SerializeField] private AudioClip coinPickUpAudioClip;
    private void Start()
    {
        LanderMovement.Instance.OnFuelPickUp += LanderMovement_OnFuelPickUp;
        LanderMovement.Instance.OnCoinPickUp += LanderMovement_OnCoinPickUp;
    }

    private void LanderMovement_OnCoinPickUp(object sender, EventArgs e)
    {
        AudioSource.PlayClipAtPoint(coinPickUpAudioClip, Camera.main.transform.position);
    }

    private void LanderMovement_OnFuelPickUp(object sender, EventArgs e)
    {
        AudioSource.PlayClipAtPoint(fuelPickUpAudioClip, Camera.main.transform.position);
    }
}
