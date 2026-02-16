 using System;
using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance{get; private set;}
    private static int totalScore = 0;
    private static int levelNumber = 1;

    public static void ResetStaticData()
    {
        levelNumber = 1;
        totalScore = 0;
        Time.timeScale = 1f;
    }
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
        GameLevel gameLevel = GetGameLevel();
        GameLevel spawnedGameLevel = Instantiate(gameLevel, Vector3.zero, Quaternion.identity);
        LanderMovement.Instance.transform.position = spawnedGameLevel.GetLanderStartPosition();
        cinemachineCamera.Target.TrackingTarget = spawnedGameLevel.GetCameraStartTargetTransform();
        CameraZoom.Instance.SetTargetOrthographicSize(spawnedGameLevel.GetZoomedOutOrthographicSize());
        
    }

    private GameLevel GetGameLevel()
    {
        foreach(GameLevel gameLevel in gameLevelList)
        {
            if(gameLevel.GetLevelNumber() == levelNumber)
            {
                return gameLevel;
            }
        }
        return null;
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

    public int GetTotalScore()
    {
        return totalScore;
    }

    public void GoToNextLevel()
    {
        totalScore += score;
        levelNumber++;
        if(GetGameLevel() == null)
        {
            SceneLoader.LoadScene(SceneLoader.Scene.GameOverScene);
        }
        else
        {
            SceneLoader.LoadScene(SceneLoader.Scene.GameScene);  
        }
    }

    public void RetryLevel()
    {
        SceneLoader.LoadScene(SceneLoader.Scene.GameScene); 
    }



}
