using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapToggle : MonoBehaviour
{
    public GameObject miniMap;
    public GameObject fullMap;
    public GameObject hud;        // semua UI lain yang mau disembunyikan

    private bool isFullMap = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            isFullMap = !isFullMap;

            miniMap.SetActive(!isFullMap);
            fullMap.SetActive(isFullMap);

            if (hud != null)
                hud.SetActive(!isFullMap);
        }
    }
}
