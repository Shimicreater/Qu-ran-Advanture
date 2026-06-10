using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class UstadDialog1 : MonoBehaviour
{
    [Header("UI Dialog Ustad")]
    public TextMeshProUGUI textComponent;
    public float textSpeed = 0.02f;

    [Header("Pilihan Ya / Tidak")]
    public GameObject choicePanel;   // panel berisi tombol Ya & Tidak
    public Button yesButton;
    public Button noButton;

    [Header("Kalimat kalau HURUF BELUM lengkap")]
    [TextArea(2, 4)] public string[] incompleteLines;

    [Header("Kalimat kalau HURUF SUDAH lengkap")]
    [TextArea(2, 4)] public string[] completeLines;

    [Header("Level Berikutnya")]
    public string nextSceneName = "level 2";

    private string[] activeLines;
    private int index;
    private bool waitingForChoice = false;
    private bool allLettersCollected = false;

    private void Awake()
    {
        if (yesButton != null)
        {
            yesButton.onClick.RemoveAllListeners();
            yesButton.onClick.AddListener(OnYesClicked);
        }

        if (noButton != null)
        {
            noButton.onClick.RemoveAllListeners();
            noButton.onClick.AddListener(OnNoClicked);
        }

        if (choicePanel != null)
            choicePanel.SetActive(false);
    }

    private void OnEnable()
    {
        // Buka cursor saat dialog ustad muncul
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private void OnDisable()
    {
        // Kunci lagi saat dialog tutup
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Dipanggil dari UstadChecker, kirim info huruf sudah lengkap atau belum
    public void StartDialogue(bool lettersComplete)
    {
        allLettersCollected = lettersComplete;
        activeLines = allLettersCollected ? completeLines : incompleteLines;

        index = 0;
        waitingForChoice = false;

        if (choicePanel != null)
            choicePanel.SetActive(false);

        textComponent.text = string.Empty;
        StopAllCoroutines();
        StartCoroutine(TypeLine());
    }

    IEnumerator TypeLine()
    {
        foreach (char c in activeLines[index].ToCharArray())
        {
            textComponent.text += c;
            yield return new WaitForSeconds(textSpeed);
        }

        // Kalau sudah di kalimat terakhir → munculkan pilihan
        if (index == activeLines.Length - 1)
        {
            ShowChoice();
        }
    }

    void ShowChoice()
    {
        waitingForChoice = true;

        if (choicePanel != null)
            choicePanel.SetActive(true);
    }

    void HideChoice()
    {
        if (choicePanel != null)
            choicePanel.SetActive(false);
    }

    void NextLine()
    {
        if (index < activeLines.Length - 1)
        {
            index++;
            textComponent.text = string.Empty;
            StopAllCoroutines();
            StartCoroutine(TypeLine());
        }
        else
        {
            // sudah line terakhir → pastikan pilihan muncul
            ShowChoice();
        }
    }

    private void Update()
    {
        if (waitingForChoice) return;

        if (Input.GetMouseButtonDown(0))
        {
            if (textComponent.text == activeLines[index])
            {
                NextLine();
            }
            else
            {
                // skip animasi
                StopAllCoroutines();
                textComponent.text = activeLines[index];

                if (index == activeLines.Length - 1)
                    ShowChoice();
            }
        }
    }

    // ====== TOMBOL ======

    void OnYesClicked()
    {
        HideChoice();
        waitingForChoice = false;

        if (allLettersCollected)
        {
            // kalau huruf lengkap → lanjut level
            StartCoroutine(GoNextScene());
        }
        else
        {
            // huruf belum lengkap → cuma tutup dialog
            gameObject.SetActive(false);
        }
    }

    void OnNoClicked()
    {
        // No = batal, tutup dialog
        HideChoice();
        waitingForChoice = false;
        gameObject.SetActive(false);
    }

    IEnumerator GoNextScene()
    {
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene(nextSceneName);
    }
}
