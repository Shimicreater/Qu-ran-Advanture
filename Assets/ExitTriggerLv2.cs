using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitTriggerLv2 : MonoBehaviour
{
    [Tooltip("Nama scene tujuan (contoh: Mainmenu)")]
    public string nextSceneName = "Mainmenu";

    [Header("Koleksi Huruf Hijaiyah")]
    [Tooltip("Berapa huruf yang WAJIB terkumpul sebelum boleh keluar")]
    public int totalLettersRequired = 3;

    [Tooltip("Berapa huruf yang sudah terkumpul (diupdate dari script huruf)")]
    public int lettersCollected = 0;

    /// <summary>
    /// Panggil fungsi ini dari script huruf ketika player mengambil 1 huruf.
    /// </summary>
    public void RegisterLetterCollected()
    {
        lettersCollected++;
        Debug.Log($"[ExitLv2] Huruf terkumpul: {lettersCollected}/{totalLettersRequired}");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        // ========== CEK SUDAH LENGKAP ATAU BELUM ==========
        if (lettersCollected < totalLettersRequired)
        {
            Debug.Log("[ExitLv2] Huruf belum lengkap, lengkapi dulu sebelum keluar!");
            // di sini kalau mau, bisa tampilkan UI peringatan
            // misal: panel teks "Kamu belum mengumpulkan semua huruf."
            return;    // JANGAN ganti scene
        }
        // ==================================================

        Debug.Log("Player menyentuh ExitPad Level 2 (huruf lengkap, lanjut ke scene berikutnya)");

        // Lepaskan cursor dulu supaya di scene berikutnya tidak hilang
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // Kalau sebelumnya ada pause / timeScale diubah, bisa di-reset juga
        Time.timeScale = 1f;

        // Pindah scene
        SceneManager.LoadScene(nextSceneName);
    }
}
