using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class GraphicSettings : MonoBehaviour
{
    public TMP_Dropdown resolutionDropdown;
    public TMP_Dropdown windowModeDropdown;
    public TMP_Dropdown shadowQualityDropdown;
    public UnityEngine.UI.Slider brightnessSlider;

    Resolution[] resolutions;
    int savedResolutionIndex;

    void Start()
    {
        // Fetch all available resolutions and sort them by size (descending)
        resolutions = Screen.resolutions;
        System.Array.Sort(resolutions, (a, b) => (b.width * b.height).CompareTo(a.width * a.height));

        // Get the current monitor's refresh rate ratio
        var currentRefreshRateRatio = Screen.currentResolution.refreshRateRatio;

        // Limit the resolution options to the top 5 largest resolutions
        List<Resolution> limitedResolutions = new List<Resolution>();
        for (int i = 0; i < Mathf.Min(5, resolutions.Length); i++)
        {
            // Only include resolutions that match the current monitor's refresh rate ratio
            if (resolutions[i].refreshRateRatio.Equals(currentRefreshRateRatio))
            {
                limitedResolutions.Add(resolutions[i]);
            }
        }

        // Clear the resolution dropdown options
        resolutionDropdown.ClearOptions();

        // List to hold resolution options in the dropdown
        List<string> options = new List<string>();
        savedResolutionIndex = 0;

        // Populate the dropdown with the top 5 biggest resolutions
        for (int i = 0; i < limitedResolutions.Count; i++)
        {
            string option = limitedResolutions[i].width + " x " + limitedResolutions[i].height;
            options.Add(option);

            // Check if this resolution matches the saved resolution
            if (PlayerPrefs.HasKey("ResolutionWidth") && PlayerPrefs.HasKey("ResolutionHeight"))
            {
                int savedWidth = PlayerPrefs.GetInt("ResolutionWidth");
                int savedHeight = PlayerPrefs.GetInt("ResolutionHeight");

                if (limitedResolutions[i].width == savedWidth && limitedResolutions[i].height == savedHeight)
                {
                    savedResolutionIndex = i;  // Set the saved resolution index
                }
            }
            else if (limitedResolutions[i].width == Screen.currentResolution.width &&
                     limitedResolutions[i].height == Screen.currentResolution.height)
            {
                savedResolutionIndex = i;  // Default to current screen resolution if no saved resolution is found
            }
        }

        // Add the options to the dropdown and set the saved value
        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = savedResolutionIndex;
        resolutionDropdown.RefreshShownValue();

        // Apply the saved resolution on startup
        SetResolution(savedResolutionIndex);

        // Load other saved settings if they exist
        LoadSettings();
    }

    public void SetResolution(int resolutionIndex)
    {
        // Get the current monitor's refresh rate ratio
        var currentRefreshRateRatio = Screen.currentResolution.refreshRateRatio;

        // Retrieve the selected resolution from the limited list
        Resolution selectedResolution = resolutions[resolutionIndex];

        // Set the resolution, refresh rate, and fullscreen mode
        Screen.SetResolution(selectedResolution.width, selectedResolution.height, Screen.fullScreenMode, currentRefreshRateRatio);

        // Save the resolution settings for future use
        PlayerPrefs.SetInt("ResolutionWidth", selectedResolution.width);
        PlayerPrefs.SetInt("ResolutionHeight", selectedResolution.height);
        PlayerPrefs.SetInt("ResolutionIndex", resolutionIndex);
        PlayerPrefs.Save();
    }

    public void SetWindowMode(int modeIndex)
    {
        FullScreenMode selectedMode;

        // Map the dropdown index to the appropriate FullScreenMode
        switch (modeIndex)
        {
            case 0: // Fullscreen
                selectedMode = FullScreenMode.ExclusiveFullScreen;
                break;
            case 1: // Windowed
                selectedMode = FullScreenMode.Windowed;
                break;
            case 2: // Borderless Windowed
                selectedMode = FullScreenMode.FullScreenWindow;
                break;
            default:
                selectedMode = FullScreenMode.Windowed; // Default to windowed if something goes wrong
                break;
        }

        // Apply the fullscreen/windowed mode
        Screen.fullScreenMode = selectedMode;

        SaveSettings();
    }

    public void SetBrightness(float brightness)
    {
        // Adjust ambient lighting based on brightness value
        RenderSettings.ambientLight = Color.white * brightness;
        PlayerPrefs.SetFloat("Brightness", brightness);
    }

    public void SetShadowQuality(int qualityIndex)
    {
        // Adjust shadow quality based on dropdown selection
        switch (qualityIndex)
        {
            case 0:
                QualitySettings.shadows = ShadowQuality.Disable;
                break;
            case 1:
                QualitySettings.shadows = ShadowQuality.HardOnly;
                break;
            case 2:
                QualitySettings.shadows = ShadowQuality.All;
                break;
        }
        SaveSettings();
    }

    void SaveSettings()
    {
        // Save all relevant settings
        PlayerPrefs.SetInt("Resolution", resolutionDropdown.value);
        PlayerPrefs.SetInt("WindowMode", windowModeDropdown.value);
        PlayerPrefs.SetInt("ShadowQuality", shadowQualityDropdown.value);
        PlayerPrefs.SetFloat("Brightness", brightnessSlider.value);
        PlayerPrefs.Save();
    }

    void LoadSettings()
    {
        // Load saved resolution settings if available
        if (PlayerPrefs.HasKey("Resolution"))
        {
            resolutionDropdown.value = PlayerPrefs.GetInt("Resolution");
            SetResolution(resolutionDropdown.value);
        }

        // Load saved window mode settings if available
        if (PlayerPrefs.HasKey("WindowMode"))
        {
            windowModeDropdown.value = PlayerPrefs.GetInt("WindowMode");
            SetWindowMode(windowModeDropdown.value);
        }

        // Load saved shadow quality settings if available
        if (PlayerPrefs.HasKey("ShadowQuality"))
        {
            shadowQualityDropdown.value = PlayerPrefs.GetInt("ShadowQuality");
            SetShadowQuality(shadowQualityDropdown.value);
        }

        // Load saved brightness settings if available
        if (PlayerPrefs.HasKey("Brightness"))
        {
            brightnessSlider.value = PlayerPrefs.GetFloat("Brightness");
            SetBrightness(brightnessSlider.value);
        }
    }
}
