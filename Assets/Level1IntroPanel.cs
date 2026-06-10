using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public class IntroSlideL1
{
    public string title;

    [TextArea(2, 5)]
    public string body;

    public Sprite image;   // boleh kosong
}

public class Level1IntroPanel : MonoBehaviour
{
    [Header("UI Root")]
    public GameObject rootPanel;     // panel hijau utama

    [Header("UI Elements")]
    public TMP_Text titleText;
    public TMP_Text bodyText;
    public Image imageHolder;

    public Button prevButton;
    public Button nextButton;
    public Button startButton;

    [Header("Slides Data")]
    public IntroSlideL1[] slides;

    [Header("HUD yang disembunyikan saat intro")]
    [Tooltip("Isi dengan MiniMap, panel HUD, dll yang tidak boleh muncul saat slide ini tampil")]
    public GameObject[] hudObjects;

    private int currentIndex = 0;
    private bool introActive = false;

    // ---------------- HUD helper ----------------
    private void SetHudActive(bool value)
    {
        if (hudObjects == null) return;

        foreach (var go in hudObjects)
        {
            if (go != null) go.SetActive(value);
        }
    }

    // Dipanggil ketika panel diaktifkan (awal level)
    private void OnEnable()
    {
        introActive = true;

        // pause game & tampilkan cursor
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // sembunyikan HUD (termasuk minimap)
        SetHudActive(false);
    }

    private void Start()
    {
        if (prevButton != null) prevButton.onClick.AddListener(OnPrevClicked);
        if (nextButton != null) nextButton.onClick.AddListener(OnNextClicked);
        if (startButton != null) startButton.onClick.AddListener(OnStartClicked);

        ShowSlide(0);
    }

    private void Update()
    {
        // selama intro, paksa cursor tetap kelihatan
        if (introActive)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    // ---------------- Logic slide ----------------
    private void ShowSlide(int index)
    {
        if (slides == null || slides.Length == 0) return;

        currentIndex = Mathf.Clamp(index, 0, slides.Length - 1);
        IntroSlideL1 s = slides[currentIndex];

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

        if (prevButton != null)
            prevButton.interactable = currentIndex > 0;

        bool isLast = currentIndex == slides.Length - 1;

        if (nextButton != null)
            nextButton.gameObject.SetActive(!isLast);

        if (startButton != null)
            startButton.gameObject.SetActive(isLast);
    }

    private void OnPrevClicked() => ShowSlide(currentIndex - 1);
    private void OnNextClicked() => ShowSlide(currentIndex + 1);

    private void OnStartClicked()
    {
        introActive = false;

        // lanjut game
        Time.timeScale = 1f;

        // mode gameplay: cursor di-lock & disembunyikan
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // tampilkan kembali HUD (minimap dll.)
        SetHudActive(true);

        if (rootPanel != null)
            rootPanel.SetActive(false);
    }

    // fallback kalau panel tiba-tiba di-disable dari luar
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
