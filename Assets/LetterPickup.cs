using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LetterPickup : MonoBehaviour
{
    [Header("Index huruf di LetterCollectionManager")]
    public int letterIndex = 0;

    [Header("UI Interaksi (tombol E)")]
    public GameObject interactUI;   // isi dengan objek PressE (child)

    private bool playerInRange = false;

    private void Start()
    {
        // UI E dimatikan dulu
        if (interactUI != null)
            interactUI.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        playerInRange = true;

        if (interactUI != null)
            interactUI.SetActive(true);

        Debug.Log("Player MASUK trigger huruf, siap diambil.");
    }

    // <-- Tambah ini
    private void OnTriggerStay(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        // Selama player masih di dalam, paksa flag tetap true 
        // dan pastikan UI tetap nyala (hindari kedip-kedip)
        if (!playerInRange)
            playerInRange = true;

        if (interactUI != null && !interactUI.activeSelf)
            interactUI.SetActive(true);
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        playerInRange = false;

        if (interactUI != null)
            interactUI.SetActive(false);

        Debug.Log("Player KELUAR trigger huruf.");
    }

    private void Update()
    {
        if (!playerInRange) return;

        // tekan E buat ambil huruf
        if (Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("TOMBOL E ditekan, ambil huruf index: " + letterIndex);

            // tambah ke koleksi
            if (LetterCollectionManager.Instance != null)
            {
                LetterCollectionManager.Instance.UnlockLetter(letterIndex);
            }
            else
            {
                Debug.LogWarning("LetterCollectionManager.Instance = null");
            }

            // matikan UI E
            if (interactUI != null)
                interactUI.SetActive(false);

            // hancurkan huruf dari world
            Destroy(gameObject);
        }
    }
}