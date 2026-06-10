using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectionToggle : MonoBehaviour
{
    [SerializeField] private GameObject collectionPanel;

    void Start()
    {
        if (collectionPanel != null)
        {
            collectionPanel.SetActive(false);   // panel koleksi tertutup saat mulai
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            if (collectionPanel == null) return;

            bool active = !collectionPanel.activeSelf;
            collectionPanel.SetActive(active);
        }
    }
}
