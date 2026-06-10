using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteractUI : MonoBehaviour
{
    [SerializeField] private GameObject containerGameObject; // icon E (Container)
    [SerializeField] private PlayerInteract playerInteract;

    [Header("Panel dialog NPC")]
    [SerializeField] private GameObject dialoguePanel1;   // PanelDialog (NPC 1)
    [SerializeField] private GameObject dialoguePanel2;   // PanelDialog2 (NPC 2)
    [SerializeField] private GameObject dialoguePanel3;   // PanelDialog3 (NPC 3)

    [Header("Panel dialog ustad")]
    [SerializeField] private GameObject ustadDialoguePanel;   // PanelDialogUstad

    private void Update()
    {
        // cek apakah ada dialog yang lagi aktif
        bool anyDialogOn =
            IsOn(dialoguePanel1) ||
            IsOn(dialoguePanel2) ||
            IsOn(dialoguePanel3) ||
            IsOn(ustadDialoguePanel);

        // kalau ada dialog aktif → sembunyikan icon E
        if (anyDialogOn)
        {
            Hide();
            return;
        }

        // kalau dekat objek yang bisa di-interact → tampilkan E
        if (playerInteract != null && playerInteract.GetInteractableObject() != null)
        {
            Show();
        }
        else
        {
            Hide();
        }
    }

    private bool IsOn(GameObject go)
    {
        return go != null && go.activeSelf;
    }

    private void Show()
    {
        if (containerGameObject != null)
            containerGameObject.SetActive(true);
    }

    private void Hide()
    {
        if (containerGameObject != null)
            containerGameObject.SetActive(false);
    }
}
