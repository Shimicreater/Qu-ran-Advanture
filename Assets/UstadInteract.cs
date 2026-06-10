using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UstadInteract : MonoBehaviour
{
    [SerializeField] private GameObject dialoguePanel;   // PanelDialogUstad

    private UstadDialog1 ustadDialog;   // pakai UstadDialog1

    [Header("Animasi Ustad")]
    [SerializeField] private Animator ustadAnimator;          // drag Animator Ustad
    [SerializeField] private string talkingBoolName = "Talking"; // nama parameter bool di Animator
    private int talkingHash;

    public static bool IsDialogueOpen { get; private set; } = false;

    private void Awake()
    {
        if (dialoguePanel != null)
        {
            ustadDialog = dialoguePanel.GetComponent<UstadDialog1>();
        }

        if (ustadAnimator == null)
            ustadAnimator = GetComponentInChildren<Animator>();

        if (!string.IsNullOrEmpty(talkingBoolName))
            talkingHash = Animator.StringToHash(talkingBoolName);
    }

    private void Start()
    {
        // PASTIKAN awalnya dialog mati dan ustad diam
        if (dialoguePanel != null)
            dialoguePanel.SetActive(false);

        IsDialogueOpen = false;
        SetTalking(false);
    }

    private void OnDisable()
    {
        // kalau object ini di-disable, paksa diam juga
        IsDialogueOpen = false;
        SetTalking(false);
    }

    public void Interact()
    {
        if (LetterCollectionManager.Instance == null)
        {
            Debug.LogWarning("UstadInteract: LetterCollectionManager belum ada di scene.");
            return;
        }

        if (dialoguePanel == null || ustadDialog == null)
        {
            Debug.LogWarning("UstadInteract: PanelDialogUstad atau UstadDialog1 belum di-assign.");
            return;
        }

        bool allDone = LetterCollectionManager.Instance.AreAllLettersCollected();

        dialoguePanel.SetActive(true);

        SetTalking(true);
        IsDialogueOpen = true;

        ustadDialog.StartDialogue(allDone);
    }

    public void CloseDialogue()
    {
        if (dialoguePanel != null)
            dialoguePanel.SetActive(false);

        SetTalking(false);
        IsDialogueOpen = false;
    }

    private void SetTalking(bool value)
    {
        if (ustadAnimator != null && talkingHash != 0)
        {
            ustadAnimator.SetBool(talkingHash, value);
        }
    }
}
