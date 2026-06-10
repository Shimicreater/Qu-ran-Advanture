using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Dialog : MonoBehaviour
{
    [Header("UI Dialog")]
    public TextMeshProUGUI textComponent;
    public string[] lines;
    public float textSpeed = 0.02f;

    [Header("Pilihan Ya / Tidak")]
    public GameObject choicePanel;     // panel yang berisi tombol Iya & Nanti/Tidak
    public Button yesButton;
    public Button noButton;

    [Header("Pengaturan Pertanyaan")]
    public int firstQuestionIndex = 0;   // "Halo, Afiq apakah ada yg bisa di bantu"
    public int secondQuestionIndex = 1;  // "bisa banget aku ada sedikit quiz..."

    [Header("Panel Quiz")]
    public GameObject quizPanel;        // panel quiz yang mau ditampilkan saat Yes di pertanyaan ke-2

    private int index;
    private bool waitingForChoice = false;

    private void Awake()
    {
        // pasang listener tombol
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

        // panel pilihan awalnya mati
        if (choicePanel != null)
            choicePanel.SetActive(false);
    }

    private void OnEnable()
    {
        // BUKA CURSOR SAAT DIALOG MUNCUL
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        StartDialogue();
    }

    private void OnDisable()
    {
        // KUNCI CURSOR LAGI SAAT DIALOG TUTUP
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void StartDialogue()
    {
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
        foreach (char c in lines[index].ToCharArray())
        {
            textComponent.text += c;
            yield return new WaitForSeconds(textSpeed);
        }

        // kalau line ini termasuk pertanyaan, munculkan pilihan
        if (index == firstQuestionIndex || index == secondQuestionIndex)
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
        if (index < lines.Length - 1)
        {
            index++;
            textComponent.text = string.Empty;
            StopAllCoroutines();
            StartCoroutine(TypeLine());
        }
        else
        {
            gameObject.SetActive(false);   // selesai dialog
        }
    }

    void Update()
    {
        // SELAMA DIALOG AKTIF, PAKSA CURSOR BEBAS
        if (Cursor.lockState != CursorLockMode.None)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        // kalau lagi nunggu pilihan, klik mouse diabaikan
        if (waitingForChoice) return;

        if (Input.GetMouseButtonDown(0))
        {
            if (textComponent.text == lines[index])
            {
                NextLine();
            }
            else
            {
                // skip animasi
                StopAllCoroutines();
                textComponent.text = lines[index];

                // kalau line ini adalah pertanyaan, tetap munculkan pilihan
                if (index == firstQuestionIndex || index == secondQuestionIndex)
                {
                    ShowChoice();
                }
            }
        }
    }

    // ==== tombol ====

    void OnYesClicked()
    {
        // Yes bisa beda aksi tergantung kita ada di line berapa
        if (index == firstQuestionIndex)
        {
            // Dari pertanyaan pertama → lanjut ke kalimat "bisa banget..."
            HideChoice();
            waitingForChoice = false;
            NextLine();
        }
        else if (index == secondQuestionIndex)
        {
            // Dari pertanyaan kedua → buka quiz
            HideChoice();
            waitingForChoice = false;

            if (quizPanel != null)
                quizPanel.SetActive(true);   // munculkan panel quiz

            gameObject.SetActive(false);      // tutup dialog
        }
        else
        {
            // guard, kalau kepanggil di line lain
            HideChoice();
            waitingForChoice = false;
        }
    }

    void OnNoClicked()
    {
        // No di kedua pertanyaan sama: keluar dari dialog
        HideChoice();
        waitingForChoice = false;
        gameObject.SetActive(false);
    }
}
