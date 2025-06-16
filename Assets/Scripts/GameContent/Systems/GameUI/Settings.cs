using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Slider = UnityEngine.UI.Slider;
using Toggle = UnityEngine.UI.Toggle;

public class Settings : MonoBehaviour
{
    [SerializeField] private Slider _mouseSensitivitySlider;
    [SerializeField] private Toggle _fullscreenToggle;
    [SerializeField] private TMP_Dropdown _resolutionDropdown;
    
    public Slider MouseSensitivitySlider => _mouseSensitivitySlider;
    
    void Start()
    {
        SetResolutionDropdownOptions();
        
        _fullscreenToggle.onValueChanged.AddListener(delegate { ToggleFullscreen(); });
        _resolutionDropdown.onValueChanged.AddListener(ResolutionChanged);
    }

    private void ToggleFullscreen()
    {
        Screen.fullScreen = _fullscreenToggle.isOn;
    }
    
    private void ResolutionChanged(int resolutionIndex)
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
}
