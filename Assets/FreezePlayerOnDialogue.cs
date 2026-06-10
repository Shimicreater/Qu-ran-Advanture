using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreezePlayerOnDialogue : MonoBehaviour
{
    [Header("Panel Dialog yang Membekukan Player")]
    [Tooltip("Drag semua panel dialog ke sini, misal: PanelDialog (NPC), PanelDialogUstad, dll.")]
    public GameObject[] dialogPanels;

    [Header("Script Movement Player yang akan DIMATIKAN saat dialog")]
    [Tooltip("Drag script gerak player, misal: PlayerMovement, ThirdPersonController, dsb.")]
    public MonoBehaviour[] movementScripts;

    [Header("Komponen Player Opsional")]
    public Animator playerAnimator;      // drag Animator Player (boleh kosong tapi lebih bagus diisi)
    public Rigidbody playerRigidbody;    // drag Rigidbody Player kalau ada (boleh kosong)

    private bool isFrozen = false;
    private bool[] movementPrevEnabled;

    private void Awake()
    {
        if (movementScripts != null && movementScripts.Length > 0)
            movementPrevEnabled = new bool[movementScripts.Length];

        if (playerAnimator == null)
            playerAnimator = GetComponentInChildren<Animator>();

        if (playerRigidbody == null)
            playerRigidbody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        bool anyDialogOpen = IsAnyDialogOpen();

        if (anyDialogOpen && !isFrozen)
        {
            FreezePlayer();
        }
        else if (!anyDialogOpen && isFrozen)
        {
            UnfreezePlayer();
        }
    }

    private bool IsAnyDialogOpen()
    {
        if (dialogPanels == null) return false;

        foreach (var panel in dialogPanels)
        {
            if (panel != null && panel.activeSelf)
                return true;
        }

        return false;
    }

    private void FreezePlayer()
    {
        // 1. Matikan script movement
        if (movementScripts != null)
        {
            if (movementPrevEnabled == null || movementPrevEnabled.Length != movementScripts.Length)
                movementPrevEnabled = new bool[movementScripts.Length];

            for (int i = 0; i < movementScripts.Length; i++)
            {
                var comp = movementScripts[i];
                if (comp == null) continue;

                movementPrevEnabled[i] = comp.enabled;
                comp.enabled = false;
            }
        }

        // 2. Nolkan Rigidbody
        if (playerRigidbody != null)
        {
            playerRigidbody.velocity = Vector3.zero;
            playerRigidbody.angularVelocity = Vector3.zero;
        }

        // 3. Paksa animasi ke IDLE (reset semua param basik)
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
                }
            }
        }

        isFrozen = true;
    }

    private void UnfreezePlayer()
    {
        // Balikan status script movement
        if (movementScripts != null &&
            movementPrevEnabled != null &&
            movementPrevEnabled.Length == movementScripts.Length)
        {
            for (int i = 0; i < movementScripts.Length; i++)
            {
                var comp = movementScripts[i];
                if (comp == null) continue;

                comp.enabled = movementPrevEnabled[i];
            }
        }

        isFrozen = false;
    }
}

