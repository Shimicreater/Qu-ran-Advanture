using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    [Header("Deteksi Interaksi")]
    [SerializeField] private Transform detectOrigin;      // titik pusat deteksi (biasanya badan player)
    [SerializeField] private float detectRadius = 2f;     // jangkauan interaksi
    [SerializeField] private LayerMask interactLayerMask = ~0;  // layer yang boleh di-interact

    private void Awake()
    {
        // kalau belum diisi di Inspector, pakai posisi player sendiri
        if (detectOrigin == null)
            detectOrigin = transform;
    }

    private void Update()
    {
        // tekan E untuk interaksi
        if (Input.GetKeyDown(KeyCode.E))
        {
            MonoBehaviour interactable = GetInteractableObject();

            if (interactable is NpcInteract npc)
            {
                npc.Interact();
            }
            else if (interactable is UstadInteract ustad)
            {
                ustad.Interact();
            }
        }
    }

    // dipakai PlayerInteractUI untuk cek "ada objek yang bisa di-interact atau tidak"
    public MonoBehaviour GetInteractableObject()
    {
        // cari semua collider di sekitar player
        Collider[] hits = Physics.OverlapSphere(detectOrigin.position, detectRadius, interactLayerMask);

        float closestDist = float.MaxValue;
        MonoBehaviour closest = null;

        foreach (var col in hits)
        {
            // cek NPC biasa dulu
            if (col.TryGetComponent<NpcInteract>(out var npc))
            {
                float d = Vector3.Distance(detectOrigin.position, col.transform.position);
                if (d < closestDist)
                {
                    closestDist = d;
                    closest = npc;
                }
            }
            // cek ustad
            else if (col.TryGetComponent<UstadInteract>(out var ustad))
            {
                float d = Vector3.Distance(detectOrigin.position, col.transform.position);
                if (d < closestDist)
                {
                    closestDist = d;
                    closest = ustad;
                }
            }
        }

        return closest;
    }

    // cuma buat bantu lihat radius di Scene View
    private void OnDrawGizmosSelected()
    {
        if (detectOrigin == null) detectOrigin = transform;

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(detectOrigin.position, detectRadius);
    }
}
