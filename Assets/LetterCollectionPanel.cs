using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LetterCollectionPanel : MonoBehaviour
{
    // 5 huruf hijaiyah yang dipakai
    [SerializeField]
    private string[] hijaiyahLetters =
    {
        "ا", // index 0
        "ب", // index 1
        "ت", // index 2
        "", // index 3
        "ج"  // index 4
    };

    private LetterSlotUI[] slots;

    private void Awake()
    {
        // cari semua LetterSlotUI di dalam panel ini
        slots = GetComponentsInChildren<LetterSlotUI>(true);
    }

    private void OnEnable()
    {
        RefreshSlots();
    }

    public void RefreshSlots()
    {
        if (LetterCollectionManager.Instance == null)
        {
            Debug.LogWarning("LetterCollectionManager belum ada.");
            return;
        }

        foreach (var slot in slots)
        {
            int index = slot.letterIndex;

            // kalau index di luar range huruf, skip
            if (index < 0 || index >= hijaiyahLetters.Length)
                continue;

            // set teks huruf hijaiyah
            slot.SetLetter(hijaiyahLetters[index]);

            // cek sudah kebuka atau belum
            bool unlocked = LetterCollectionManager.Instance.IsUnlocked(index);
            slot.Refresh(unlocked);
        }
    }
}
