using UnityEngine;
using UnityEngine.Audio;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;

public class mainMenu_optionsMenu : MonoBehaviour
{
    public AudioMixer audioMixer;
    Resolution[] availResolution;

    [SerializeField]
    private TMP_Dropdown resolutionDropDown;

    [SerializeField]
    private TMP_Dropdown qualityDropDown;

    [SerializeField]
    private Slider sensSlider;

    [SerializeField]
    private Slider volumeSlider;

    void Start()
    {
        availResolution = Screen.resolutions;
        resolutionDropDown.ClearOptions();
        List<string> options = new List<string>();
        int currentResIndex = 0;
        for (int i = 0; i < availResolution.Length; i++)
        {
            string option = availResolution[i].width + "x" + availResolution[i].height;
            options.Add(option);

            if (
                availResolution[i].width == Screen.currentResolution.width
                && availResolution[i].height == Screen.currentResolution.height
            )
            {
                currentResIndex = i;
            }
        }
        resolutionDropDown.AddOptions(options);
        resolutionDropDown.value = currentResIndex;
        resolutionDropDown.RefreshShownValue();

        qualityDropDown.value = QualitySettings.GetQualityLevel();
        qualityDropDown.RefreshShownValue();

        sensSlider.value = PlayerPrefs.GetFloat("user_senstivity", 200.0f);

        volumeSlider.value = PlayerPrefs.GetFloat("user_volume", 0f);
        audioMixer.SetFloat("volume", PlayerPrefs.GetFloat("user_volume", 0f));
    }

    public void SetVolume(float volume)
    {
        audioMixer.SetFloat("volume", volume);
        PlayerPrefs.SetFloat("user_volume", volume);
    }

    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
    }

    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }

    public void SetResolution(int resIndex)
    {
        Resolution resolution = availResolution[resIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    public void SetSenstivity(float sens)
    {
        PlayerPrefs.SetFloat("user_senstivity", sens);
    }
}
