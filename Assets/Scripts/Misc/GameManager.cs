using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("UI Elements")]
    [SerializeField] public GameObject pauseMenu;
    [SerializeField] public GameObject restartConfirmation;
    [SerializeField] public GameObject homeConfirmation;
    [SerializeField] public AudioSource audioSource;
    [SerializeField] public GameObject winLevelPanel;
    [SerializeField] public GameObject gameOverPanel;
    [SerializeField] public SceneTransitions sceneTransitions;
    [SerializeField] public float sceneOffset;
    private bool gameStarted;
    private bool isPaused;

    [Header("Settings")]
    [SerializeField] public GameObject objectivesPage;
    [SerializeField] public GameObject settingsPage;
    [SerializeField] public GameObject audioMenu;
    [SerializeField] public GameObject graphicsMenu;
    [SerializeField] public GameObject aboutMenu;
    [SerializeField] public Slider masterVolumeSlider;
    [SerializeField] public Slider musicVolumeSlider;
    [SerializeField] public Slider sfxVolumeSlider;
    [SerializeField] public AudioMixer audioMixer;
    [SerializeField] public PlayerLevelSaveData playerLevelSaveData;
    [SerializeField] public int levelUnlockedIndex;

    void Start()
    {
        Instance = this;
        pauseMenu.SetActive(false);
        objectivesPage.SetActive(true);
        settingsPage.SetActive(false);
        winLevelPanel.SetActive(false);
        gameOverPanel.SetActive(false);
        Time.timeScale = 1f;
        SetUpVolumeLevels();
        isPaused = false;
        StartCoroutine(nameof(GameBegin));
    }

    void Update()
    {
        if(gameStarted){
        if (Input.GetKeyDown(KeyCode.P) || Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                ResumePauseMenu();
                isPaused = false;
            }
            else
            {
                OpenPauseMenu();
                isPaused = true;
            }
        }
        }
    }

    public void OpenPauseMenu()
    {
        Time.timeScale = 0f;
        pauseMenu.SetActive(true);
        objectivesPage.SetActive(true);
        settingsPage.SetActive(false);
        audioSource.Pause();
        Cursor.lockState = CursorLockMode.None;
    }

    public void OpenSettingsMenu()
    {
        objectivesPage.SetActive(false);
        settingsPage.SetActive(true);
    }

    public void CloseSettingsMenu()
    {
        objectivesPage.SetActive(true);
        settingsPage.SetActive(false);
    }

    public void ConfirmRestart(){
        restartConfirmation.SetActive(true);
    }

    public void DeclineRestart(){
        restartConfirmation.SetActive(false);
    }
    public void RestartGame()
    {
        Time.timeScale = 1f;
        StartCoroutine(nameof(CloseSceneTransition));
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
    }

    public void ConfirmReturnToHome(){
        homeConfirmation.SetActive(true);
    }

    public void DeclineReturnToHome(){
        homeConfirmation.SetActive(false);
    }

    public void ReturnToMenu()
    {
        Time.timeScale = 1f;
        StartCoroutine(nameof(CloseSceneTransition));
        SceneManager.LoadSceneAsync("MenuScene");
    }

    public void ResumePauseMenu()
    {
        Time.timeScale = 1f;
        pauseMenu.SetActive(false);
        objectivesPage.SetActive(false);
        settingsPage.SetActive(false);
        audioSource.UnPause();
        isPaused = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void SetMasterVolume()
    {
        float value = masterVolumeSlider.value;
        audioMixer.SetFloat("MasterVolumeParameter", Mathf.Log10(value) * 20);
        if (PlayerPrefs.HasKey("MasterVolume"))
        {
            PlayerPrefs.SetFloat("MasterVolume", value);
        }
        else
        {
            PlayerPrefs.SetFloat("MasterVolume", value);
        }
    }

    public void SetMusicVolume()
    {
        float value = musicVolumeSlider.value;
        audioMixer.SetFloat("MusicVolumeParameter", Mathf.Log10(value) * 20);
        if (PlayerPrefs.HasKey("MusicVolume"))
        {
            PlayerPrefs.SetFloat("MusicVolume", value);
        }
        else
        {
            PlayerPrefs.SetFloat("MusicVolume", value);
        }
    }

    public void SetSFXVolume()
    {
        float value = sfxVolumeSlider.value;
        audioMixer.SetFloat("SFXVolumeParameter", Mathf.Log10(value) * 20);
        if (PlayerPrefs.HasKey("SFXVolume"))
        {
            PlayerPrefs.SetFloat("SFXVolume", value);
        }
        else
        {
            PlayerPrefs.SetFloat("SFXVolume", value);
        }
    }

    public void SetUpVolumeLevels()
    {
        if (PlayerPrefs.HasKey("MasterVolume"))
        {
            float val1 = PlayerPrefs.GetFloat("MasterVolume");
            audioMixer.SetFloat("MasterVolumeParameter", Mathf.Log10(val1) * 20);
            masterVolumeSlider.value = val1;
        }
        else
        {
            SetMasterVolume();
        }

        if (PlayerPrefs.HasKey("MusicVolume"))
        {
            float val2 = PlayerPrefs.GetFloat("MusicVolume");
            audioMixer.SetFloat("MusicVolumeParameter", Mathf.Log10(val2) * 20);
            musicVolumeSlider.value = val2;
        }
        else
        {
            SetMusicVolume();
        }

        if (PlayerPrefs.HasKey("SFXVolume"))
        {
            float val3 = PlayerPrefs.GetFloat("SFXVolume");
            audioMixer.SetFloat("SFXVolumeParameter", Mathf.Log10(val3) * 20);
            musicVolumeSlider.value = val3;
        }
        else
        {
            SetSFXVolume();
        }
    }
    public void ViewAudioSettings()
    {
        audioMenu.SetActive(true);
        graphicsMenu.SetActive(false);
        aboutMenu.SetActive(false);
    }

    public void ViewGraphicsSettings()
    {
        audioMenu.SetActive(false);
        graphicsMenu.SetActive(true);
        aboutMenu.SetActive(false);
    }

    public void ViewAbout()
    {
        audioMenu.SetActive(false);
        graphicsMenu.SetActive(false);
        aboutMenu.SetActive(true);
    }
    public void WinLevel()
    {
        audioSource.Pause();
        Time.timeScale = 0f;
        winLevelPanel.SetActive(true);
        playerLevelSaveData.SaveLevelCompletion(levelUnlockedIndex);
        Cursor.lockState = CursorLockMode.None;
    }

    public void NextLevel(){
        Time.timeScale = 1f;
        StartCoroutine(nameof(CloseSceneTransition));
    }

    public void GameOverLevel(){
        Time.timeScale = 0f;
        gameOverPanel.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
    }

    IEnumerator GameBegin(){
        yield return new WaitForSeconds(2f);
        gameStarted = true;
    }

    IEnumerator CloseSceneTransition(){
        sceneTransitions.CloseTransition();
        yield return new WaitForSeconds(sceneOffset);
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
