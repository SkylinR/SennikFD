using System;
using UnityEngine;

public static class PlayerOptions {
    public static Action<float> OnAudioVolumeChanged;

    private const string AUDIO_VOLUME_PREF = "audio_volume_pref";
    private const float INITIAL_AUDIO_VOLUME = 1.0f;

    private const string CAMERA_SENSIVITY_PREF = "camera_sensivity_pref";
    private const float INITIAL_CAMERA_SENSIVITY = 0.5f;

    public static float AudioVolume => GetAudioVolume();
    public static float CameraSensivity => GetCameraSensivity();

    public static void SetAudioVolume(float volume) {
        if (volume >= 0 && volume <= 1f && volume != AudioVolume) {
            PlayerPrefs.SetFloat(AUDIO_VOLUME_PREF, volume);
            OnAudioVolumeChanged?.Invoke(volume);
        }
    }

    public static void SetCameraSensivity(float cameraSensivity) {
        if (cameraSensivity >= 0 && cameraSensivity <= 1f && cameraSensivity != CameraSensivity) {
            PlayerPrefs.SetFloat(CAMERA_SENSIVITY_PREF, cameraSensivity);
        }
    }

    private static float GetAudioVolume() {
        return PlayerPrefs.GetFloat(AUDIO_VOLUME_PREF, INITIAL_AUDIO_VOLUME);
    }

    private static float GetCameraSensivity() {
        return PlayerPrefs.GetFloat(CAMERA_SENSIVITY_PREF, INITIAL_CAMERA_SENSIVITY);
    }
}