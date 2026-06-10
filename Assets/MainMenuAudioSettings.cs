using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuAudioSettings : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private Slider volumeSlider;

    [Header("Audio Sources")]
    [SerializeField] private AudioSource bgmSource;     // BGM_MainMenu
    [SerializeField] private AudioSource uiSfxSource;   // UISoundManager

    private const string VolumeKey = "MasterVolume";

    private void Awake()
    {
        // Ambil volume tersimpan, default 1
        float savedVolume = PlayerPrefs.GetFloat(VolumeKey, 1f);

        // Set slider & apply ke audio
        if (volumeSlider != null)
        {
            volumeSlider.minValue = 0f;
            volumeSlider.maxValue = 1f;
            volumeSlider.value = savedVolume;
            volumeSlider.onValueChanged.AddListener(OnVolumeChanged);
        }

        ApplyVolume(savedVolume);
    }

    private void OnDestroy()
    {
        if (volumeSlider != null)
        {
            volumeSlider.onValueChanged.RemoveListener(OnVolumeChanged);
        }
    }

    private void OnVolumeChanged(float value)
    {
        ApplyVolume(value);
        PlayerPrefs.SetFloat(VolumeKey, value);
    }

    private void ApplyVolume(float value)
    {
        if (bgmSource != null)
            bgmSource.volume = value;

        if (uiSfxSource != null)
            uiSfxSource.volume = value;

        // Optional: master volume seluruh game
        AudioListener.volume = value;
    }
}
