using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LetterCollectionPanelL2 : MonoBehaviour
{
    // huruf hijaiyah untuk LEVEL 2
    [SerializeField]
    private string[] hijaiyahLettersL2 =
    {
        "ح", // index 0
        "خ", // index 1
        "د", // index 2
        "ذ", // index 3
        "ر"  // index 4
    };

    private LetterSlotUI[] slots;

    private void Awake()
    {
        // cari semua LetterSlotUI di dalam panel ini (termasuk yang inactive)
        slots = GetComponentsInChildren<LetterSlotUI>(true);
    }

    private void OnEnable()
    {
        RefreshSlots();
    }

    public void RefreshSlots()
    {
        if (LetterCollectionManagerL2.Instance == null)
        {
            Debug.LogWarning("[L2Panel] LetterCollectionManagerL2 belum ada di scene.");
            return;
        }

        if (slots == null || slots.Length == 0)
        {
            Debug.LogWarning("[L2Panel] Tidak menemukan LetterSlotUI di anak-anak panel.");
            return;
        }

        foreach (var slot in slots)
        {
            if (slot == null) continue;

            int index = slot.letterIndex;

            // kalau index di luar range huruf, skip
            if (index < 0 || index >= hijaiyahLettersL2.Length)
                continue;

            // set teks huruf hijaiyah level 2
            slot.SetLetter(hijaiyahLettersL2[index]);

            // cek sudah kebuka atau belum
            bool unlocked = LetterCollectionManagerL2.Instance.IsUnlocked(index);
            slot.Refresh(unlocked);
        }
    }
}
