using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class Menu : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] public GameObject mainMenu;
    [SerializeField] public GameObject levelSelectMenu;
    [SerializeField] public GameObject map1Menu;
    [SerializeField] public GameObject map2Menu;
    [SerializeField] public GameObject settingsMenu;
    [SerializeField] public SceneTransitions sceneTransitions;
    [SerializeField] public float sceneOffset;

    [Header("Audio Settings")]
    [SerializeField] public GameObject audioMenu;
    [SerializeField] public Slider masterVolumeSlider;
    [SerializeField] public Slider musicVolumeSlider;
    [SerializeField] public Slider sfxVolumeSlider;
    [SerializeField] public AudioMixer audioMixer;

    [Header("Graphics Settings")]
    [SerializeField] public GameObject graphicsMenu;
    [SerializeField] public GameObject aboutMenu;

    [Header("Level Information")]
    [SerializeField] public List<LevelInfo> levelList = new List<LevelInfo>();
    [SerializeField] private LevelInfo selectedLevel;
    [SerializeField] private TMP_Text levelNameText;
    [SerializeField] private TMP_Text levelDescriptionText;
    [SerializeField] private TMP_Text levelObjectivesText;
    [SerializeField] private GameObject levelInfoPanel;

    void Start()
    {
        mainMenu.SetActive(true);
        settingsMenu.SetActive(false);
        SetUpVolumeLevels();
        ViewAudioSettings();
        CloseSelectLevel();
    }

    public void OpenSelectLevel(int index){
        for(int i = 0; i<levelList.Count;i++){
            if(i == index){
                selectedLevel = levelList[i];
                SetUpLevelInfoScreen();
                return;
            }
        }
    }

    public void CloseSelectLevel(){
        levelInfoPanel.SetActive(false);
    }

    private void SetUpLevelInfoScreen(){
        levelNameText.text = selectedLevel.levelName;
        levelDescriptionText.text = selectedLevel.levelDescription;
        levelObjectivesText.text = selectedLevel.levelObjectives;
        levelInfoPanel.SetActive(true);
    }

    public void GoToLevelScene()
    {
        StartCoroutine(nameof(StartLevelScene));
    }

    IEnumerator StartLevelScene(){
        sceneTransitions.CloseTransition();
        yield return new WaitForSeconds(sceneOffset);
        SceneManager.LoadSceneAsync(selectedLevel.levelIndexName);
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

    public void SwitchMap(){
        map1Menu.SetActive(!map1Menu.activeSelf);
        map2Menu.SetActive(!map2Menu.activeSelf);
    }
}

[System.Serializable]
public class LevelInfo
{
    public string levelName;
    [TextArea]
    public string levelDescription;
    [TextArea]
    public string levelObjectives;
    public string levelIndexName;
}