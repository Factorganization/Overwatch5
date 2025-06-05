using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Slider = UnityEngine.UI.Slider;
using Toggle = UnityEngine.UI.Toggle;

public class Settings : MonoBehaviour
{
    [SerializeField] private Slider _bgmSlider, _sfxSlider, _mouseSensitivitySlider;
    [SerializeField] private Toggle _fullscreenToggle;
    [SerializeField] private TMP_Dropdown _resolutionDropdown;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SetResolutionDropdownOptions();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void ToggleFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }
    
    public void ResolutionChanged(int resolutionIndex)
    {
        Resolution[] resolutions = Screen.resolutions;
        if (resolutionIndex < 0 || resolutionIndex >= resolutions.Length) return;

        Resolution selectedResolution = resolutions[resolutionIndex];
        Screen.SetResolution(selectedResolution.width, selectedResolution.height, Screen.fullScreen);
    }
    
    private void SetResolutionDropdownOptions()
    {
        Resolution[] resolutions = Screen.resolutions;
        List<string> options = new List<string>();

        foreach (var resolution in resolutions)
        {
            options.Add($"{resolution.width} x {resolution.height}");
        }

        _resolutionDropdown.ClearOptions();
        _resolutionDropdown.AddOptions(options);
        
        // Set the current resolution as the selected option
        int currentResolutionIndex = System.Array.FindIndex(resolutions, r => r.width == Screen.width && r.height == Screen.height);
        if (currentResolutionIndex >= 0)
        {
            _resolutionDropdown.value = currentResolutionIndex;
            _resolutionDropdown.RefreshShownValue();
        }
    }
    
    public void SetMouseSensitivity(float sensitivity)
    {
        // Assuming you have a method to set mouse sensitivity in your game
        // MouseSensitivityManager.SetSensitivity(sensitivity);
        Debug.Log($"Mouse sensitivity set to: {sensitivity}");
    }
    
    public void SetBGMVolume(float volume)
    {
        // Assuming you have a method to set BGM volume in your game
        // AudioManager.SetBGMVolume(volume);
        Debug.Log($"BGM volume set to: {volume}");
    }
}
