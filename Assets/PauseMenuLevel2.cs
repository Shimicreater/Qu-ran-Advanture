using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseMenuLevel2 : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject pauseMenuUI;   // Canvas / Panel Pause
    [SerializeField] private Slider volumeSlider;      // Slider volume di panel pause

    [Header("Audio")]
    [SerializeField] private AudioSource bgmSource;    // AudioSource BGM_Level2

    [Header("Scene Navigation")]
    [SerializeField] private string mainMenuScene = "Mainmenu"; // ganti kalau nama scenemu beda

    [Header("Player")]
    [SerializeField] private Rigidbody playerRb;       // drag Rigidbody Player ke sini

    private bool isPaused = false;
    private string volumeKey = "Volume_Level2";        // key PlayerPrefs khusus level 2

    void Start()
    {
        if (pauseMenuUI != null)
            pauseMenuUI.SetActive(false);

        float savedVolume = PlayerPrefs.GetFloat(volumeKey, 1f);

        if (volumeSlider != null)
        {
            volumeSlider.value = savedVolume;
            volumeSlider.onValueChanged.AddListener(OnVolumeChanged);
        }

        if (bgmSource != null)
            bgmSource.volume = savedVolume;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        Time.timeScale = 1f;
        isPaused = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused) ResumeGame();
            else PauseGame();
        }
    }

    public void PauseGame()
    {
        isPaused = true;
        Time.timeScale = 0f;

        if (pauseMenuUI != null)
            pauseMenuUI.SetActive(true);

        // berhentikan gerakan fisik player tanpa mengubah constraint
        if (playerRb != null)
        {
            playerRb.velocity = Vector3.zero;
            playerRb.angularVelocity = Vector3.zero;
        }

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1f;

        if (pauseMenuUI != null)
            pauseMenuUI.SetActive(false);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void OnResumeButton()
    {
        ResumeGame();
    }

    public void OnQuitToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(mainMenuScene);
    }

    public void OnVolumeChanged(float value)
    {
        if (bgmSource != null)
            bgmSource.volume = value;

        PlayerPrefs.SetFloat(volumeKey, value);
    }
}
