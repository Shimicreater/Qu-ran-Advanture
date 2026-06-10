using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseMenuLevel : MonoBehaviour
{
    public static bool GamePaused = false;

    [Header("UI")]
    [SerializeField] private GameObject pauseMenuUI;
    [SerializeField] private Slider volumeSlider;

    [Header("Audio")]
    [SerializeField] private AudioSource bgmSource;   // isi: AudioSource di BGM_Level1

    [Header("Scene")]
    [SerializeField] private string mainMenuScene = "Mainmenu";  // sesuaikan nama scene-mu

    private const string VolumeKey = "MasterVolume";

    private void Start()
    {
        // ambil volume tersimpan
        float v = PlayerPrefs.GetFloat(VolumeKey, 1f);

        if (volumeSlider != null)
        {
            volumeSlider.minValue = 0f;
            volumeSlider.maxValue = 1f;
            volumeSlider.value = v;
            volumeSlider.onValueChanged.AddListener(OnVolumeChanged);
        }

        ApplyVolume(v);
        Resume();   // pastikan game tidak ke-pause waktu mulai
    }

    private void OnDestroy()
    {
        if (volumeSlider != null)
            volumeSlider.onValueChanged.RemoveListener(OnVolumeChanged);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GamePaused) Resume();
            else Pause();
        }
    }

    public void Pause()
    {
        if (pauseMenuUI == null)
        {
            Debug.LogWarning("PauseMenuLevel: pauseMenuUI belum di-assign / sudah hilang.");
            Time.timeScale = 0f;
            GamePaused = true;
            return;
        }

        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GamePaused = true;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void Resume()
    {
        if (pauseMenuUI == null)
        {
            Debug.LogWarning("PauseMenuLevel: pauseMenuUI belum di-assign / sudah hilang.");
            Time.timeScale = 1f;
            GamePaused = false;
            return;
        }

        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GamePaused = false;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void BackToMainMenu()
    {
        Time.timeScale = 1f;
        GamePaused = false;
        SceneManager.LoadScene(mainMenuScene);
    }

    private void OnVolumeChanged(float value)
    {
        ApplyVolume(value);
        PlayerPrefs.SetFloat(VolumeKey, value);
    }

    private void ApplyVolume(float v)
    {
        AudioListener.volume = v;          // semua SFX ikut
        if (bgmSource != null)
            bgmSource.volume = v;          // BGM level ini
    }
}

