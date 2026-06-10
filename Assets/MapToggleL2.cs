using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapToggleL2 : MonoBehaviour
{
    [Header("UI Panel")]
    public GameObject miniMapPanel;       // panel minimap (MAP > Image > Minimap)
    public GameObject fullMapPanel;       // panel full map (MAP > Image > fullmap)

    [Header("Extra MiniMap Objects")]
    // isi dengan lingkaran abu-abu / frame minimap, dll
    public GameObject[] extraMiniMapObjects;

    [Header("Input")]
    public KeyCode toggleKey = KeyCode.M; // tombol buka/tutup map

    private bool isFullMap = false;

    void Start()
    {
        ShowMiniMap();
    }

    void Update()
    {
        if (Input.GetKeyDown(toggleKey))
        {
            if (isFullMap) ShowMiniMap();
            else ShowFullMap();
        }
    }

    public void ShowFullMap()
    {
        isFullMap = true;
        if (fullMapPanel) fullMapPanel.SetActive(true);
        SetMiniMapActive(false);
    }

    public void ShowMiniMap()
    {
        isFullMap = false;
        if (fullMapPanel) fullMapPanel.SetActive(false);
        SetMiniMapActive(true);
    }

    private void SetMiniMapActive(bool active)
    {
        if (miniMapPanel) miniMapPanel.SetActive(active);

        if (extraMiniMapObjects != null)
        {
            foreach (var go in extraMiniMapObjects)
            {
                if (go != null) go.SetActive(active);
            }
        }
    }
}
