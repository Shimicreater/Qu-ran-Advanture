using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class QuizUIController : MonoBehaviour
{
    [Header("Feedback Quiz (Kalau Salah)")]
    public TextMeshProUGUI feedbackText;      // Feedback salah (Text TMP)
    public int letterIndexToUnlock = 0;       // index huruf yang dibuka kalau benar

    [Header("Elemen Soal & Jawaban")]
    public GameObject soalObject;             // GameObject "Soal"
    public GameObject[] jawabanButtons;       // Jawaban 1,2,3,4

    // objek akan dicari otomatis
    private GameObject mapObject;             // "MAP"
    private GameObject interaksiUI;           // "Interaksi"

    private bool prevMapActive;
    private bool prevInteraksiActive;
    private bool waitingRetry = false;

    [Header("Animasi Reward (Kalau Benar)")]
    public RectTransform rewardPanel;         // RewardPanel (panel biru hadiah)
    public RectTransform rewardIcon;          // RewardLetterIcon (ikon huruf)
    public TextMeshProUGUI rewardTitleText;   // RewardTitleText (Text TMP di atas)
    public float rewardPopupTime = 0.35f;
    public float rewardStayTime = 1f;

    [Header("Panel & Notif Huruf")]
    public HurufGetPanel hurufGetPanel;       // script untuk panel "Selamat..." + suara notif

    private void OnEnable()
    {
        // cari MAP + Interaksi di hierarchy
        mapObject = GameObject.Find("MAP");
        interaksiUI = GameObject.Find("Interaksi");

        if (mapObject != null)
        {
            prevMapActive = mapObject.activeSelf;
            mapObject.SetActive(false);
        }

        if (interaksiUI != null)
        {
            prevInteraksiActive = interaksiUI.activeSelf;
            interaksiUI.SetActive(false);
        }

        // BEBASKAN CURSOR SAAT QUIZ MUNCUL
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // reset tampilan quiz
        ShowQuestion(true);
        if (feedbackText != null)
        {
            feedbackText.text = "";
            feedbackText.gameObject.SetActive(true);
        }

        waitingRetry = false;

        // pastikan panel reward & ikon off di awal
        if (rewardPanel != null)
            rewardPanel.gameObject.SetActive(false);
        if (rewardIcon != null)
            rewardIcon.localScale = Vector3.zero;
    }

    private void OnDisable()
    {
        if (mapObject != null) mapObject.SetActive(prevMapActive);
        if (interaksiUI != null) interaksiUI.SetActive(prevInteraksiActive);

        // KUNCI CURSOR LAGI SAAT QUIZ TUTUP
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void CloseQuiz()
    {
        gameObject.SetActive(false);
    }

    // ---------------- LOGIKA JAWABAN ----------------

    public void AnswerCorrect()
    {
        // sembunyikan soal & jawaban
        ShowQuestion(false);

        // feedbackText tidak dipakai untuk benar → kosongkan & sembunyikan
        if (feedbackText != null)
        {
            feedbackText.text = "";
            feedbackText.gameObject.SetActive(false);
        }

        // unlock huruf
        if (LetterCollectionManager.Instance != null)
        {
            LetterCollectionManager.Instance.UnlockLetter(letterIndexToUnlock);
        }

        // TAMPILKAN PANEL "SELAMAT..." + SUARA NOTIF
        if (hurufGetPanel != null)
        {
            hurufGetPanel.ShowPanelHuruf();
        }

        // animasi hadiah kalau ada panel & ikon (boleh pakai panel yang sama dengan HurufGetPanel)
        if (rewardPanel != null && rewardIcon != null)
            StartCoroutine(ShowRewardAnimation());
        else
            Invoke(nameof(CloseQuiz), 1.2f);
    }

    public void AnswerWrong()
    {
        // sembunyikan soal & jawaban, hanya feedback salah
        ShowQuestion(false);

        if (feedbackText != null)
        {
            feedbackText.gameObject.SetActive(true);
            feedbackText.text = "Jawaban salah, coba lagi!";
        }

        // tunggu klik untuk ulangi
        waitingRetry = true;
    }

    private void Update()
    {
        // PAKSA cursor tetap bebas selama panel Quiz aktif
        if (Cursor.lockState != CursorLockMode.None)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        // kalau lagi mode "feedback salah" dan user klik → tampilkan lagi soal
        if (waitingRetry && Input.GetMouseButtonDown(0))
        {
            waitingRetry = false;
            ShowQuestion(true);
            if (feedbackText != null)
            {
                feedbackText.text = "";
                feedbackText.gameObject.SetActive(true);
            }
        }
    }

    // ---------------- FUNGSI BANTU ----------------

    private void ShowQuestion(bool show)
    {
        if (soalObject != null)
            soalObject.SetActive(show);

        if (jawabanButtons != null)
        {
            foreach (GameObject btn in jawabanButtons)
            {
                if (btn != null) btn.SetActive(show);
            }
        }
    }

    private IEnumerator ShowRewardAnimation()
    {
        // aktifkan panel hadiah
        rewardPanel.gameObject.SetActive(true);

        // set teks judul hadiah
        if (rewardTitleText != null)
        {
            rewardTitleText.text = "Selamat, kamu mendapatkan huruf hijaiyah!";
        }

        // mulai dari scale 0 → 1 (ikon saja yang pop)
        rewardIcon.localScale = Vector3.zero;

        float t = 0f;
        while (t < rewardPopupTime)
        {
            t += Time.deltaTime;
            float progress = t / rewardPopupTime;

            // efek "ease out" + sedikit overshoot
            float scale = Mathf.SmoothStep(0f, 1.1f, progress);
            rewardIcon.localScale = Vector3.one * scale;
            yield return null;
        }

        rewardIcon.localScale = Vector3.one;

        // diam sebentar biar player sempat lihat
        yield return new WaitForSeconds(rewardStayTime);

        // tutup quiz
        CloseQuiz();
    }
}
