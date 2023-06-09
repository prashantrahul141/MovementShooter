using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public class mainMenu_optionsMenu : MonoBehaviour
{
    public AudioMixer audioMixer;
    Resolution[] availResolution;

    [SerializeField]
    private TMP_Dropdown resolutionDropDown;

    [SerializeField]
    private TMP_Dropdown qualityDropDown;

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
    }

    public void SetVolume(float volume)
    {
        audioMixer.SetFloat("volume", volume);
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
}
