using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcInteract : MonoBehaviour
{
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private int letterIndexToGive = 0;
    private bool letterGiven = false;

    [Header("Animasi NPC")]
    [SerializeField] private Animator npcAnimator;
    private static readonly int TalkingHash = Animator.StringToHash("Talking");

    [Header("Player yang mau di-freeze")]
    [Tooltip("Root object Player (yang punya controller, animator, dsb).")]
    [SerializeField] private GameObject playerRoot;

    [Tooltip("Animator Player (untuk paksa idle).")]
    [SerializeField] private Animator playerAnimator;

    [Tooltip("Script movement player yang menggerakkan jalan/lari (contoh: PlayerMovement, ThirdPersonController).")]
    [SerializeField] private MonoBehaviour[] movementScripts;

    [Tooltip("Rigidbody player (kalau ada, boleh dikosongkan kalau tidak pakai Rigidbody).")]
    [SerializeField] private Rigidbody playerRigidbody;

    // === FLAG GLOBAL: dipakai FreezeCameraOnDialogue ===
    public static bool IsDialogueOpen { get; private set; } = false;
    // ===================================================

    private bool dialogDariNpcIni = false;
    private bool[] prevMovementEnabled;

    private void Awake()
    {
        if (npcAnimator == null)
            npcAnimator = GetComponentInChildren<Animator>();

        if (playerAnimator == null && playerRoot != null)
            playerAnimator = playerRoot.GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        // Kalau dialog ini merasa aktif, tapi panel dialog sudah dimatikan oleh Dialog.cs,
        // berarti dialog selesai → lepas freeze.
        if (dialogDariNpcIni &&
            dialoguePanel != null &&
            !dialoguePanel.activeSelf &&
            IsDialogueOpen)
        {
            SelesaiDialogInternal();
        }
    }

    public void Interact()
    {
        if (dialoguePanel != null)
            dialoguePanel.SetActive(true);

        // NPC mulai ngomong
        SetTalking(true);

        // Freeze movement player
        FreezePlayer();

        // Set flag global
        IsDialogueOpen = true;
        dialogDariNpcIni = true;

        // Unlock huruf sekali
        if (!letterGiven && LetterCollectionManager.Instance != null)
        {
            LetterCollectionManager.Instance.UnlockLetter(letterIndexToGive);
            letterGiven = true;
        }
    }

    public void CloseDialogue()
    {
        if (dialoguePanel != null)
            dialoguePanel.SetActive(false);

        SelesaiDialogInternal();
    }

    private void SelesaiDialogInternal()
    {
        SetTalking(false);
        UnfreezePlayer();
        IsDialogueOpen = false;
        dialogDariNpcIni = false;
    }

    private void SetTalking(bool value)
    {
        if (npcAnimator != null)
            npcAnimator.SetBool(TalkingHash, value);
    }

    private void FreezePlayer()
    {
        if (movementScripts != null && movementScripts.Length > 0)
        {
            if (prevMovementEnabled == null || prevMovementEnabled.Length != movementScripts.Length)
                prevMovementEnabled = new bool[movementScripts.Length];

            for (int i = 0; i < movementScripts.Length; i++)
            {
                var comp = movementScripts[i];
                if (comp == null) continue;

                prevMovementEnabled[i] = comp.enabled;
                comp.enabled = false;   // MATIKAN script movement
            }
        }

        // Matikan momentum
        if (playerRigidbody != null)
        {
            playerRigidbody.velocity = Vector3.zero;
            playerRigidbody.angularVelocity = Vector3.zero;
        }

        // Paksa animasi IDLE (semua param reset)
        if (playerAnimator != null)
        {
            foreach (var p in playerAnimator.parameters)
            {
                switch (p.type)
                {
                    case AnimatorControllerParameterType.Float:
                        playerAnimator.SetFloat(p.name, 0f);
                        break;
                    case AnimatorControllerParameterType.Int:
                        playerAnimator.SetInteger(p.name, 0);
                        break;
                    case AnimatorControllerParameterType.Bool:
                        playerAnimator.SetBool(p.name, false);
                        break;
                        // Trigger diabaikan
                }
            }
        }
    }

    private void UnfreezePlayer()
    {
        if (movementScripts != null &&
            prevMovementEnabled != null &&
            movementScripts.Length == prevMovementEnabled.Length)
        {
            for (int i = 0; i < movementScripts.Length; i++)
            {
                var comp = movementScripts[i];
                if (comp == null) continue;

                comp.enabled = prevMovementEnabled[i];   // balik ke kondisi sebelumnya
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        CloseDialogue();
    }
}
