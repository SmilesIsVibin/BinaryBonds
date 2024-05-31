using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] public GameObject mainMenu;
    [SerializeField] public GameObject levelSelectMenu;
    [SerializeField] public GameObject settingsMenu;

    [Header("Audio Settings")]
    [SerializeField] public GameObject audioMenu;
    [SerializeField] public Slider masterVolumeSlider;
    [SerializeField] public Slider musicVolumeSlider;
    [SerializeField] public Slider sfxVolumeSlider;
    [SerializeField] public AudioMixer audioMixer;

    [Header("Graphics Settings")]
    [SerializeField] public GameObject graphicsMenu;
    [SerializeField] public GameObject aboutMenu;

    void Start()
    {
        mainMenu.SetActive(true);
        settingsMenu.SetActive(false);
        SetUpVolumeLevels();
        ViewAudioSettings();
    }

    public void GoToNextLevelScene(string sceneName)
    {
        SceneManager.LoadSceneAsync(sceneName);
    }

    #region Menu Navigation
    public void GoToSettings()
    {
        mainMenu.SetActive(false);
        levelSelectMenu.SetActive(false);
        settingsMenu.SetActive(true);
    }

    public void GoToMenu()
    {
        mainMenu.SetActive(true);
        levelSelectMenu.SetActive(false);
        settingsMenu.SetActive(false);
    }

    public void GoToLevelSelect()
    {
        mainMenu.SetActive(false);
        levelSelectMenu.SetActive(true);
        settingsMenu.SetActive(false);
    }
    #endregion

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

    #region Audio Functions
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
    #endregion

    public void ExitGame()
    {
        Application.Quit();
    }
}
