using UnityEngine;
using UnityEngine.UI;

public class VolumeSlider : MonoBehaviour
{
    private enum VolumeType
    {
        Master,
        Music,
        SFX,
        Ambience
    }
    
    [Header("Type")]
    [SerializeField] private VolumeType volumeType;
    
    [Header("Slider")]
    private Slider volumeSlider;

    private void Awake()
    {
        volumeSlider = this.GetComponent<Slider>();
    }

    private void Update()
    {
        switch (volumeType)
        {
            case VolumeType.Master:
                volumeSlider.value = AudioManager.Instance.masterVolume;
                break;
            case VolumeType.Music:
                volumeSlider.value = AudioManager.Instance.musicVolume;
                break;
            case VolumeType.SFX:
                volumeSlider.value = AudioManager.Instance.SFXVolume;
                break;
            case VolumeType.Ambience:
                volumeSlider.value = AudioManager.Instance.ambienceVolume;
                break;
        }
    }
    
    public void OnSliderValueChanged()
    {
        switch (volumeType)
        {
            case VolumeType.Master:
                AudioManager.Instance.masterVolume = volumeSlider.value;
                break;
            case VolumeType.Music:
                AudioManager.Instance.musicVolume = volumeSlider.value;
                break;
            case VolumeType.SFX:
                AudioManager.Instance.SFXVolume = volumeSlider.value;
                break;
            case VolumeType.Ambience:
                AudioManager.Instance.ambienceVolume = volumeSlider.value;
                break;
        }
    }
}
