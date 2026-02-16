using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class LanderMovement : MonoBehaviour
{
    private const float GRAVITY_NORMAL = 0.9f;

    public static LanderMovement Instance { get; private set;} //burayı anlamadım Q-Q
    public event EventHandler OnUpForce;
    public event EventHandler OnRightForce;
    public event EventHandler OnLeftForce;
    public event EventHandler OnBeforeForce;
    public event EventHandler OnCoinPickUp;
    public event EventHandler<OnStateChangedEventArgs> OnStateChanged;
    public class OnStateChangedEventArgs : EventArgs
    {
        public State state;
    }
    public event EventHandler<OnLandedEventArgs> OnLanded;
    public class OnLandedEventArgs : EventArgs
    {
        public LandingType landingType;
        public int score;
        public float dotVector;
        public float landingSpeed;
        public float scoreMultiplier;
    }

    public enum LandingType
    {
        Success,
        WrongLandingArea,
        TooSteepAngle,
        TooFastLanding,
    }

    public enum State
    {
        WaitingToStart,
        Normal,
        GameOver,
    }

    private Rigidbody2D rb;
    float force = 500f;
    float torque = 100f;
    float fuelAmountMax = 10f;
    float fuelAmount;
    private State state;


    void Awake()
    {
        Instance = this;
        rb = GetComponent<Rigidbody2D>();
        fuelAmount = fuelAmountMax;
        rb.gravityScale = 0f;
        state = State.WaitingToStart;
    }

    void FixedUpdate()
    {
        OnBeforeForce?.Invoke(this, EventArgs.Empty);

        switch (state)
        {
            default:
            case State.WaitingToStart:
                if(Keyboard.current.upArrowKey.isPressed || Keyboard.current.leftArrowKey.isPressed || Keyboard.current.rightArrowKey.isPressed)
                {
                    rb.gravityScale = GRAVITY_NORMAL;
                    SetState(State.Normal);
                }
                break;
            case State.Normal:
                if(fuelAmount <= 0)
                {
                    return;
                }

                if(Keyboard.current.upArrowKey.isPressed || Keyboard.current.leftArrowKey.isPressed || Keyboard.current.rightArrowKey.isPressed)
                {
                    ConsumeFuel();
                    
                }

                if (Keyboard.current.upArrowKey.isPressed)
                {
                    rb.AddForce(force * transform.up * Time.deltaTime * 5);
                    OnUpForce?.Invoke(this, EventArgs.Empty);
        
                }
                if (Keyboard.current.leftArrowKey.isPressed)
                {
                    rb.AddTorque(torque * Time.deltaTime);
                    OnLeftForce?.Invoke(this, EventArgs.Empty);
            
                }
                if (Keyboard.current.rightArrowKey.isPressed)
                {
                    rb.AddTorque(-torque * Time.deltaTime);
                    OnRightForce?.Invoke(this, EventArgs.Empty);
                }
                break;
            case State.GameOver:
                break;
        }
    }
    void OnCollisionEnter2D(Collision2D collision2D)
    {
        float softLanding = 5f;
        float landingSpeed = collision2D.relativeVelocity.magnitude;
        float minDotVector = 0.92f;
        float dotVector = Vector2.Dot(Vector2.up, transform.up);

        if(!collision2D.gameObject.TryGetComponent(out LandingPad landingPad))
        {
            Debug.Log("Landed on dirt!");
            OnLanded?.Invoke(this, new OnLandedEventArgs
            {
                landingType = LandingType.WrongLandingArea,
                dotVector = dotVector,
                landingSpeed = landingSpeed,
                scoreMultiplier = 0,
                score = 0,
            });
            SetState(State.GameOver);
            return;
        }

        if(landingSpeed > softLanding)
        {
            Debug.Log("Landed too harsh!");
            OnLanded?.Invoke(this, new OnLandedEventArgs
            {
                landingType = LandingType.TooFastLanding,
                dotVector = dotVector,
                landingSpeed = landingSpeed,
                scoreMultiplier = landingPad.GetScoreMultiplier(),
                score = 0,
            });
            SetState(State.GameOver);
            return;
        }

        if(dotVector < minDotVector)
        {
            Debug.Log("Landed too yamuk!");
            OnLanded?.Invoke(this, new OnLandedEventArgs
            {
                landingType = LandingType.TooSteepAngle,
                dotVector = dotVector,
                landingSpeed = landingSpeed,
                scoreMultiplier = 0f,
                score = 0,
            });
            SetState(State.GameOver);
            return;
        }

        
        Debug.Log("landed successfully!");
        float maxScoreAmountLandingAngle = 100f;
        float scoreDotVectorMultiplier = 10f;
        float landingAngleScore = maxScoreAmountLandingAngle - Mathf.Abs(dotVector - 1f) * scoreDotVectorMultiplier * maxScoreAmountLandingAngle;

        float landingSpeedScoreMultiplier = 100f;
        float landingSpeedScore = (softLanding - landingSpeed) * landingSpeedScoreMultiplier;

        Debug.Log("Landing angle score: " + landingAngleScore);
        Debug.Log("Landing speed score: " + landingSpeedScore);
        
        int score = Mathf.RoundToInt((landingAngleScore + landingSpeedScore) * landingPad.GetScoreMultiplier());
        Debug.Log("Total Score: " + score);
        OnLanded?.Invoke(this, new OnLandedEventArgs
        {
            landingType = LandingType.Success,
            dotVector = dotVector,
            landingSpeed = landingSpeed,
            scoreMultiplier = landingPad.GetScoreMultiplier(),
            score = score,
        });
        SetState(State.GameOver);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.TryGetComponent(out FuelPickUp fuelPickUp))
        {
            float addFuelAmount = 10f;
            fuelAmount =+ addFuelAmount;
            if(fuelAmount > fuelAmountMax)
            {
                fuelAmount = fuelAmountMax;
            }
            fuelPickUp.DestroySelf();
        }

        if(collision.gameObject.TryGetComponent(out CoinPickUp coinPickUp))
        {
            OnCoinPickUp?.Invoke(this, EventArgs.Empty);
            coinPickUp.DestroyCoin();
        }
    }

    private void SetState(State state)
    {
        this.state = state;
        OnStateChanged?.Invoke(this, new OnStateChangedEventArgs
        {
            state = state,
        });
    }

    private void ConsumeFuel()
    {
        float fuelConsumptionAmount = 1f;
        fuelAmount -= fuelConsumptionAmount * Time.deltaTime;
    }
    public float GetFuel()
    {
        return fuelAmount;
    }

    public float GetFuelAmountNormalized()
    {
        return fuelAmount/fuelAmountMax;
    }
    public float GetSpeedX()
    {
        return rb.linearVelocityX;
    }

    public float GetSpeedY()
    {
        return rb.linearVelocityY;
    }

    internal class OnstateChangedEventArgs
    {
    }
}
