using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public class IntroSlide
{
    public string title;
    [TextArea(2, 5)]
    public string body;
    public Sprite image; // boleh dikosongkan
}

public class Level2IntroPanel : MonoBehaviour
{
    [Header("UI Root")]
    public GameObject rootPanel;

    [Header("UI Elements")]
    public TMP_Text titleText;
    public TMP_Text bodyText;
    public Image imageHolder;

    public Button prevButton;
    public Button nextButton;
    public Button startButton;

    [Header("Slides Data")]
    public IntroSlide[] slides;

    [Header("HUD To Hide While Intro")]
    public GameObject[] hudObjects;   // isi: MiniMap, HUD lain yang mau disembunyikan

    private int currentIndex = 0;
    private bool introActive = false;

    // ----------------------------------------------------
    // Helper HUD
    // ----------------------------------------------------
    private void SetHudActive(bool value)
    {
        if (hudObjects == null) return;

        foreach (var go in hudObjects)
        {
            if (go != null) go.SetActive(value);
        }
    }

    // Panel aktif (awal level)
    private void OnEnable()
    {
        introActive = true;

        // pause game dan tampilkan cursor
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // sembunyikan HUD (mini map, dsb.)
        SetHudActive(false);
    }

    private void Start()
    {
        if (prevButton != null) prevButton.onClick.AddListener(OnPrevClicked);
        if (nextButton != null) nextButton.onClick.AddListener(OnNextClicked);
        if (startButton != null) startButton.onClick.AddListener(OnStartClicked);

        ShowSlide(0);
    }

    // Selama intro aktif, paksa cursor tetap kelihatan
    private void Update()
    {
        if (introActive)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    private void ShowSlide(int index)
    {
        if (slides == null || slides.Length == 0) return;

        currentIndex = Mathf.Clamp(index, 0, slides.Length - 1);
        IntroSlide s = slides[currentIndex];

        if (titleText != null) titleText.text = s.title;
        if (bodyText != null) bodyText.text = s.body;

        if (imageHolder != null)
        {
            if (s.image != null)
            {
                imageHolder.gameObject.SetActive(true);
                imageHolder.sprite = s.image;
            }
            else
            {
                imageHolder.gameObject.SetActive(false);
            }
        }

        prevButton.interactable = currentIndex > 0;

        bool isLast = currentIndex == slides.Length - 1;
        nextButton.gameObject.SetActive(!isLast);
        startButton.gameObject.SetActive(isLast);
    }

    private void OnPrevClicked() => ShowSlide(currentIndex - 1);
    private void OnNextClicked() => ShowSlide(currentIndex + 1);

    private void OnStartClicked()
    {
        introActive = false;

        // resume game
        Time.timeScale = 1f;

        // mode gameplay: cursor hilang & locked
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // tampilkan lagi HUD (mini map, dll.)
        SetHudActive(true);

        if (rootPanel != null)
            rootPanel.SetActive(false);
    }

    // Fallback kalau panel tiba-tiba di-disable dari luar
    private void OnDisable()
    {
        if (introActive)
        {
            introActive = false;
            Time.timeScale = 1f;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            SetHudActive(true);
        }
    }
}
