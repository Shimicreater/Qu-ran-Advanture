using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Level2GameManager : MonoBehaviour
{
    public static Level2GameManager Instance { get; private set; }

    [Header("Huruf Level 2")]
    public int totalLetters = 3;   // berapa huruf yang wajib diambil
    private int collected = 0;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void AddLetter()
    {
        collected++;
        Debug.Log($"[L2] Letter collected {collected}/{totalLetters}");
        // di sini nanti bisa panggil update UI bar/teks kalau mau
    }

    public bool AllLettersCollected()
    {
        return collected >= totalLetters;
    }

    public void OnPlayerCaught()
    {
        Debug.Log("[L2] Player caught! Restart level.");
        Scene current = SceneManager.GetActiveScene();
        SceneManager.LoadScene(current.buildIndex);
    }

    public void OnPlayerReachExit()
    {
        if (AllLettersCollected())
        {
            Debug.Log("[L2] Level complete!");
            // TODO: ganti ke scene berikutnya atau show win UI
            // SceneManager.LoadScene("NamaSceneBerikutnya");
        }
        else
        {
            Debug.Log("[L2] Belum semua huruf diambil!");
        }
    }
}


