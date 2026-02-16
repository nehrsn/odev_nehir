 using System;
using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance{get; private set;}

    private static int levelNumber = 1;
    [SerializeField]private List<GameLevel> gameLevelList;
    [SerializeField]private CinemachineCamera cinemachineCamera;
    private int score;
    private float time;
    private bool isTimerActive;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        LanderMovement.Instance.OnCoinPickUp += LanderMovement_OnCoinPickUp;
        LanderMovement.Instance.OnLanded += LanderMovement_OnLanded;
        LanderMovement.Instance.OnStateChanged += LanderMovement_OnStateChanged;

        LoadCurrentLevel();
    }



    private void LanderMovement_OnStateChanged(object sender, LanderMovement.OnStateChangedEventArgs e )
    {
        isTimerActive = e.state == LanderMovement.State.Normal;
        if(e.state == LanderMovement.State.Normal)
        {
            cinemachineCamera.Target.TrackingTarget = LanderMovement.Instance.transform;
            CameraZoom.Instance.SetNormalOrthographicSize();
        }
    }

    private void Update()
    {
        if (isTimerActive)
        {
            time += Time.deltaTime;
        }
    }
    private void LoadCurrentLevel()
    {
        foreach(GameLevel gameLevel in gameLevelList)
        {
            if(gameLevel.GetLevelNumber() == levelNumber)
            {
                GameLevel spawnedGameLevel = Instantiate(gameLevel, Vector3.zero, Quaternion.identity);
                LanderMovement.Instance.transform.position = spawnedGameLevel.GetLanderStartPosition();
                cinemachineCamera.Target.TrackingTarget = spawnedGameLevel.GetCameraStartTargetTransform();
                CameraZoom.Instance.SetTargetOrthographicSize(spawnedGameLevel.GetZoomedOutOrthographicSize());
            }
        }
    }

    private void LanderMovement_OnLanded(object sender, LanderMovement.OnLandedEventArgs e)
    {
        AddScore(e.score);
    }

    private void LanderMovement_OnCoinPickUp(object sender, System.EventArgs e)
    {
        AddScore(500);
        
    }
    public void AddScore (int addScoreAmount)
    {
        score += addScoreAmount;
        Debug.Log(score);
    }
    public int GetScore()
    {
        return score;
    }
    public float GetTime()
    {
        return time;
    }

    public void GoToNextLevel()
    {
        levelNumber++;
        SceneLoader.LoadScene(SceneLoader.Scene.GameScene);  
    }

    public void RetryLevel()
    {
        SceneLoader.LoadScene(SceneLoader.Scene.GameScene); 
    }

    public void PauseGame()
    {
        Time.timeScale = 0f;
    }
    public void UnpauseGame()
    {
        Time.timeScale = 1f;
    }

}
