using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UILogic : MonoBehaviour
{
    [Header("Panels")]
    public GameObject PanelMainMenu;    // Canvas/PanelMainMenu
    public GameObject PanelLevel;       // Canvas/Panel level
    public GameObject PanelPengaturan;  // Canvas/PanelPengaturan
    public GameObject PanelHowToPlay;   // Canvas/HowToPlayPanel

    [Header("Nama Scene Level")]
    public string Level1SceneName = "level 1";
    public string Level2SceneName = "level 2";

    private void Awake()
    {
        Time.timeScale = 1f;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // atur state panel awal
        if (PanelMainMenu != null) PanelMainMenu.SetActive(true);
        if (PanelLevel != null) PanelLevel.SetActive(false);
        if (PanelPengaturan != null) PanelPengaturan.SetActive(false);
        if (PanelHowToPlay != null) PanelHowToPlay.SetActive(false);
    }

    // =========================
    // NAVIGASI ANTAR PANEL
    // =========================

    public void BukaPanelLevel()
    {
        if (PanelMainMenu != null) PanelMainMenu.SetActive(false);
        if (PanelPengaturan != null) PanelPengaturan.SetActive(false);
        if (PanelHowToPlay != null) PanelHowToPlay.SetActive(false);

        if (PanelLevel != null) PanelLevel.SetActive(true);
    }

    public void BukaPanelPengaturan()
    {
        if (PanelMainMenu != null) PanelMainMenu.SetActive(false);
        if (PanelLevel != null) PanelLevel.SetActive(false);
        if (PanelHowToPlay != null) PanelHowToPlay.SetActive(false);

        if (PanelPengaturan != null) PanelPengaturan.SetActive(true);
    }

    public void BukaPanelHowToPlay()
    {
        Debug.Log("BukaPanelHowToPlay dipanggil, panel = " + PanelHowToPlay);

        // MATIKAN PANEL LAIN DULU
        if (PanelMainMenu != null) PanelMainMenu.SetActive(false);
        if (PanelLevel != null) PanelLevel.SetActive(false);
        if (PanelPengaturan != null) PanelPengaturan.SetActive(false);

        // NYALAKAN PANEL HOW TO PLAY
        if (PanelHowToPlay != null)
        {
            PanelHowToPlay.SetActive(true);
        }
        else
        {
            Debug.LogWarning("PanelHowToPlay BELUM di-assign di UILogic");
        }
    }

    public void TutupPanelHowToPlay()
    {
        if (PanelHowToPlay != null)
            PanelHowToPlay.SetActive(false);

        // balik ke main menu (opsional, tapi biasanya enak begitu)
        if (PanelMainMenu != null)
            PanelMainMenu.SetActive(true);
    }

    public void KembaliKeMainMenu()
    {
        if (PanelLevel != null) PanelLevel.SetActive(false);
        if (PanelPengaturan != null) PanelPengaturan.SetActive(false);
        if (PanelHowToPlay != null) PanelHowToPlay.SetActive(false);

        if (PanelMainMenu != null) PanelMainMenu.SetActive(true);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void KembaliDariPengaturan()
    {
        if (PanelPengaturan != null) PanelPengaturan.SetActive(false);
        if (PanelMainMenu != null) PanelMainMenu.SetActive(true);
    }

    // =========================
    // LOAD SCENE LEVEL
    // =========================

    public void MainkanLevel1()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        SceneManager.LoadScene(Level1SceneName);
    }

    public void MainkanLevel2()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        SceneManager.LoadScene(Level2SceneName);
    }

    // =========================
    // KELUAR GAME
    // =========================

    public void KeluarGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
