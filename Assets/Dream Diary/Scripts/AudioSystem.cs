using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSystem : MonoBehaviour
{
    private void Awake() {
        AudioListener.volume = PlayerOptions.AudioVolume;
    }

    private void OnEnable() {
        PlayerOptions.OnAudioVolumeChanged += HandleOnVolumeChange;
    }

    private void OnDisable() {
        PlayerOptions.OnAudioVolumeChanged -= HandleOnVolumeChange;
    }

    private void HandleOnVolumeChange(float volume) {
        if (volume >= 0 && volume <= 1) {
            AudioListener.volume = volume;
        }
    }
}
