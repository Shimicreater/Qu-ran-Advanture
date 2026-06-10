using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverUI : MonoBehaviour
{
    public string levelSceneName = "level 2";
    public string mainMenuSceneName = "Mainmenu";

    // 👇 TAMBAHKAN INI
    private void Start()
    {
        Cursor.lockState = CursorLockMode.None;  // lepas kunci cursor
        Cursor.visible = true;                   // tampilkan cursor
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(levelSceneName);
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene(mainMenuSceneName);
    }
}