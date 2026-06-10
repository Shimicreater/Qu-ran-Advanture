using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LetterPickupL2 : MonoBehaviour
{
    [Header("Index huruf level 2 (0–4)")]
    public int LetterIndex = 0;          // index huruf untuk manager level 2

    [Header("UI Interaksi (Press E)")]
    public GameObject InteractUI;        // text "Press E to pick"

    [Header("SFX Pickup")]
    public AudioClip pickupSfx;          // suara notif ketika huruf diambil

    private bool isInRange = false;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        isInRange = true;
        if (InteractUI != null)
            InteractUI.SetActive(true);
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        isInRange = false;
        if (InteractUI != null)
            InteractUI.SetActive(false);
    }

    private void Update()
    {
        if (!isInRange) return;

        // tekan E untuk ambil
        if (Input.GetKeyDown(KeyCode.E))
        {
            Pickup();
        }
    }

    private void Pickup()
    {
        // 1. Beritahu manager huruf level 2
        if (LetterCollectionManagerL2.Instance != null)
        {
            LetterCollectionManagerL2.Instance.UnlockLetter(LetterIndex);
        }
        else
        {
            Debug.LogWarning("[PickupL2] LetterCollectionManagerL2 belum ada.");
        }

        // 2. (Opsional) beritahu ExitTriggerLv2 supaya counter naik
        ExitTriggerLv2 exit = FindObjectOfType<ExitTriggerLv2>();
        if (exit != null)
        {
            exit.RegisterLetterCollected();
        }

        // 3. MAIN-KAN SUARA NOTIF
        if (pickupSfx != null)
        {
            AudioSource.PlayClipAtPoint(pickupSfx, transform.position);
        }

        // 4. Matikan UI & hurufnya
        if (InteractUI != null)
            InteractUI.SetActive(false);

        gameObject.SetActive(false);  // atau Destroy(gameObject);
    }
}
