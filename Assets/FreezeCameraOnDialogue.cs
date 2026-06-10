using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;


public class FreezeCameraOnDialogue : MonoBehaviour
    {
        [Header("Panel Dialog yang Membekukan Kamera")]
        [Tooltip("Drag semua panel dialog ke sini, misal: PanelDialog (NPC), PanelDialogUstad, dll.")]
        public GameObject[] dialogPanels;

        private CinemachineFreeLook freeLook;
        private float defaultXSpeed;
        private float defaultYSpeed;
        private bool initialized = false;
        private bool isFrozen = false;

        private void Awake()
        {
            freeLook = GetComponent<CinemachineFreeLook>();
            if (freeLook != null)
            {
                defaultXSpeed = freeLook.m_XAxis.m_MaxSpeed;
                defaultYSpeed = freeLook.m_YAxis.m_MaxSpeed;
                initialized = true;
            }
            else
            {
                Debug.LogWarning("FreezeCameraOnDialogue: CinemachineFreeLook tidak ditemukan di GameObject ini.");
            }
        }

        private void LateUpdate()
        {
            if (!initialized) return;

            bool anyDialogOpen = IsAnyDialogOpen();

            if (anyDialogOpen && !isFrozen)
            {
                // Bekukan kamera
                freeLook.m_XAxis.m_MaxSpeed = 0f;
                freeLook.m_YAxis.m_MaxSpeed = 0f;
                isFrozen = true;
            }
            else if (!anyDialogOpen && isFrozen)
            {
                // Balikkan ke speed normal
                freeLook.m_XAxis.m_MaxSpeed = defaultXSpeed;
                freeLook.m_YAxis.m_MaxSpeed = defaultYSpeed;
                isFrozen = false;
            }
        }

        private bool IsAnyDialogOpen()
        {
            if (dialogPanels == null) return false;

            foreach (var panel in dialogPanels)
            {
                if (panel != null && panel.activeSelf)
                    return true;
            }

            return false;
        }
    }

