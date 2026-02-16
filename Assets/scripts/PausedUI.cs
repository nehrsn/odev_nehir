using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PausedUI : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button mainMenuButton;
    [SerializeField] private Button soundVolumeButton;
    [SerializeField] private TextMeshProUGUI soundVolumeTextMesh;
    [SerializeField] private Button musicVolumeButton;
    [SerializeField] private TextMeshProUGUI musicVolumeTextMesh;

    public bool IsPaused;

    void Awake()
    {
        soundVolumeButton.onClick.AddListener(() =>
        {
            SoundManager.Instance.ChangeSoundVolume();
            soundVolumeTextMesh.text = "SOUND " + SoundManager.Instance.GetSoundVolume();
        });
        musicVolumeButton.onClick.AddListener(() =>
        {
            MusicManager.Instance.ChangeMusicVolume();   
            musicVolumeTextMesh.text = "MUSIC " + MusicManager.Instance.GetMusicVolume();
        });

        mainMenuButton.onClick.AddListener(() =>
        {
            GameManager.ResetStaticData();
            SceneLoader.LoadScene(SceneLoader.Scene.MainMenuScene);
        });

        resumeButton.onClick.AddListener(() =>
        {
            pauseMenu.SetActive(false);
            Time.timeScale = 1f;
        });


    }

    void Start()
    {
        soundVolumeTextMesh.text = "SOUND " + SoundManager.Instance.GetSoundVolume();
        musicVolumeTextMesh.text = "MUSIC " + MusicManager.Instance.GetMusicVolume();
        pauseMenu.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (IsPaused)
            {
                UnPauseGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    public void PauseGame()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
        IsPaused = true;
    }

    public void UnPauseGame()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        IsPaused = false;
    }
}
