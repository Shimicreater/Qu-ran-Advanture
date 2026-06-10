using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HurufGetPanel : MonoBehaviour
{
    [Header("UI")]
    public GameObject panelHuruf;   // Panel "Selamat, kamu mendapatkan huruf hijaiyah!"

    [Header("SFX")]
    public AudioSource sfxSource;   // AudioSource SFX (SFX_Level1 atau AudioSource ke-2 di BGM_Level1)
    public AudioClip notifClip;     // Suara notif

    // Panggil fungsi ini saat huruf berhasil diambil
    public void ShowPanelHuruf()
    {
        panelHuruf.SetActive(true);

        // mainkan suara notif
        if (sfxSource != null && notifClip != null)
        {
            sfxSource.PlayOneShot(notifClip);
        }

        // kalau mau pause game dsb bisa tambahin di sini
    }

    public void HidePanelHuruf()
    {
        panelHuruf.SetActive(false);
    }
}
