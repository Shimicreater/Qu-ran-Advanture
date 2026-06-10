using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectionUIController : MonoBehaviour
{
    private GameObject mapObject;      // objek "MAP" di Hierarchy
    private GameObject interaksiUI;    // objek "Interaksi" di Hierarchy

    private bool prevMapActive;
    private bool prevInteraksiActive;

    private void OnEnable()
    {
        // cari objek MAP dan Interaksi di scene
        mapObject = GameObject.Find("MAP");
        interaksiUI = GameObject.Find("Interaksi");

        if (mapObject != null)
        {
            prevMapActive = mapObject.activeSelf;
            mapObject.SetActive(false);
        }

        if (interaksiUI != null)
        {
            prevInteraksiActive = interaksiUI.activeSelf;
            interaksiUI.SetActive(false);
        }

        // cursor bebas saat panel koleksi dibuka
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private void OnDisable()
    {
        // balikin kondisi awal MAP & Interaksi
        if (mapObject != null)
            mapObject.SetActive(prevMapActive);

        if (interaksiUI != null)
            interaksiUI.SetActive(prevInteraksiActive);

        // kunci lagi cursor saat panel koleksi ditutup
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
