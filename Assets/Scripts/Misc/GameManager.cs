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

    /*
    [Header("Dev Panel")]
    [SerializeField] public GameObject debugPanel;
    [Header("Elara Debug Panel")]
    [SerializeField] public GameObject elaraDebugPanel;*/
    [SerializeField] public PlayerController elaraController;
    /*
    [SerializeField] public TMP_InputField elaraSprintSpeed;
    [SerializeField] public TMP_InputField elaraJumpHeight;

    [Header("Happy Debug Panel")]
    [SerializeField] public GameObject happyDebugPanel;*/
    [SerializeField] public RobotController happyController;
    /*
    [SerializeField] public TMP_InputField happySprintSpeed;
    [SerializeField] public TMP_InputField happyJumpHeight;
    */

    void Start()
    {
        Instance = this;
        pauseMenu.SetActive(false);
        objectivesPage.SetActive(true);
        settingsPage.SetActive(false);
        winLevelPanel.SetActive(false);
        Time.timeScale = 1f;
        SetUpVolumeLevels();
        isPaused = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
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

        /*
    public void OpenDebugTools()
    {
        pauseMenu.SetActive(false);
        settingsMenu.SetActive(false);
        debugPanel.SetActive(true);
    }
    private void SetupDebugPanel()
    {
        if(happyController != null)
        {
            elaraDebugPanel.SetActive(true);
            elaraSprintSpeed.text = elaraController.sprintSpeed.ToString();
            elaraJumpHeight.text = elaraController.jumpSpeed.ToString();

            happyDebugPanel.SetActive(true);
            happySprintSpeed.text = happyController.sprintSpeed.ToString();
            happyJumpHeight.text = happyController.jumpSpeed.ToString();
        }
        else
        {
            elaraDebugPanel.SetActive(true);
            elaraSprintSpeed.text = elaraController.sprintSpeed.ToString();
            elaraJumpHeight.text = elaraController.jumpSpeed.ToString();

            happyDebugPanel.SetActive(false);
        }
    }

    public void AddCharacterSprintSpeed(int target)
    {
        switch (target)
        {
            case 0:
                elaraController.sprintSpeed = int.Parse(elaraSprintSpeed.text);
                break;
            case 1:
                happyController.sprintSpeed = int.Parse(happySprintSpeed.text);
                break;
            default:
                elaraController.sprintSpeed = int.Parse(elaraSprintSpeed.text);
                break;
        }
    }

    public void AddCharacterJumpForce(int target)
    {
        switch (target)
        {
            case 0:
                elaraController.jumpSpeed = int.Parse(elaraJumpHeight.text);
                break;
            case 1:
                happyController.jumpSpeed = int.Parse(happyJumpHeight.text);
                break;
            default:
                elaraController.jumpSpeed = int.Parse(happyJumpHeight.text);
                break;
        }
    }
    */

    public void WinLevel()
    {
        if(happyController != null)
        {
            elaraController.isActive = false;
            happyController.isActive = false;
            audioSource.Pause();
            winLevelPanel.SetActive(true);
        }
        else
        {
            elaraController.isActive = false;
            audioSource.Pause();
            winLevelPanel.SetActive(true);
        }
        Cursor.lockState = CursorLockMode.None;
    }
}
