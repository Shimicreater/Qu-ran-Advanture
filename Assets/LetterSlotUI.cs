using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;     // <-- TAMBAHKAN INI
using TMPro;

public class LetterSlotUI : MonoBehaviour
{
    public int letterIndex;
    public TextMeshProUGUI letterText;
    public Image backgroundImage;

    public Color lockedColor = Color.gray;
    public Color unlockedColor = Color.white;

    public void SetLetter(string text)
    {
        if (letterText != null)
            letterText.text = text;
    }

    public void Refresh(bool unlocked)
    {
        if (backgroundImage != null)
            backgroundImage.color = unlocked ? unlockedColor : lockedColor;
    }
}
