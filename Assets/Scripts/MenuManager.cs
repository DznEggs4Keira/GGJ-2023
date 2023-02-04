using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class MenuManager : MonoBehaviour
{
    [Header("Settings Menu Values")]
    public AudioMixer audioMixer;
    public TMP_Dropdown resolutionsDropdown;
    private Resolution[] resolutions;

    [Header("Pause Menu Values")]
    public GameObject PauseMenu;
    private bool isPaused = false;

    // Start is called before the first frame update
    void Start()
    {

        resolutions = Screen.resolutions;
        resolutionsDropdown.ClearOptions();

        List<string> options = new List<string>();

        int currentResolutionIndex = 0;
        for (int i = 0; i < resolutions.Length; i++) {
            string option = resolutions[i].width + " x " + resolutions[i].height;

            options.Add(option);

            if(resolutions[i].width == Screen.currentResolution.width &&
               resolutions[i].height == Screen.currentResolution.height) {
                currentResolutionIndex = i;
            }
        }

        resolutionsDropdown.AddOptions(options);
        resolutionsDropdown.value = currentResolutionIndex;
        resolutionsDropdown.RefreshShownValue();

    }

    private void Update() {

        if (Input.GetKeyDown(KeyCode.Escape)) {
            if(isPaused) {
                isPaused = false;
                PauseMenu.SetActive(false);
                Time.timeScale = 1;
            } else {
                Pause();
                isPaused = true;
            }
            
        }
    }

    public void Play() {
        Time.timeScale = 1;
    }

    public void Pause() {
        Time.timeScale = 0;
        PauseMenu.SetActive(true);
    }

    public void Exit() {
        Application.Quit();
    }

    #region SETTINGS MENU

    public void SetMusicVolume(float volume) {
        audioMixer.SetFloat("MusicVolume", volume);
    }

    public void SetSFXVolume(float volume) {
        audioMixer.SetFloat("SFXVolume", volume);
    }

    public void SetQuality(int quality) {
        QualitySettings.SetQualityLevel(quality);
    }

    public void SetFullscreen(bool isFullscreen) {
        Screen.fullScreen = isFullscreen;
    }

    public void SetResolution(int resolutionIndex) {
        Resolution resolution = resolutions[resolutionIndex];

        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    #endregion
}
