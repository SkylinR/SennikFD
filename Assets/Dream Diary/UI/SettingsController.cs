using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

public class SettingsController : MonoBehaviour
{
    [SerializeField] Slider audioVolumeSlider;
    [SerializeField] Slider cameraSensivitySlider;

    float startAudioVolume;
    float startCameraSensivity;

    void Awake() {
        Assert.IsNotNull(audioVolumeSlider);
        Assert.IsNotNull(cameraSensivitySlider);
    }

    void OnEnable() {
        audioVolumeSlider.SetValueWithoutNotify(PlayerOptions.AudioVolume);
        startAudioVolume = PlayerOptions.AudioVolume;

        cameraSensivitySlider.SetValueWithoutNotify(PlayerOptions.CameraSensivity);
        startCameraSensivity = PlayerOptions.CameraSensivity;
    }

    public void ChangeVolume(float volume) {
        PlayerOptions.SetAudioVolume(volume);
    }

    public void ChangeCameraSensivity(float cameraSensivity) {
        PlayerOptions.SetCameraSensivity(cameraSensivity);
    }

    public void ResetToPrevious() {
        PlayerOptions.SetAudioVolume(startAudioVolume);
        PlayerOptions.SetCameraSensivity(startCameraSensivity);
    }
}
