using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIButtonSound : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler
{
    [Header("Setup")]
    [SerializeField] private AudioSource audioSource;

    [Header("Clips")]
    [SerializeField] private AudioClip hoverClip;  // suara saat cursor di atas button
    [SerializeField] private AudioClip clickClip;  // suara saat button di-klik

    // ketika cursor MASUK ke area button
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (audioSource != null && hoverClip != null)
        {
            audioSource.PlayOneShot(hoverClip);
        }
    }

    // ketika button DI-KLIK (klik kiri)
    public void OnPointerClick(PointerEventData eventData)
    {
        if (audioSource != null && clickClip != null)
        {
            audioSource.PlayOneShot(clickClip);
        }
    }
}
