using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LetterCollectionManager : MonoBehaviour
{
    public static LetterCollectionManager Instance { get; private set; }

    [Header("Jumlah huruf yang dipakai")]
    public int letterCount = 5;   // harus sama dengan jumlah huruf di panel

    // array status huruf (true = sudah keambil)
    private bool[] unlockedLetters;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        unlockedLetters = new bool[letterCount];
    }

    // Dipanggil oleh LetterPickup ketika huruf diambil
    public void UnlockLetter(int index)
    {
        if (index < 0 || index >= unlockedLetters.Length)
            return;

        unlockedLetters[index] = true;
        Debug.Log($"Huruf index {index} di-unlock.");
    }

    // ✅ INI FUNGSI BARU yang dibutuhkan oleh LetterCollectionPanel
    public bool IsUnlocked(int index)
    {
        if (index < 0 || index >= unlockedLetters.Length)
            return false;

        return unlockedLetters[index];
    }

    // Dipakai oleh UstadChecker buat cek semua huruf sudah lengkap atau belum
    public bool AreAllLettersCollected()
    {
        for (int i = 0; i < unlockedLetters.Length; i++)
        {
            if (!unlockedLetters[i])
                return false;
        }
        return true;
    }
}   
