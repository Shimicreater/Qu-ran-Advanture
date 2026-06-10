using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LetterCollectionManagerL2 : MonoBehaviour
{
    public static LetterCollectionManagerL2 Instance { get; private set; }

    [Header("Jumlah huruf yang dipakai di Level 2")]
    public int letterCount = 5;   // harus sama dengan jumlah huruf di panel level 2

    private bool[] unlockedLetters;

    private void Awake()
    {
        // singleton
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        // selalu mulai dari kosong (semua false)
        unlockedLetters = new bool[letterCount];
        for (int i = 0; i < letterCount; i++)
        {
            unlockedLetters[i] = false;
        }
    }

    public void UnlockLetter(int index)
    {
        if (index < 0 || index >= unlockedLetters.Length)
        {
            Debug.LogWarning($"[L2Mgr] Index {index} di luar range.");
            return;
        }

        unlockedLetters[index] = true;
        Debug.Log($"[L2Mgr] Unlock letter index {index}");
    }

    public bool IsUnlocked(int index)
    {
        if (index < 0 || index >= unlockedLetters.Length)
            return false;

        return unlockedLetters[index];
    }

    public bool AreAllLettersCollected()
    {
        for (int i = 0; i < unlockedLetters.Length; i++)
        {
            if (!unlockedLetters[i]) return false;
        }
        return true;
    }
}




