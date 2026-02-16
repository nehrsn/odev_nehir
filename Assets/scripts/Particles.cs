using System;
using UnityEngine;

public class Particles : MonoBehaviour
{

    [SerializeField] private ParticleSystem LeftThrusterParticleSystem;
    [SerializeField] private ParticleSystem MiddleThrusterParticleSystem;
    [SerializeField] private ParticleSystem RightThrusterParticleSystem;
    [SerializeField] private GameObject LanderExplotionVfx;

    private LanderMovement landerMovement; //en başta sadece hareketi buraya yaparım diye düşünmüştüm adını düzeltmeye üşendim şimdilik özür dilerim TvT

    void Awake()
    {
        SetEnabledThrusterParticleSystem(LeftThrusterParticleSystem, false);
        SetEnabledThrusterParticleSystem(MiddleThrusterParticleSystem, false);
        SetEnabledThrusterParticleSystem(RightThrusterParticleSystem, false);

        landerMovement = GetComponent<LanderMovement>();

        landerMovement.OnUpForce += LanderMovement_OnUpForce;
        landerMovement.OnLeftForce += LanderMovement_OnLeftForce;
        landerMovement.OnRightForce += LanderMovement_OnRightForce;
        landerMovement.OnBeforeForce += LanderMovement_OnBeforeForce;
    }

    private void Start()
    {
        landerMovement.OnLanded += LanderMovement_OnLanded;
    }

    private void LanderMovement_OnLanded(object sender, LanderMovement.OnLandedEventArgs e)
    {
        switch (e.landingType)
        {
            case LanderMovement.LandingType.TooFastLanding:
            case LanderMovement.LandingType.TooSteepAngle:
            case LanderMovement.LandingType.WrongLandingArea:
                Instantiate(LanderExplotionVfx, transform.position, Quaternion.identity);
                gameObject.SetActive(false);
                break; 
        }
    }

    private void LanderMovement_OnBeforeForce(object sender, System.EventArgs e)
    {
        SetEnabledThrusterParticleSystem(LeftThrusterParticleSystem, false);
        SetEnabledThrusterParticleSystem(MiddleThrusterParticleSystem, false);
        SetEnabledThrusterParticleSystem(RightThrusterParticleSystem, false);
    }

    private void LanderMovement_OnUpForce(object sender, System.EventArgs e)
    {
        SetEnabledThrusterParticleSystem(LeftThrusterParticleSystem, true);
        SetEnabledThrusterParticleSystem(MiddleThrusterParticleSystem, true);
        SetEnabledThrusterParticleSystem(RightThrusterParticleSystem, true);
        
    }

    private void LanderMovement_OnLeftForce(object sender, System.EventArgs e)
    {
        SetEnabledThrusterParticleSystem(LeftThrusterParticleSystem, false);
        SetEnabledThrusterParticleSystem(MiddleThrusterParticleSystem, false);
        SetEnabledThrusterParticleSystem(RightThrusterParticleSystem, true);
        
    }

    private void LanderMovement_OnRightForce(object sender, System.EventArgs e)
    {
        SetEnabledThrusterParticleSystem(LeftThrusterParticleSystem, true);
        SetEnabledThrusterParticleSystem(MiddleThrusterParticleSystem, false);
        SetEnabledThrusterParticleSystem(RightThrusterParticleSystem, false);
        
    }

    private void SetEnabledThrusterParticleSystem(ParticleSystem particleSystem, bool enabled)
    {
        ParticleSystem.EmissionModule emissionModule = particleSystem.emission;
        emissionModule.enabled = enabled;
    }

}
